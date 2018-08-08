namespace Splat.NLog
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using Splat;

    using LogLevel = Splat.LogLevel;

    /// <summary>
    /// NLog Logger taken from ReactiveUI 5.
    /// </summary>
    [DebuggerDisplay("Name={_inner.Name} Level={Level}")]
    internal sealed class NLogSplatLogger : IFullLogger
    {
        private readonly global::NLog.Logger _inner;
        private MethodInfo _debugFp;
        private MethodInfo _fatalNoFp;
        private MethodInfo _infoNoFp;
        private MethodInfo _infoFp;
        private MethodInfo _debugNoFp;
        private MethodInfo _fatalFp;
        private MethodInfo _warnFp;
        private MethodInfo _warnNoFp;
        private MethodInfo _errorFp;
        private MethodInfo _errorNoFp;

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogLogger"/> class.
        /// </summary>
        /// <param name="inner">The actual nlog logger</param>
        /// <exception cref="ArgumentNullException">NLog logger not passed</exception>
        public NLogSplatLogger(global::NLog.Logger inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException(nameof(inner));
            }

            this._inner = inner;
        }

        /// <summary>
        /// Gets or sets the logging level.
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Writes a message at the specified log level
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="logLevel">The log level to write the message at.</param>
        public void Write(String message, LogLevel logLevel)
        {
            this._inner.Log(RxUitoNLogLevel(logLevel), message);
        }

        public bool IsDebugEnabled => this._inner.IsDebugEnabled;
        public bool IsInfoEnabled => this._inner.IsInfoEnabled;
        public bool IsWarnEnabled => this._inner.IsWarnEnabled;
        public bool IsErrorEnabled => this._inner.IsErrorEnabled;
        public bool IsFatalEnabled => this._inner.IsFatalEnabled;

        /// <summary>
        /// Writes a debug message using a generic argument
        /// </summary>
        /// <typeparam name="T">The type of the argument</typeparam>
        /// <param name="value">The argument to log</param>
        public void Debug<T>(T value)
        {
            this._inner.Debug(value);
        }

        /// <summary>
        /// Writes a debug message using a generic argument
        /// </summary>
        /// <typeparam name="T">The type of the argument</typeparam>
        /// <param name="formatProvider">The format provider</param>
        /// <param name="value">The argument to log</param>
        public void Debug<T>(IFormatProvider formatProvider, T value)
        {
            this._inner.Debug(formatProvider, value);
        }

        /// <summary>
        /// Logs a debug message and exception.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception that has occurred</param>
        public void DebugException(String message, Exception exception)
        {
            this._inner.Debug(exception, message);
        }

        /// <summary>
        /// Logs a debug message and array of arguments.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="message">The message to log</param>
        /// <param name="args">The argument array to log</param>
        public void Debug(IFormatProvider formatProvider, String message, params Object[] args)
        {
            if (this._debugFp == null)
            {
#pragma warning disable CC0021 // Use nameof
                this._debugFp = this._inner.GetType().GetMethod("Debug", new[] { typeof(IFormatProvider), typeof(String), typeof(Object[]) });
#pragma warning restore CC0021 // Use nameof
            }

            var innerArgs = new Object[args.Length + 2];
            innerArgs[0] = formatProvider;
            innerArgs[1] = message;
            Array.Copy(args, 0, innerArgs, 2, args.Length);
            this._debugFp.Invoke(this._inner, innerArgs);
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Debug(String message)
        {
            this._inner.Debug(message);
        }

        /// <summary>
        /// Logs a debug message and array of arguments.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="args">The argument array to log</param>
        public void Debug(String message, params Object[] args)
        {
            if (this._debugNoFp == null)
            {
#pragma warning disable CC0021 // Use nameof
                this._debugNoFp = this._inner.GetType().GetMethod("Debug", new[] { typeof(String), typeof(Object[]) });
#pragma warning restore CC0021 // Use nameof
            }

            var innerArgs = new Object[args.Length + 1];
            innerArgs[0] = message;
            Array.Copy(args, 0, innerArgs, 1, args.Length);
            this._debugNoFp.Invoke(this._inner, innerArgs);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument">The type of argument</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument">The argument to log</param>
        public void Debug<TArgument>(IFormatProvider formatProvider, String message, TArgument argument)
        {
            this._inner.Debug(formatProvider, message, argument);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument">The type of argument</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument">The argument to log</param>
        public void Debug<TArgument>(String message, TArgument argument)
        {
            this._inner.Debug(message, argument);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, String message, TArgument1 argument1, TArgument2 argument2)
        {
            this._inner.Debug(formatProvider, message, argument1, argument2);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        public void Debug<TArgument1, TArgument2>(String message, TArgument1 argument1, TArgument2 argument2)
        {
            this._inner.Debug(message, argument1, argument2);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        /// <param name="argument3">The third argument to log</param>
        public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, String message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            this._inner.Debug(formatProvider, message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Logs a debug message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        /// <param name="argument3">The third argument to log</param>
        public void Debug<TArgument1, TArgument2, TArgument3>(String message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            this._inner.Debug(message, argument1, argument2, argument3);
        }

        public void Info<T>(T value)
        {
            this._inner.Info(value);
        }

        public void Info<T>(IFormatProvider formatProvider, T value)
        {
            this._inner.Info(formatProvider, value);
        }

        /// <summary>
        /// Logs an information message and exception.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception that has occurred</param>
        public void InfoException(String message, Exception exception)
        {
            this._inner.Info(exception, message);
        }

        /// <summary>
        /// Logs an information message and array of arguments.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="message">The message to log</param>
        /// <param name="args">The argument array to log</param>
        public void Info(IFormatProvider formatProvider, String message, params Object[] args)
        {
            if (this._infoFp == null)
            {
#pragma warning disable CC0021 // Use nameof
                this._infoFp = this._inner.GetType().GetMethod("Info", new[] { typeof(IFormatProvider), typeof(String), typeof(Object[]) });
#pragma warning restore CC0021 // Use nameof
            }

            var innerArgs = new Object[args.Length + 2];
            innerArgs[0] = formatProvider;
            innerArgs[1] = message;
            Array.Copy(args, 0, innerArgs, 2, args.Length);
            this._infoFp.Invoke(this._inner, innerArgs);
        }

        /// <summary>
        /// Logs an information message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Info(String message)
        {
            this._inner.Info(message);
        }

        /// <summary>
        /// Logs an information message and array of arguments.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="args">The argument array to log</param>
        public void Info(String message, params Object[] args)
        {
            if (this._infoNoFp == null)
            {
#pragma warning disable CC0021 // Use nameof
                this._infoNoFp = this._inner.GetType().GetMethod("Info", new[] { typeof(String), typeof(Object[]) });
#pragma warning restore CC0021 // Use nameof
            }

            var innerArgs = new Object[args.Length + 1];
            innerArgs[0] = message;
            Array.Copy(args, 0, innerArgs, 1, args.Length);
            this._infoNoFp.Invoke(this._inner, innerArgs);
        }

        public void Info<TArgument>(IFormatProvider formatProvider, String message, TArgument argument)
        {
            this._inner.Info(formatProvider, message, argument);
        }

        /// <summary>
        /// Logs an information message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument">The type of argument</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument">The argument to log</param>
        public void Info<TArgument>(String message, TArgument argument)
        {
            this._inner.Info(message, argument);
        }

        /// <summary>
        /// Logs an information message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, String message, TArgument1 argument1, TArgument2 argument2)
        {
            this._inner.Info(formatProvider, message, argument1, argument2);
        }

        /// <summary>
        /// Logs an information message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        public void Info<TArgument1, TArgument2>(String message, TArgument1 argument1, TArgument2 argument2)
        {
            this._inner.Info(message, argument1, argument2);
        }

        /// <summary>
        /// Logs an information message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        /// <param name="argument3">The third argument to log</param>
        public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, String message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            this._inner.Info(formatProvider, message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Logs an information message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        /// <param name="argument3">The third argument to log</param>
        public void Info<TArgument1, TArgument2, TArgument3>(String message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            this._inner.Info(message, argument1, argument2, argument3);
        }

        public void Warn<T>(T value)
        {
            this._inner.Warn(value);
        }

        public void Warn<T>(IFormatProvider formatProvider, T value)
        {
            this._inner.Warn(formatProvider, value);
        }

        /// <summary>
        /// Logs a warning message and exception.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception that has occurred</param>
        public void WarnException(String message, Exception exception)
        {
            this._inner.Warn(exception, message);
        }

        /// <summary>
        /// Logs a warning message and array of arguments.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="message">The message to log</param>
        /// <param name="args">The argument array to log</param>
        public void Warn(IFormatProvider formatProvider, String message, params Object[] args)
        {
            if (this._warnFp == null)
            {
#pragma warning disable CC0021 // Use nameof
                this._warnFp = this._inner.GetType().GetMethod("Warn", new[] { typeof(IFormatProvider), typeof(String), typeof(Object[]) });
#pragma warning restore CC0021 // Use nameof
            }

            var innerArgs = new Object[args.Length + 2];
            innerArgs[0] = formatProvider;
            innerArgs[1] = message;
            Array.Copy(args, 0, innerArgs, 2, args.Length);
            this._warnFp.Invoke(this._inner, innerArgs);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Warn(String message)
        {
            this._inner.Warn(message);
        }

        /// <summary>
        /// Logs a debug message and array of arguments.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="args">The argument array to log</param>
        public void Warn(String message, params Object[] args)
        {
            if (this._warnNoFp == null)
            {
#pragma warning disable CC0021 // Use nameof
                this._warnNoFp = this._inner.GetType().GetMethod("Warn", new[] { typeof(String), typeof(Object[]) });
#pragma warning restore CC0021 // Use nameof
            }

            var innerArgs = new Object[args.Length + 1];
            innerArgs[0] = message;
            Array.Copy(args, 0, innerArgs, 1, args.Length);
            this._warnNoFp.Invoke(this._inner, innerArgs);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument">The type of argument</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument">The argument to log</param>
        public void Warn<TArgument>(IFormatProvider formatProvider, String message, TArgument argument)
        {
            this._inner.Warn(formatProvider, message, argument);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument">The type of argument</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument">The argument to log</param>
        public void Warn<TArgument>(String message, TArgument argument)
        {
            this._inner.Warn(message, argument);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, String message, TArgument1 argument1, TArgument2 argument2)
        {
            this._inner.Warn(formatProvider, message, argument1, argument2);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        public void Warn<TArgument1, TArgument2>(String message, TArgument1 argument1, TArgument2 argument2)
        {
            this._inner.Warn(message, argument1, argument2);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        /// <param name="argument3">The third argument to log</param>
        public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, String message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            this._inner.Warn(formatProvider, message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Logs a warning message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        /// <param name="argument3">The third argument to log</param>
        public void Warn<TArgument1, TArgument2, TArgument3>(String message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            this._inner.Warn(message, argument1, argument2, argument3);
        }

        public void Error<T>(T value)
        {
            this._inner.Error(value);
        }

        public void Error<T>(IFormatProvider formatProvider, T value)
        {
            this._inner.Error(formatProvider, value);
        }

        /// <summary>
        /// Logs an error message and exception.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception that has occurred</param>
        public void ErrorException(String message, Exception exception)
        {
            this._inner.Error(exception, message);
        }

        /// <summary>
        /// Logs an error message and array of arguments.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="message">The message to log</param>
        /// <param name="args">The argument array to log</param>
        public void Error(IFormatProvider formatProvider, String message, params Object[] args)
        {
            if (this._errorFp == null)
            {
#pragma warning disable CC0021 // Use nameof
                this._errorFp = this._inner.GetType().GetMethod("Error", new[] { typeof(IFormatProvider), typeof(String), typeof(Object[]) });
#pragma warning restore CC0021 // Use nameof
            }

            var innerArgs = new Object[args.Length + 2];
            innerArgs[0] = formatProvider;
            innerArgs[1] = message;
            Array.Copy(args, 0, innerArgs, 2, args.Length);
            this._errorFp.Invoke(this._inner, innerArgs);
        }

        /// <summary>
        /// Logs a error message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Error(String message)
        {
            this._inner.Error(message);
        }

        /// <summary>
        /// Logs a debug message and array of arguments.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="args">The argument array to log</param>
        public void Error(String message, params Object[] args)
        {
            if (this._errorNoFp == null)
            {
#pragma warning disable CC0021 // Use nameof
                this._errorNoFp = this._inner.GetType().GetMethod("Error", new[] { typeof(String), typeof(Object[]) });
#pragma warning restore CC0021 // Use nameof
            }

            var innerArgs = new Object[args.Length + 1];
            innerArgs[0] = message;
            Array.Copy(args, 0, innerArgs, 1, args.Length);
            this._errorNoFp.Invoke(this._inner, innerArgs);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument">The type of argument</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument">The argument to log</param>
        public void Error<TArgument>(IFormatProvider formatProvider, String message, TArgument argument)
        {
            this._inner.Error(formatProvider, message, argument);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument">The type of argument</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument">The argument to log</param>
        public void Error<TArgument>(String message, TArgument argument)
        {
            this._inner.Error(message, argument);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, String message, TArgument1 argument1, TArgument2 argument2)
        {
            this._inner.Error(formatProvider, message, argument1, argument2);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        public void Error<TArgument1, TArgument2>(String message, TArgument1 argument1, TArgument2 argument2)
        {
            this._inner.Error(message, argument1, argument2);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        /// <param name="argument3">The third argument to log</param>
        public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, String message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            this._inner.Error(formatProvider, message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Logs an error message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        /// <param name="argument3">The third argument to log</param>
        public void Error<TArgument1, TArgument2, TArgument3>(String message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            this._inner.Error(message, argument1, argument2, argument3);
        }

        public void Fatal<T>(T value)
        {
            this._inner.Fatal(value);
        }

        public void Fatal<T>(IFormatProvider formatProvider, T value)
        {
            this._inner.Fatal(formatProvider, value);
        }

        /// <summary>
        /// Logs a fatal message and exception.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception that has occurred</param>
        public void FatalException(String message, Exception exception)
        {
            this._inner.Fatal(exception, message);
        }

        /// <summary>
        /// Logs a fatal message and array of arguments.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <param name="message">The message to log</param>
        /// <param name="args">The argument array to log</param>
        public void Fatal(IFormatProvider formatProvider, String message, params Object[] args)
        {
            if (this._fatalFp == null)
            {
#pragma warning disable CC0021 // Use nameof
                this._fatalFp = this._inner.GetType().GetMethod("Fatal", new[] { typeof(IFormatProvider), typeof(String), typeof(Object[]) });
#pragma warning restore CC0021 // Use nameof
            }

            var innerArgs = new Object[args.Length + 2];
            innerArgs[0] = formatProvider;
            innerArgs[1] = message;
            Array.Copy(args, 0, innerArgs, 2, args.Length);
            this._fatalFp.Invoke(this._inner, innerArgs);
        }

        /// <summary>
        /// Logs a fatal message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void Fatal(String message)
        {
            this._inner.Fatal(message);
        }

        /// <summary>
        /// Logs a fatal message and array of arguments.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="args">The argument array to log</param>
        public void Fatal(String message, params Object[] args)
        {
            if (this._fatalNoFp == null)
            {
#pragma warning disable CC0021 // Use nameof
                this._fatalNoFp = this._inner.GetType().GetMethod("Fatal", new[] { typeof(String), typeof(Object[]) });
#pragma warning restore CC0021 // Use nameof
            }

            var innerArgs = new Object[args.Length + 1];
            innerArgs[0] = message;
            Array.Copy(args, 0, innerArgs, 1, args.Length);
            this._fatalNoFp.Invoke(this._inner, innerArgs);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument">The type of argument</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument">The argument to log</param>
        public void Fatal<TArgument>(IFormatProvider formatProvider, String message, TArgument argument)
        {
            this._inner.Fatal(formatProvider, message, argument);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument">The type of argument</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument">The argument to log</param>
        public void Fatal<TArgument>(String message, TArgument argument)
        {
            this._inner.Fatal(message, argument);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, String message, TArgument1 argument1, TArgument2 argument2)
        {
            this._inner.Fatal(formatProvider, message, argument1, argument2);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        public void Fatal<TArgument1, TArgument2>(String message, TArgument1 argument1, TArgument2 argument2)
        {
            this._inner.Fatal(message, argument1, argument2);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3</typeparam>
        /// <param name="formatProvider">The format provider to use for the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        /// <param name="argument3">The third argument to log</param>
        public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, String message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            this._inner.Fatal(formatProvider, message, argument1, argument2, argument3);
        }

        /// <summary>
        /// Logs a fatal message while passing generic arguments
        /// </summary>
        /// <typeparam name="TArgument1">The type of argument 1</typeparam>
        /// <typeparam name="TArgument2">The type of argument 2</typeparam>
        /// <typeparam name="TArgument3">The type of argument 3</typeparam>
        /// <param name="message">The message to log</param>
        /// <param name="argument1">The first argument to log</param>
        /// <param name="argument2">The second argument to log</param>
        /// <param name="argument3">The third argument to log</param>
        public void Fatal<TArgument1, TArgument2, TArgument3>(String message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            this._inner.Fatal(message, argument1, argument2, argument3);
        }

        private static global::NLog.LogLevel RxUitoNLogLevel(LogLevel logLevel)
        {
            var mappings = new[]
                               {
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Debug, global::NLog.LogLevel.Debug),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Info, global::NLog.LogLevel.Info),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Warn, global::NLog.LogLevel.Warn),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Error, global::NLog.LogLevel.Error),
                                   new Tuple<LogLevel, global::NLog.LogLevel>(LogLevel.Fatal, global::NLog.LogLevel.Fatal)
                               };

            return mappings.First(x => x.Item1 == logLevel).Item2;
        }
    }
}
