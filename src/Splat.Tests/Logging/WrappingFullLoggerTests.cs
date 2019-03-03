// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Tests that verify the <see cref="WrappingFullLogger"/> class.
    /// </summary>
    public class WrappingFullLoggerTests
    {
        /// <summary>
        /// Test to make sure the message writes.
        /// </summary>
        [Fact]
        public void Write_Should_Write_Message()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Write("This is a test.", LogLevel.Debug);

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
        }

        /// <summary>
        /// Test to make sure the type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Write_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Write("This is a test.", LogLevel.Debug);

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Debug<DummyObjectClass1>("This is a test.");

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
            Assert.Equal(typeof(DummyObjectClass1), textLogger.PassedTypes.FirstOrDefault());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Debug_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Debug<DummyObjectClass2>("This is a test.");

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
            Assert.Equal(typeof(DummyObjectClass2), textLogger.PassedTypes.FirstOrDefault());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Info<DummyObjectClass1>("This is a test.");

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
            Assert.Equal(typeof(DummyObjectClass1), textLogger.PassedTypes.FirstOrDefault());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Info_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Info<DummyObjectClass2>("This is a test.");

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
            Assert.Equal(typeof(DummyObjectClass2), textLogger.PassedTypes.FirstOrDefault());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Warn<DummyObjectClass1>("This is a test.");

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
            Assert.Equal(typeof(DummyObjectClass1), textLogger.PassedTypes.FirstOrDefault());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Warn_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Warn<DummyObjectClass2>("This is a test.");

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
            Assert.Equal(typeof(DummyObjectClass2), textLogger.PassedTypes.FirstOrDefault());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Error<DummyObjectClass1>("This is a test.");

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
            Assert.Equal(typeof(DummyObjectClass1), textLogger.PassedTypes.FirstOrDefault());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Error_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Error<DummyObjectClass2>("This is a test.");

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
            Assert.Equal(typeof(DummyObjectClass2), textLogger.PassedTypes.FirstOrDefault());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Fatal<DummyObjectClass1>("This is a test.");

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
            Assert.Equal(typeof(DummyObjectClass1), textLogger.PassedTypes.FirstOrDefault());
        }

        /// <summary>
        /// Test to make sure the generic type parameter is passed to the logger.
        /// </summary>
        [Fact]
        public void Fatal_With_Generic_Type_Should_Write_Message_And_Type_Provided()
        {
            var textLogger = new TextLogger();
            var logger = new WrappingFullLogger(textLogger);

            logger.Fatal<DummyObjectClass2>("This is a test.");

            Assert.Equal("This is a test." + Environment.NewLine, textLogger.Value);
            Assert.Equal(typeof(DummyObjectClass2), textLogger.PassedTypes.FirstOrDefault());
        }
    }
}
