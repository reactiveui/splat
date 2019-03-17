// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting;
using Splat.Serilog;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Tests that verify the <see cref="SerilogLogger"/> class.
    /// </summary>
    public class SerilogLoggerTests
    {
        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        [Fact]
        public void Write_Should_Write_Message()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Write("This is a test.", LogLevel.Debug);

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal("This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Write_Should_Write_Message_And_Type()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Write("This is a test.", LogLevel.Debug);

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal("This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Debug<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal($"{typeof(DummyObjectClass1).FullName}: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Debug<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal($"{typeof(DummyObjectClass2).FullName}: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the error is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Exception_Should_Write_Exception_Message_And_Provided()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Debug(new Exception(), "This is a test.");

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal("[(\"Type\": \"System.Exception\"), (\"HResult\": -2146233088), (\"Message\": \"Exception of type 'System.Exception' was thrown.\"), (\"Source\": null)]: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Info<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal($"{typeof(DummyObjectClass1).FullName}: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Info<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal($"{typeof(DummyObjectClass2).FullName}: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the error is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Exception_Should_Write_Exception_Message_And_Provided()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Info(new Exception(), "This is a test.");

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal("[(\"Type\": \"System.Exception\"), (\"HResult\": -2146233088), (\"Message\": \"Exception of type 'System.Exception' was thrown.\"), (\"Source\": null)]: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Warn<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal($"{typeof(DummyObjectClass1).FullName}: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Warn<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, target.Logs.Count);
            Assert.Equal($"{typeof(DummyObjectClass2).FullName}: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the error is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Exception_Should_Write_Exception_Message_And_Provided()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Warn(new Exception(), "This is a test.");

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal("[(\"Type\": \"System.Exception\"), (\"HResult\": -2146233088), (\"Message\": \"Exception of type 'System.Exception' was thrown.\"), (\"Source\": null)]: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Error<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, target.Logs.Count);
            Assert.Equal($"{typeof(DummyObjectClass1).FullName}: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Error<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, target.Logs.Count);
            Assert.Equal($"{typeof(DummyObjectClass2).FullName}: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the error is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Exception_Should_Write_Exception_Message_And_Provided()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Error(new Exception(), "This is a test.");

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal("[(\"Type\": \"System.Exception\"), (\"HResult\": -2146233088), (\"Message\": \"Exception of type 'System.Exception' was thrown.\"), (\"Source\": null)]: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Fatal<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, target.Logs.Count);
            Assert.Equal($"{typeof(DummyObjectClass1).FullName}: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Fatal<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, target.Logs.Count);
            Assert.Equal($"{typeof(DummyObjectClass2).FullName}: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the error is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Exception_Should_Write_Exception_Message_And_Provided()
        {
            var serilogLoggerAndTarget = GetSplatSerilogLoggerAndTarget();
            var logger = new WrappingFullLogger(serilogLoggerAndTarget.Logger);
            var target = serilogLoggerAndTarget.Target;

            Assert.Equal(0, target.Logs.Count);

            logger.Fatal(new Exception(), "This is a test.");

            Assert.Equal(1, target.Logs.Count);

            Assert.Equal("[(\"Type\": \"System.Exception\"), (\"HResult\": -2146233088), (\"Message\": \"Exception of type 'System.Exception' was thrown.\"), (\"Source\": null)]: This is a test.", target.Logs.First());
        }

        /// <summary>
        /// Test to make sure the calling `UseSerilogWithWrappingFullLogger` logs.
        /// </summary>
        [Fact]
        public void Configuring_With_Static_Log_Should_Write_Message()
        {
            var originalLocator = Locator.InternalLocator;
            try
            {
                Locator.InternalLocator = new InternalLocator();
                var serilogLoggerAndTarget = GetActualSerilogLoggerAndTarget();
                Log.Logger = serilogLoggerAndTarget.Logger;
                var target = serilogLoggerAndTarget.Target;

                Locator.CurrentMutable.UseSerilogFullLogger();

                Assert.Equal(0, target.Logs.Count);

                IEnableLogger logger = null;
                logger.Log().Debug<DummyObjectClass2>("This is a test.");

                Assert.Equal(1, target.Logs.Count);
                Assert.Equal($"{typeof(DummyObjectClass2).FullName}: This is a test.", target.Logs.First());
            }
            finally
            {
                Locator.InternalLocator = originalLocator;
            }
        }

        /// <summary>
        /// Test to make calling `UseSerilogWithWrappingFullLogger(Serilog.ILogger)` logs.
        /// </summary>
        [Fact]
        public void Configuring_With_PreConfigured_Log_Should_Write_Message()
        {
            var originalLocator = Locator.InternalLocator;
            try
            {
                Locator.InternalLocator = new InternalLocator();
                var serilogLoggerAndTarget = GetActualSerilogLoggerAndTarget();
                var target = serilogLoggerAndTarget.Target;
                Locator.CurrentMutable.UseSerilogFullLogger(serilogLoggerAndTarget.Logger);

                Assert.Equal(0, target.Logs.Count);

                IEnableLogger logger = null;

                logger.Log().Debug<DummyObjectClass2>("This is a test.");

                Assert.Equal(1, target.Logs.Count);
                Assert.Equal($"{typeof(DummyObjectClass2).FullName}: This is a test.", target.Logs.First());
            }
            finally
            {
                Locator.InternalLocator = originalLocator;
            }
        }

        private static (global::Serilog.ILogger Logger, LogTarget Target) GetActualSerilogLoggerAndTarget()
        {
            var messages = new LogTarget();

            var log = new LoggerConfiguration()
                .Enrich
                .WithExceptionDetails()
                .MinimumLevel
                .Debug()
                .WriteTo
                .Sink(messages)
                .CreateLogger();

            return (log, messages);
        }

        private static (ILogger Logger, LogTarget Target) GetSplatSerilogLoggerAndTarget()
        {
            var actualSerilogLogger = GetActualSerilogLoggerAndTarget();
            return (new SerilogFullLogger(actualSerilogLogger.Logger), actualSerilogLogger.Target);
        }

        private class LogTarget : ILogEventSink
        {
            public IList<string> Logs { get; } = new List<string>();

            public void Emit(LogEvent logEvent)
            {
                if (logEvent.Properties.TryGetValue("SourceContext", out var context))
                {
                    Logs.Add(context.ToString().Trim('"').Trim() + ": " + logEvent.RenderMessage());
                }
                else if (logEvent.Properties.TryGetValue("ExceptionDetail", out var error))
                {
                    Logs.Add(error.ToString().Trim('"').Trim() + ": " + logEvent.RenderMessage());
                }
                else
                {
                    Logs.Add(logEvent.RenderMessage());
                }
            }
        }
    }
}
