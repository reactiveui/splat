// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using Splat.Microsoft.Extensions.Logging;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Tests that verify the <see cref="MicrosoftExtensionsLoggingLogger"/> class.
    /// </summary>
    public class MicrosoftExtensionsLoggingLoggerTests
    {
        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        [Fact]
        public void Write_Should_Write_Message()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Write("This is a test.", LogLevel.Debug);

            Assert.Equal(1, memoryTarget.Count);

            Assert.Equal("This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Write_Should_Write_Message_And_Type()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Write("This is a test.", LogLevel.Debug);

            Assert.Equal(1, memoryTarget.Count);

            Assert.Equal("This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Debug<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, memoryTarget.Count);

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Debug<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, memoryTarget.Count);

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Info<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, memoryTarget.Count);

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Info<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, memoryTarget.Count);

            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Warn<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, memoryTarget.Count);

            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Warn<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, memoryTarget.Count);
            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Error<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, memoryTarget.Count);
            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Error<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, memoryTarget.Count);
            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Fatal<DummyObjectClass1>("This is a test.");

            Assert.Equal(1, memoryTarget.Count);
            Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", memoryTarget.First());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var loggerAndMemoryTarget = GetSplatLoggerAndMemoryTarget();
            var logger = new WrappingFullLogger(loggerAndMemoryTarget.Logger);
            var memoryTarget = loggerAndMemoryTarget.MemoryTarget;

            Assert.Equal(0, memoryTarget.Count);

            logger.Fatal<DummyObjectClass2>("This is a test.");

            Assert.Equal(1, memoryTarget.Count);
            Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", memoryTarget.First());
        }

        private static (global::Microsoft.Extensions.Logging.ILogger Logger, IList<string> MemoryTarget) GetActualloggerAndMemoryTarget()
        {
            var memoryTarget = new List<string>();
            var mockLogger = new MockActualMicrosoftExtensionsLoggingLogger(memoryTarget);

            return (mockLogger, memoryTarget);
        }

        private static (ILogger Logger, IList<string> MemoryTarget) GetSplatLoggerAndMemoryTarget()
        {
            var actualLogger = GetActualloggerAndMemoryTarget();
            return (new MicrosoftExtensionsLoggingLogger(actualLogger.Logger), actualLogger.MemoryTarget);
        }
    }
}
