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
        ILogEntry Write(LogLevel logLevel, string message);
        LogLevel Level { get; set; }
    }

    public interface ILogEntry : IDisposable
    {
        ILogger Logger { get; }
        long MessageId { get; }
    }

    public interface IFullLogger : ILogger
    {
        ILogEntry Debug<T>(T value);
        ILogEntry DebugException(string message, Exception exception);
        ILogEntry Debug(string message);
        ILogEntry Debug(string message, params object[] args);
        ILogEntry Debug<TArgument>(string message, TArgument argument);
        ILogEntry Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        ILogEntry Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        ILogEntry Info<T>(T value);
        ILogEntry InfoException(string message, Exception exception);
        ILogEntry Info(string message);
        ILogEntry Info(string message, params object[] args);
        ILogEntry Info<TArgument>(string message, TArgument argument);
        ILogEntry Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        ILogEntry Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        ILogEntry Warn<T>(T value);
        ILogEntry WarnException(string message, Exception exception);
        ILogEntry Warn(string message);
        ILogEntry Warn(string message, params object[] args);
        ILogEntry Warn<TArgument>(string message, TArgument argument);
        ILogEntry Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        ILogEntry Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        ILogEntry Error<T>(T value);
        ILogEntry ErrorException(string message, Exception exception);
        ILogEntry Error(string message);
        ILogEntry Error(string message, params object[] args);
        ILogEntry Error<TArgument>(string message, TArgument argument);
        ILogEntry Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);
        ILogEntry Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        ILogEntry EnterSpan(string message);
        ILogEntry EnterSpan(string message, params object[] args);
        ILogEntry EnterSpan(LogLevel level, string message);
        ILogEntry EnterSpan(LogLevel logLevel, string message, params object[] args);
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
        static LogEntry emptyEntry = new LogEntry();

        public ILogEntry Write(LogLevel logLevel, string message) { return emptyEntry; }
        public LogLevel Level { get; set; }
    }

    public class DebugLogger : ILogger
    {
        static LogEntry emptyEntry = new LogEntry();

        public ILogEntry Write(LogLevel logLevel, string message)
        {
            if ((int)logLevel < (int)Level) return emptyEntry;
            Debug.WriteLine(message);

            return new LogEntry(emptyEntry);
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

    public sealed class LogEntry : ILogEntry, IDisposable
    {
        public ILogger Logger { get; set; }
        public long MessageId { get; set; }

        IDisposable inner = ActionDisposable.Empty;
        public LogEntry(IDisposable onDispose = null)
        {
            inner = onDispose;
        }

        public void Dispose() 
        {
            Interlocked.Exchange(ref inner, ActionDisposable.Empty).Dispose();
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
            stringFormat = typeof (String).GetMethod("Format", new[] {typeof (IFormatProvider), typeof (string), typeof (object[])});

            Contract.Requires(inner != null);
            Contract.Requires(stringFormat != null);
        }

        public ILogEntry Write(LogLevel logLevel, string message)
        {
            return _inner.Write(logLevel, message);
        }

        public ILogEntry Debug<T>(T value)
        {
            return _inner.Write(LogLevel.Debug, prefix + value);
        }

        public ILogEntry DebugException(string message, Exception exception)
        {
            return _inner.Write(LogLevel.Debug, String.Format("{0}{1}: {2}", prefix, message, exception));
        }

        public ILogEntry Debug(string message)
        {
            return _inner.Write(LogLevel.Debug, prefix + message);
        }

        public ILogEntry Debug(string message, params object[] args)
        {
            var result = invokeStringFormat(CultureInfo.InvariantCulture, message, args);
            return _inner.Write(LogLevel.Debug, prefix + result);
        }

        public ILogEntry Debug<TArgument>(string message, TArgument argument)
        {
            return _inner.Write(LogLevel.Debug, prefix + String.Format(CultureInfo.InvariantCulture, message, argument));
        }

        public ILogEntry Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            return _inner.Write(LogLevel.Debug, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2));
        }

        public ILogEntry Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            return _inner.Write(LogLevel.Debug, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3));
        }

        public ILogEntry Info<T>(T value)
        {
            return _inner.Write(LogLevel.Info, prefix + value);
        }

        public ILogEntry InfoException(string message, Exception exception)
        {
            return _inner.Write(LogLevel.Info, String.Format("{0}{1}: {2}", prefix, message, exception));
        }

        public ILogEntry Info(string message)
        {
            return _inner.Write(LogLevel.Info, prefix + message);
        }

        public ILogEntry Info(string message, params object[] args)
        {
            var result = invokeStringFormat(CultureInfo.InvariantCulture, message, args);
            return _inner.Write(LogLevel.Info, prefix + result);
        }

        public ILogEntry Info<TArgument>(string message, TArgument argument)
        {
            return _inner.Write(LogLevel.Info, prefix + String.Format(CultureInfo.InvariantCulture, message, argument));
        }

        public ILogEntry Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            return _inner.Write(LogLevel.Info, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2));
        }

        public ILogEntry Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            return _inner.Write(LogLevel.Info, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3));
        }

        public ILogEntry Warn<T>(T value)
        {
            return _inner.Write(LogLevel.Warn, prefix + value);
        }

        public ILogEntry WarnException(string message, Exception exception)
        {
            return _inner.Write(LogLevel.Warn, String.Format("{0}{1}: {2}", prefix, message, exception));
        }

        public ILogEntry Warn(string message)
        {
            return _inner.Write(LogLevel.Warn, prefix + message);
        }

        public ILogEntry Warn(string message, params object[] args)
        {
            var result = invokeStringFormat(CultureInfo.InvariantCulture, message, args);
            return _inner.Write(LogLevel.Warn, prefix + result);
        }

        public ILogEntry Warn<TArgument>(string message, TArgument argument)
        {
            return _inner.Write(LogLevel.Warn, prefix + String.Format(CultureInfo.InvariantCulture, message, argument));
        }

        public ILogEntry Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            return _inner.Write(LogLevel.Warn, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2));
        }

        public ILogEntry Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            return _inner.Write(LogLevel.Warn, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3));
        }

        public ILogEntry Error<T>(T value)
        {
            return _inner.Write(LogLevel.Error, prefix + value);
        }

        public ILogEntry ErrorException(string message, Exception exception)
        {
            return _inner.Write(LogLevel.Error, String.Format("{0}{1}: {2}", prefix, message, exception));
        }

        public ILogEntry Error(string message)
        {
            return _inner.Write(LogLevel.Error, prefix + message);
        }

        public ILogEntry Error(string message, params object[] args)
        {
            var result = invokeStringFormat(CultureInfo.InvariantCulture, message, args);
            return _inner.Write(LogLevel.Error, prefix + result);
        }

        public ILogEntry Error<TArgument>(string message, TArgument argument)
        {
            return _inner.Write(LogLevel.Error, prefix + String.Format(CultureInfo.InvariantCulture, message, argument));
        }

        public ILogEntry Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            return _inner.Write(LogLevel.Error, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2));
        }

        public ILogEntry Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            return _inner.Write(LogLevel.Error, prefix + String.Format(CultureInfo.InvariantCulture, message, argument1, argument2, argument3));
        }

        public ILogEntry EnterSpan(string message)
        {
            Info("Entered Span: " + message);
            return new LogEntry(new ActionDisposable(() => Info("Exited Span: " + message)));
        }

        public ILogEntry EnterSpan(string message, params object[] args)
        {
            var msg = invokeStringFormat(CultureInfo.InvariantCulture, message, args);
            return EnterSpan(msg);
        }

        public ILogEntry EnterSpan(LogLevel logLevel, string message)
        {
            _inner.Write(logLevel, ("Entered Span: " + message));
            return new LogEntry(new ActionDisposable(() => _inner.Write(logLevel, "Exited Span: " + message)));
        }

        public ILogEntry EnterSpan(LogLevel logLevel, string message, params object[] args)
        {
            var msg = invokeStringFormat(CultureInfo.InvariantCulture, message, args);
            return EnterSpan(logLevel, msg);
        }

        public LogLevel Level {
            get { return _inner.Level; }
            set { _inner.Level = value; }
        }

        string invokeStringFormat(IFormatProvider formatProvider, string message, object[] args)
        {
            var sfArgs = new object[3];
            sfArgs[0] = formatProvider;
            sfArgs[1] = message;
            sfArgs[2] = args;

            return (string) stringFormat.Invoke(null, sfArgs);
        }
    }
    #endregion
}

// vim: tw=120 ts=4 sw=4 et :
