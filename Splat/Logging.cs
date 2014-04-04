using System;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Splat
{
    /*    
     * Interfaces
     */

    public enum LogLevel {
        Debug = 1, Info, Warn, Error, 
    }

    public interface ILogger
    {
        void Write(LogLevel logLevel, string message);
        LogLevel Level { get; set; }
    }

    public interface IFullLogger : ILogger
    {
        void Debug<T>(T value);
        void DebugException(string message, Exception exception);
        void Debug(string message);
        void Debug(string message, params object[] args);
        void Debug<TArgument>(string message, TArgument argument);
        void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        void Info<T>(T value);
        void InfoException(string message, Exception exception);
        void Info(string message);
        void Info(string message, params object[] args);
        void Info<TArgument>(string message, TArgument argument);
        void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        void Warn<T>(T value);
        void WarnException(string message, Exception exception);
        void Warn(string message);
        void Warn(string message, params object[] args);
        void Warn<TArgument>(string message, TArgument argument);
        void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        void Error<T>(T value);
        void ErrorException(string message, Exception exception);
        void Error(string message);
        void Error(string message, params object[] args);
        void Error<TArgument>(string message, TArgument argument);
        void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
    }

    public interface ILogManager
    {
        IFullLogger GetLogger(Type type);
    }

    public class DefaultLogManager : ILogManager
    {
        readonly MemoizingMRUCache<Type, IFullLogger> loggerCache;

        public DefaultLogManager(IDependencyResolver dependencyResolver = null)
        {
            dependencyResolver = dependencyResolver ?? Locator.Current;

            loggerCache = new MemoizingMRUCache<Type, IFullLogger>((type, _) => {
                var ret = dependencyResolver.GetService<ILogger>();
                if (ret == null) {
                    throw new Exception("Couldn't find an ILogger. This should never happen, your dependency resolver is probably broken.");
                }

                return new WrappingFullLogger(ret, type);
            }, 64);
        }

        static readonly IFullLogger nullLogger = new WrappingFullLogger(new NullLogger(), typeof(MemoizingMRUCache<Type, IFullLogger>));
        public IFullLogger GetLogger(Type type)
        {
            if (LogHost.suppressLogging) return nullLogger;
            if (type == typeof(MemoizingMRUCache<Type, IFullLogger>)) return nullLogger;

            lock (loggerCache) {
                return loggerCache.Get(type);
            }
        }
    }

    public class FuncLogManager : ILogManager
    {
        readonly Func<Type, IFullLogger> _inner;
        public FuncLogManager(Func<Type, IFullLogger> getLogger)
        {
            _inner = getLogger;
        }

        public IFullLogger GetLogger(Type type)
        {
            return _inner(type);
        }
    }

    public static class LogManagerMixin
    {
        public static IFullLogger GetLogger<T>(this ILogManager This)
        {
            return This.GetLogger(typeof(T));
        }
    }

    public class NullLogger : ILogger
    {
        public void Write(LogLevel logLevel, string message) { }
        public LogLevel Level { get; set; }
    }

    public class DebugLogger : ILogger
    {
        public void Write(LogLevel logLevel, string message)
        {
            if ((int)logLevel < (int)Level) return;
            Debug.WriteLine(message);
        }

        public LogLevel Level { get; set; }
    }


    /*    
     * LogHost / Logging Mixin
     */

    /// <summary>
    /// "Implement" this interface in your class to get access to the Log() 
    /// Mixin, which will give you a Logger that includes the class name in the
    /// log.
    /// </summary>
    public interface IEnableLogger { }

    public static class LogHost
    {
        static internal bool suppressLogging = false;
        static readonly IFullLogger nullLogger = new WrappingFullLogger(new NullLogger(), typeof(string));

        /// <summary>
        /// Use this logger inside miscellaneous static methods where creating
        /// a class-specific logger isn't really worth it.
        /// </summary>
        public static IFullLogger Default {
            get {
                if (suppressLogging) return nullLogger;

                var factory = Locator.Current.GetService<ILogManager>();
                if (factory == null) {
                    throw new Exception("ILogManager is null. This should never happen, your dependency resolver is broken");
                }
                return factory.GetLogger(typeof(LogHost));
            }
        }

        /// <summary>
        /// Call this method to write log entries on behalf of the current 
        /// class.
        /// </summary>
        public static IFullLogger Log<T>(this T This) where T : IEnableLogger
        {
            if (suppressLogging) return nullLogger;

            var factory = Locator.Current.GetService<ILogManager>();
            if (factory == null) {
                throw new Exception("ILogManager is null. This should never happen, your dependency resolver is broken");
            }

            return factory.GetLogger<T>();
        }
    }

    #region Extremely Dull Code Ahead
    public class WrappingFullLogger : IFullLogger
    {
        readonly ILogger _inner;
        readonly string prefix;
        readonly MethodInfo stringFormat;

        public WrappingFullLogger(ILogger inner, Type callingType)
        {
            _inner = inner;
            prefix = String.Format(CultureInfo.InvariantCulture, "{0}: ", callingType.Name);

            Contract.Requires(inner != null);
            Contract.Requires(stringFormat != null);
        }

        public void Debug<T>(T value)
        {
            _inner.Write(LogLevel.Debug, prefix + value);
        }

        public void DebugException(string message, Exception exception)
        {
            _inner.Write(LogLevel.Debug, String.Format("{0}{1}: {2}", prefix, message, exception));
        }

        public void Debug(string message)
        {
            _inner.Write(LogLevel.Debug, prefix + message);
        }

        public void Debug(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(LogLevel.Debug, prefix + result);
        }

        public void Debug<TArgument>(string message, TArgument argument)
        {
            _inner.Write(LogLevel.Debug, prefix + String.Format(CultureInfo.InvariantCulture, message, argument));
        }

        public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(LogLevel.Debug, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2));
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(LogLevel.Debug, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3));
        }

        public void Info<T>(T value)
        {
            _inner.Write(LogLevel.Info, prefix + value);
        }

        public void InfoException(string message, Exception exception)
        {
            _inner.Write(LogLevel.Info, String.Format("{0}{1}: {2}", prefix, message, exception));
        }

        public void Info(string message)
        {
            _inner.Write(LogLevel.Info, prefix + message);
        }

        public void Info(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(LogLevel.Info, prefix + result);
        }

        public void Info<TArgument>(string message, TArgument argument)
        {
            _inner.Write(LogLevel.Info, prefix + String.Format(CultureInfo.InvariantCulture, message, argument));
        }

        public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(LogLevel.Info, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2));
        }

        public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(LogLevel.Info, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3));
        }

        public void Warn<T>(T value)
        {
            _inner.Write(LogLevel.Warn, prefix + value);
        }

        public void WarnException(string message, Exception exception)
        {
            _inner.Write(LogLevel.Warn, String.Format("{0}{1}: {2}", prefix, message, exception));
        }

        public void Warn(string message)
        {
            _inner.Write(LogLevel.Warn, prefix + message);
        }

        public void Warn(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(LogLevel.Warn, prefix + result);
        }

        public void Warn<TArgument>(string message, TArgument argument)
        {
            _inner.Write(LogLevel.Warn, prefix + String.Format(CultureInfo.InvariantCulture, message, argument));
        }

        public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(LogLevel.Warn, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2));
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(LogLevel.Warn, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3));
        }


        public void Error<T>(T value)
        {
            _inner.Write(LogLevel.Error, prefix + value);
        }

        public void ErrorException(string message, Exception exception)
        {
            _inner.Write(LogLevel.Error, String.Format("{0}{1}: {2}", prefix, message, exception));
        }

        public void Error(string message)
        {
            _inner.Write(LogLevel.Error, prefix + message);
        }

        public void Error(string message, params object[] args)
        {
            var result = InvokeStringFormat(CultureInfo.InvariantCulture, message, args);
            _inner.Write(LogLevel.Error, prefix + result);
        }

        public void Error<TArgument>(string message, TArgument argument)
        {
            _inner.Write(LogLevel.Error, prefix + String.Format(CultureInfo.InvariantCulture, message, argument));
        }

        public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            _inner.Write(LogLevel.Error, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2));
        }

        public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            _inner.Write(LogLevel.Error, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3));
        }

        public LogLevel Level {
            get { return _inner.Level; }
            set { _inner.Level = value; }
        }
    }
    #endregion
}

// vim: tw=120 ts=4 sw=4 et :
