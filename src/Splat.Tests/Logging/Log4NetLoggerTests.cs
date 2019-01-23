using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Splat.Log4Net;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Tests that verify the <see cref="Log4NetLogger"/> class.
    /// </summary>
    public class Log4NetLoggerTests
    {
        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        [Fact]
        public void Write_Should_Write_Message()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Write("This is a test.", LogLevel.Debug);

            var logEvent = Assert.Single(memoryTarget.GetEvents());

            Assert.Equal("This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Write_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Write("This is a test.", LogLevel.Debug);

            var logEvent = Assert.Single(memoryTarget.GetEvents());

            Assert.Equal("This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Debug<DummyObjectClass1>("This is a test.");

            var logEvent = Assert.Single(memoryTarget.GetEvents());

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Debug<DummyObjectClass2>("This is a test.");

            var logEvent = Assert.Single(memoryTarget.GetEvents());

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Info<DummyObjectClass1>("This is a test.");

            var logEvent = Assert.Single(memoryTarget.GetEvents());

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Info<DummyObjectClass2>("This is a test.");

            var logEvent = Assert.Single(memoryTarget.GetEvents());

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Warn<DummyObjectClass1>("This is a test.");

            var logEvent = Assert.Single(memoryTarget.GetEvents());

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Warn<DummyObjectClass2>("This is a test.");

            var logEvent = Assert.Single(memoryTarget.GetEvents());
            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Error<DummyObjectClass1>("This is a test.");

            var logEvent = Assert.Single(memoryTarget.GetEvents());
            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Error<DummyObjectClass2>("This is a test.");

            var logEvent = Assert.Single(memoryTarget.GetEvents());
            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Fatal<DummyObjectClass1>("This is a test.");

            var logEvent = Assert.Single(memoryTarget.GetEvents());
            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", logEvent.MessageObject);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var nlogLoggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Empty(memoryTarget.GetEvents());

            logger.Fatal<DummyObjectClass2>("This is a test.");

            var logEvent = Assert.Single(memoryTarget.GetEvents());
            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", logEvent.MessageObject);
        }

        private static (global::log4net.ILog Logger, global::log4net.Appender.MemoryAppender MemoryTarget) GetActualLog4NetLoggerAndMemoryTarget()
        {
            var memory = new global::log4net.Appender.MemoryAppender();
            memory.ActivateOptions();

            var hierarchy = (global::log4net.Repository.Hierarchy.Hierarchy)global::log4net.LogManager.GetRepository("TestRepository");
            hierarchy.Root.AddAppender(memory);
            hierarchy.Root.Level = global::log4net.Core.Level.Trace;
            hierarchy.Configured = true;

            var logger = global::log4net.LogManager.GetLogger(typeof(Log4NetLoggerTests));

            return (logger, memory);
        }

        private static (ILogger Logger, global::log4net.Appender.MemoryAppender MemoryTarget) GetSplatLoggerAndMemoryTarget()
        {
            var actualLoggerAndMemoryTarget = GetActualLog4NetLoggerAndMemoryTarget();
            return (new Log4NetLogger(actualLoggerAndMemoryTarget.Logger), actualLoggerAndMemoryTarget.MemoryTarget);
        }
    }
}
