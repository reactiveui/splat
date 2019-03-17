// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// A base class for testing full loggers that are available.
    /// </summary>
    /// <typeparam name="T">The type of logger.</typeparam>
    public abstract class FullLoggerTestBase<T>
        where T : IFullLogger, new()
    {
        /// <summary>
        /// Test to make sure the debug emits nothing when not enabled.
        /// </summary>
        [Fact]
        public void Debug_Disabled_Should_Not_Emit()
        {
            var (logger, target) = GetLogger(LogLevel.Fatal);
            bool invoked = false;

            logger.Warn<DummyObjectClass1>(
                () =>
                {
                    invoked = true;
                    return "This is a test.";
                });

            Assert.Equal("This is a test." + Environment.NewLine, target.Logs.Last().message);
            Assert.Equal(typeof(DummyObjectClass1), target.PassedTypes.FirstOrDefault());
            Assert.True(invoked);
        }

        /// <summary>
        /// Test to make sure the debug emits something when enabled.
        /// </summary>
        [Fact]
        public void Debug_Enabled_Should_Emit()
        {
            var (logger, target) = GetLogger(LogLevel.Debug);
            bool invoked = false;

            logger.Debug<DummyObjectClass1>(
                () =>
                {
                    invoked = true;
                    return "This is a test.";
                });

            Assert.Equal("This is a test." + Environment.NewLine, target.Logs.Last().message);
            Assert.Equal(typeof(DummyObjectClass1), target.PassedTypes.FirstOrDefault());
            Assert.True(invoked);
        }

        /// <summary>
        /// Test to make sure the Info emits nothing when not enabled.
        /// </summary>
        [Fact]
        public void Info_Disabled_Should_Not_Emit()
        {
            var (logger, target) = GetLogger(LogLevel.Fatal);
            bool invoked = false;

            logger.Info<DummyObjectClass1>(
                () =>
                {
                    invoked = true;
                    return "This is a test.";
                });

            Assert.Equal("This is a test." + Environment.NewLine, target.Logs.Last().message);
            Assert.Equal(typeof(DummyObjectClass1), target.PassedTypes.FirstOrDefault());
            Assert.True(invoked);
        }

        /// <summary>
        /// Test to make sure the Info emits something when enabled.
        /// </summary>
        [Fact]
        public void Info_Enabled_Should_Emit()
        {
            var (logger, target) = GetLogger(LogLevel.Debug);
            bool invoked = false;

            logger.Info<DummyObjectClass1>(
                () =>
                {
                    invoked = true;
                    return "This is a test.";
                });

            Assert.Equal("This is a test." + Environment.NewLine, target.Logs.Last().message);
            Assert.Equal(typeof(DummyObjectClass1), target.PassedTypes.FirstOrDefault());
            Assert.True(invoked);
        }

        /// <summary>
        /// Test to make sure the Warn emits nothing when not enabled.
        /// </summary>
        [Fact]
        public void Warn_Disabled_Should_Not_Emit()
        {
            var (logger, target) = GetLogger(LogLevel.Fatal);
            bool invoked = false;

            logger.Warn<DummyObjectClass1>(
                () =>
                {
                    invoked = true;
                    return "This is a test.";
                });

            Assert.Equal("This is a test." + Environment.NewLine, target.Logs.Last().message);
            Assert.Equal(typeof(DummyObjectClass1), target.PassedTypes.FirstOrDefault());
            Assert.True(invoked);
        }

        /// <summary>
        /// Test to make sure the Warn emits something when enabled.
        /// </summary>
        [Fact]
        public void Warn_Enabled_Should_Emit()
        {
            var (logger, target) = GetLogger(LogLevel.Debug);
            bool invoked = false;

            logger.Warn<DummyObjectClass1>(
                () =>
                {
                    invoked = true;
                    return "This is a test.";
                });

            Assert.Equal("This is a test." + Environment.NewLine, target.Logs.Last().message);
            Assert.Equal(typeof(DummyObjectClass1), target.PassedTypes.FirstOrDefault());
            Assert.True(invoked);
        }

        /// <summary>
        /// Test to make sure the Error emits nothing when not enabled.
        /// </summary>
        [Fact]
        public void Error_Disabled_Should_Not_Emit()
        {
            var (logger, target) = GetLogger(LogLevel.Fatal);
            bool invoked = false;

            logger.Error<DummyObjectClass1>(
                () =>
                {
                    invoked = true;
                    return "This is a test.";
                });

            Assert.Equal("This is a test." + Environment.NewLine, target.Logs.Last().message);
            Assert.Equal(typeof(DummyObjectClass1), target.PassedTypes.FirstOrDefault());
            Assert.True(invoked);
        }

        /// <summary>
        /// Test to make sure the Error emits something when enabled.
        /// </summary>
        [Fact]
        public void Error_Enabled_Should_Emit()
        {
            var (logger, target) = GetLogger(LogLevel.Debug);
            bool invoked = false;

            logger.Error<DummyObjectClass1>(
                () =>
                {
                    invoked = true;
                    return "This is a test.";
                });

            Assert.Equal("This is a test." + Environment.NewLine, target.Logs.Last().message);
            Assert.Equal(typeof(DummyObjectClass1), target.PassedTypes.FirstOrDefault());
            Assert.True(invoked);
        }

        /// <summary>
        /// Test to make sure the Fatal emits something when enabled.
        /// </summary>
        [Fact]
        public void Fatal_Enabled_Should_Emit()
        {
            var (logger, target) = GetLogger(LogLevel.Fatal);
            bool invoked = false;

            logger.Fatal<DummyObjectClass1>(
                () =>
                {
                    invoked = true;
                    return "This is a test.";
                });

            Assert.Equal("This is a test." + Environment.NewLine, target.Logs.Last().message);
            Assert.Equal(typeof(DummyObjectClass1), target.PassedTypes.FirstOrDefault());
            Assert.True(invoked);
        }

        /// <summary>
        /// Gets the logger to test.
        /// </summary>
        /// <param name="minimumLogLevel">The minimum log level.</param>
        /// <returns>The logger.</returns>
        protected abstract (IFullLogger, IMockLogTarget) GetLogger(LogLevel minimumLogLevel);
    }
}
