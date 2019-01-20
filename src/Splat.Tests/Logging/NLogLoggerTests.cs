using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;
using Splat.NLog;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Tests that verify the <see cref="NLogLogger"/> class.
    /// </summary>
    public class NLogLoggerTests
    {
        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        [Fact]
        public void Write_Should_Write_Message()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Write("This is a test.", LogLevel.Debug);

            Assert.Equal(1, memoryTarget.Logs.Count);

            Assert.Equal("This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Write_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Write("This is a test.", LogLevel.Debug);

            Assert.Equal(1, memoryTarget.Logs.Count);

            Assert.Equal("This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Debug<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, memoryTarget.Logs.Count);

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Debug<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, memoryTarget.Logs.Count);

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Info<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, memoryTarget.Logs.Count);

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Info<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, memoryTarget.Logs.Count);

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Warn<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, memoryTarget.Logs.Count);

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Warn<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, memoryTarget.Logs.Count);
            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Error<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, memoryTarget.Logs.Count);
            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Error<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, memoryTarget.Logs.Count);
            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Fatal<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, memoryTarget.Logs.Count);
            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", memoryTarget.Logs.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var nlogLoggerAndMemoryTarget = GetSplatNLogLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(nlogLoggerAndMemoryTarget.Logger);
            var memoryTarget = nlogLoggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Logs.Count);

            logger.Fatal<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, memoryTarget.Logs.Count);
            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", memoryTarget.Logs.First());
        }

        private static (global::NLog.ILogger Logger, MemoryTarget MemoryTarget) GetActualNLogLoggerAndMemoryTarget()
        {
            var configuration = new LoggingConfiguration();

            var memoryTarget = new MemoryTarget("UnitTestMemoryTarget")
            {
                Layout = "${message}"
            };
            configuration.AddTarget(memoryTarget);
            var loggingRule = new LoggingRule("*", global::NLog.LogLevel.Trace, memoryTarget);
            configuration.LoggingRules.Add(loggingRule);

            LogManager.Configuration = configuration;

            return (LogManager.GetCurrentClassLogger(), memoryTarget);
        }

        private static (ILogger Logger, MemoryTarget MemoryTarget) GetSplatNLogLoggerAndMemoryTarget()
        {
            var actualNLogLogger = GetActualNLogLoggerAndMemoryTarget();
            return (new NLogLogger(actualNLogLogger.Logger), actualNLogLogger.MemoryTarget);
        }
    }
}
