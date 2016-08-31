using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Splat;
using Xunit;

namespace UnitTests
{
    public partial class LoggingTests
    {
        [Conditional("BAIT")]
        [Conditional("SWITCH")]
        [Theory]
        [InlineData("This is an informational message.", "LogHost: This is an informational message.")]
        public void InfoShouldBeExpected(string input, string expected)
        {
            var onWrite = new Action<string, LogLevel>((message, loglevel) =>
            {
                Assert.Equal(expected, message);
                Assert.Equal(loglevel, LogLevel.Info);
            });

            Locator.CurrentMutable.Register(() => new LoggerFixture(onWrite), typeof(ILogger));

            LogHost.Default.Info(input);
        }

        [Conditional("BAIT")]
        [Conditional("SWITCH")]
        [Theory]
        [InlineData("This is an informational message.", "LogHost: This is an informational message.: System.Exception: This is an informational message.")]
        public void InfoExceptionShouldBeExpected(string input, string expected)
        {
            var onWrite = new Action<string, LogLevel>((message, loglevel) =>
            {
                Assert.Equal(expected, message);
                Assert.Equal(loglevel, LogLevel.Info);
            });

            Locator.CurrentMutable.Register(() => new LoggerFixture(onWrite), typeof(ILogger));

            LogHost.Default.InfoException(input, new Exception(input));
        }


        [Conditional("BAIT")]
        [Conditional("SWITCH")]
        [Theory]
        [InlineData("This is a debug message.", "LogHost: This is a debug message.")]
        public void DebugShouldBeExpected(string input, string expected)
        {
            var onWrite = new Action<string, LogLevel>((message, loglevel) =>
            {
                Assert.Equal(expected, message);
                Assert.Equal(loglevel, LogLevel.Debug);
            });

            Locator.CurrentMutable.Register(() => new LoggerFixture(onWrite), typeof(ILogger));

            LogHost.Default.Debug(input);
        }

        [Conditional("BAIT")]
        [Conditional("SWITCH")]
        [Theory]
        [InlineData("This is a debug message.", "LogHost: This is a debug message.: System.Exception: This is a debug message.")]
        public void DebugExceptionShouldBeExpected(string input, string expected)
        {
            var onWrite = new Action<string, LogLevel>((message, loglevel) =>
            {
                Assert.Equal(expected, message);
                Assert.Equal(loglevel, LogLevel.Debug);
            });

            Locator.CurrentMutable.Register(() => new LoggerFixture(onWrite), typeof(ILogger));

            LogHost.Default.DebugException(input, new Exception(input));
        }


        [Conditional("BAIT")]
        [Conditional("SWITCH")]
        [Theory]
        [InlineData("This is a warning message.", "LogHost: This is a warning message.")]
        public void WarnShouldBeExpected(string input, string expected)
        {
            var onWrite = new Action<string, LogLevel>((message, loglevel) =>
            {
                Assert.Equal(expected, message);
                Assert.Equal(loglevel, LogLevel.Warn);
            });

            Locator.CurrentMutable.Register(() => new LoggerFixture(onWrite), typeof(ILogger));

            LogHost.Default.Warn(input);
        }

        [Conditional("BAIT")]
        [Conditional("SWITCH")]
        [Theory]
        [InlineData("This is a warning message.", "LogHost: This is a warning message.: System.Exception: This is a warning message.")]
        public void WarnExceptionShouldBeExpected(string input, string expected)
        {
            var onWrite = new Action<string, LogLevel>((message, loglevel) =>
            {
                Assert.Equal(expected, message);
                Assert.Equal(loglevel, LogLevel.Warn);
            });

            Locator.CurrentMutable.Register(() => new LoggerFixture(onWrite), typeof(ILogger));

            LogHost.Default.WarnException(input, new Exception(input));
        }

        [Conditional("BAIT")]
        [Conditional("SWITCH")]
        [Theory]
        [InlineData("This is an error message.", "LogHost: This is an error message.")]
        public void ErrorShouldBeExpected(string input, string expected)
        {
            var onWrite = new Action<string, LogLevel>((message, loglevel) =>
            {
                Assert.Equal(expected, message);
                Assert.Equal(loglevel, LogLevel.Error);
            });

            Locator.CurrentMutable.Register(() => new LoggerFixture(onWrite), typeof(ILogger));

            LogHost.Default.Error(input);
        }

        [Conditional("BAIT")]
        [Conditional("SWITCH")]
        [Theory]
        [InlineData("This is an error message.", "LogHost: This is an error message.: System.Exception: This is an error message.")]
        public void ErrorExceptionShouldBeExpected(string input, string expected)
        {
            var onWrite = new Action<string, LogLevel>((message, loglevel) =>
            {
                Assert.Equal(expected, message);
                Assert.Equal(loglevel, LogLevel.Error);
            });

            Locator.CurrentMutable.Register(() => new LoggerFixture(onWrite), typeof(ILogger));

            LogHost.Default.ErrorException(input, new Exception(input));
        }

        [Conditional("BAIT")]
        [Conditional("SWITCH")]
        [Theory]
        [InlineData("This is a fatal message.", "LogHost: This is a fatal message.")]
        public void FatalShouldBeExpected(string input, string expected)
        {
            var onWrite = new Action<string, LogLevel>((message, loglevel) =>
            {
                Assert.Equal(expected, message);
                Assert.Equal(loglevel, LogLevel.Fatal);
            });

            Locator.CurrentMutable.Register(() => new LoggerFixture(onWrite), typeof(ILogger));

            LogHost.Default.Fatal(input);
        }

        [Conditional("BAIT")]
        [Conditional("SWITCH")]
        [Theory]
        [InlineData("This is a fatal message.", "LogHost: This is a fatal message.: System.Exception: This is a fatal message.")]
        public void FatalExceptionShouldBeExpected(string input, string expected)
        {
            var onWrite = new Action<string, LogLevel>((message, loglevel) =>
            {
                Assert.Equal(expected, message);
                Assert.Equal(loglevel, LogLevel.Fatal);
            });

            Locator.CurrentMutable.Register(() => new LoggerFixture(onWrite), typeof(ILogger));

            LogHost.Default.FatalException(input, new Exception(input));
        }
    }
}
