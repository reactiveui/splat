using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Unit Tests for the Static Logging wrapper implementation.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class StaticLoggerTests
    {
        private const string Message = "Message";

        /// <summary>
        /// Test to ensure debug writes.
        /// </summary>
        [Fact]
        public void Debug_Writes_Message()
        {
            Test_Write_Message(logger => logger.Debug(Message));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Debug_Writes_Message_InvariantCulture()
        {
            Test_Write_Message(logger => logger.Debug(CultureInfo.InvariantCulture, "{0}", "Message"));
        }

        /// <summary>
        /// Test to ensure info writes.
        /// </summary>
        [Fact]
        public void Info_Writes_Message()
        {
            Test_Write_Message(logger => logger.Info(Message));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Info_Writes_Message_InvariantCulture()
        {
            Test_Write_Message(logger => logger.Info(CultureInfo.InvariantCulture, "{0}", "Message"));
        }

        /// <summary>
        /// Test to ensure warn writes.
        /// </summary>
        [Fact]
        public void Warn_Writes_Message()
        {
            Test_Write_Message(logger => logger.Warn(Message));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Warn_Writes_Message_InvariantCulture()
        {
            Test_Write_Message(logger => logger.Warn(CultureInfo.InvariantCulture, "{0}", "Message"));
        }

        /// <summary>
        /// Test to ensure error writes.
        /// </summary>
        [Fact]
        public void Error_Writes_Message()
        {
            Test_Write_Message(logger => logger.Error(Message));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Error_Writes_Message_InvariantCulture()
        {
            Test_Write_Message(logger => logger.Error(CultureInfo.InvariantCulture, "{0}", "Message"));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Fatal_Writes_Message()
        {
            Test_Write_Message(logger => logger.Fatal(Message));
        }

        /// <summary>
        /// Test to ensure fatal writes.
        /// </summary>
        [Fact]
        public void Fatal_Writes_Message_InvariantCulture()
        {
            Test_Write_Message(logger => logger.Fatal(CultureInfo.InvariantCulture, "{0}", "Message"));
        }

        private static void Test_Write_Message(Action<StaticFullLogger> testMethodFunc)
        {
            var textLogger = new TextLogger();
            Assert.Equal(0, textLogger.Logs.Count);
            var staticLogger = GetLogger(textLogger);

            testMethodFunc(staticLogger);
            Assert.Equal(1, textLogger.Logs.Count);
            var line = textLogger.Logs.First();
            Assert.True(line.message.Contains("()"));
        }

        private static StaticFullLogger GetLogger(TextLogger textLogger) => new StaticFullLogger(new WrappingFullLogger(textLogger));
    }
}
