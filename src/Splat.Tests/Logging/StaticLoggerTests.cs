using System;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Unit Tests for the Static Logging wrapper implementation.
    /// </summary>
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
        /// Test to ensure info writes.
        /// </summary>
        [Fact]
        public void Info_Writes_Message()
        {
            Test_Write_Message(logger => logger.Info(Message));
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
        public void Fatal_Writes_Message()
        {
            Test_Write_Message(logger => logger.Fatal(Message));
        }

        private static void Test_Write_Message(Action<StaticFullLogger> testMethodFunc)
        {
            var textLogger = new TextLogger();
            Assert.Equal(0, textLogger.Logs.Count);
            var staticLogger = GetLogger(textLogger);

            testMethodFunc(staticLogger);
            Assert.Equal(1, textLogger.Logs.Count);
        }

        private static StaticFullLogger GetLogger(TextLogger textLogger) => new StaticFullLogger(new WrappingFullLogger(textLogger));
    }
}
