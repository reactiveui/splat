using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests.Logging
{
    /// <summary>
    /// Tests that check the functionality of the <see cref="AllocationFreeLoggerBase"/> class.
    /// </summary>
    /// <typeparam name="T">The type of logger to test.</typeparam>
    [SuppressMessage("Naming", "CA1034: Do not nest type", Justification = "Deliberate usage")]
    public abstract class AllocationFreeLoggerBaseTestBase<T> : FullLoggerTestBase<T>
        where T : IFullLogger, IAllocationFreeLogger, new()
    {
        private static char[] NewLine => Environment.NewLine.ToCharArray();

        private static Exception Exception => new Exception();

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugOneArgumentMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}", 1);
            Assert.Equal("1", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugOneArgumentMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}", 1);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionOneArgumentMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}", 1);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}", 1);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugTwoArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}", 1, 2);
            Assert.Equal("1, 2", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}", 1, 2);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionTwoArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}", 1, 2);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}", 1, 2);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugThreeArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}", 1, 2, 3);
            Assert.Equal("1, 2, 3", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}", 1, 2, 3);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionThreeArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}", 1, 2, 3);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}", 1, 2, 3);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugFourArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Equal("1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionFourArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugFiveArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Equal("1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionFiveArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugSixArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Equal("1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionSixArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes seven arguments.
        /// </summary>
        [Fact]
        public void DebugSevenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes seven arguments.
        /// </summary>
        [Fact]
        public void DebugSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionSevenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes eight arguments.
        /// </summary>
        [Fact]
        public void DebugEightArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes eighth arguments.
        /// </summary>
        [Fact]
        public void DebugEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionEightArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes nine arguments.
        /// </summary>
        [Fact]
        public void DebugNineArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes nine arguments.
        /// </summary>
        [Fact]
        public void DebugNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionNineArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes ten arguments.
        /// </summary>
        [Fact]
        public void DebugTenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes ten arguments.
        /// </summary>
        [Fact]
        public void DebugTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionTenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void DebugExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoOneArgumentMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}", 1);
            Assert.Equal("1", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoOneArgumentMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}", 1);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionOneArgumentMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}", 1);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}", 1);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoTwoArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}", 1, 2);
            Assert.Equal("1, 2", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}", 1, 2);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionTwoArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}", 1, 2);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}", 1, 2);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoThreeArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}", 1, 2, 3);
            Assert.Equal("1, 2, 3", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}", 1, 2, 3);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionThreeArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}", 1, 2, 3);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}", 1, 2, 3);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoFourArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Equal("1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionFourArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoFiveArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Equal("1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionFiveArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoSixArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Equal("1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionSixArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes seven arguments.
        /// </summary>
        [Fact]
        public void InfoSevenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes seven arguments.
        /// </summary>
        [Fact]
        public void InfoSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionSevenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes eight arguments.
        /// </summary>
        [Fact]
        public void InfoEightArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes eight arguments.
        /// </summary>
        [Fact]
        public void InfoEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionEightArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes nine arguments.
        /// </summary>
        [Fact]
        public void InfoNineArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes nine arguments.
        /// </summary>
        [Fact]
        public void InfoNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionNineArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes ten arguments.
        /// </summary>
        [Fact]
        public void InfoTenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Info;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes ten arguments.
        /// </summary>
        [Fact]
        public void InfoTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionTenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void InfoExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnOneArgumentMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}", 1);
            Assert.Equal("1", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnOneArgumentMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Error;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}", 1);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionOneArgumentMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}", 1);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}", 1);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnTwoArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}", 1, 2);
            Assert.Equal("1, 2", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Error;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}", 1, 2);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionTwoArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}", 1, 2);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}", 1, 2);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnThreeArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}", 1, 2, 3);
            Assert.Equal("1, 2, 3", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Error;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}", 1, 2, 3);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionThreeArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}", 1, 2, 3);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}", 1, 2, 3);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnFourArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Equal("1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Error;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionFourArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnFiveArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Equal("1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Error;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionFiveArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnSixArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Equal("1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Error;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionSixArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes seven arguments.
        /// </summary>
        [Fact]
        public void WarnSevenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes seven arguments.
        /// </summary>
        [Fact]
        public void WarnSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionSevenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes eighth arguments.
        /// </summary>
        [Fact]
        public void WarnEightArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes eighth arguments.
        /// </summary>
        [Fact]
        public void WarnEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionEightArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes nine arguments.
        /// </summary>
        [Fact]
        public void WarnNineArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes nine arguments.
        /// </summary>
        [Fact]
        public void WarnNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionNineArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes ten arguments.
        /// </summary>
        [Fact]
        public void WarnTenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes ten arguments.
        /// </summary>
        [Fact]
        public void WarnTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionTenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void WarnExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorOneArgumentMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}", 1);
            Assert.Equal("1", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorOneArgumentMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}", 1);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionOneArgumentMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}", 1);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionOneArgumentMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}", 1);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorTwoArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}", 1, 2);
            Assert.Equal("1, 2", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}", 1, 2);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionTwoArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}", 1, 2);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionTwoArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}", 1, 2);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorThreeArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}", 1, 2, 3);
            Assert.Equal("1, 2, 3", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}", 1, 2, 3);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionThreeArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}", 1, 2, 3);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionThreeArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}", 1, 2, 3);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorFourArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Equal("1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Error;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionFourArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionFourArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorFiveArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Equal("1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Error;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionFiveArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionFiveArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorSixArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Equal("1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Error;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionSixArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionSixArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes seven arguments.
        /// </summary>
        [Fact]
        public void ErrorSevenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes seven arguments.
        /// </summary>
        [Fact]
        public void ErrorSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionSevenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionSevenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes eighth arguments.
        /// </summary>
        [Fact]
        public void ErrorEightArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes eighth arguments.
        /// </summary>
        [Fact]
        public void ErrorEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionEightArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionEightArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes nine arguments.
        /// </summary>
        [Fact]
        public void ErrorNineArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes nine arguments.
        /// </summary>
        [Fact]
        public void ErrorNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionNineArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionNineArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes ten arguments.
        /// </summary>
        [Fact]
        public void ErrorTenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes ten arguments.
        /// </summary>
        [Fact]
        public void ErrorTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionTenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void ErrorExceptionTenArgumentsMethod_Should_Not_Write_If_Higher_Level()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Fatal;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Null(inner.Value);
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalOneArgumentMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal("{0}", 1);
            Assert.Equal("1", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalExceptionOneArgumentMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal(Exception, "{0}", 1);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalTwoArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal("{0}, {1}", 1, 2);
            Assert.Equal("1, 2", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalExceptionTwoArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal(Exception, "{0}, {1}", 1, 2);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalThreeArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal("{0}, {1}, {2}", 1, 2, 3);
            Assert.Equal("1, 2, 3", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalExceptionThreeArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal(Exception, "{0}, {1}, {2}", 1, 2, 3);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalFourArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal("{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Equal("1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalExceptionFourArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalFiveArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Equal("1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalExceptionFiveArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalSixArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Equal("1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalExceptionSixArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes seven arguments.
        /// </summary>
        [Fact]
        public void FatalSevenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalExceptionSevenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes eighth arguments.
        /// </summary>
        [Fact]
        public void FatalEightArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalExceptionEightArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes nine arguments.
        /// </summary>
        [Fact]
        public void FatalNineArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalExceptionNineArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes ten arguments.
        /// </summary>
        [Fact]
        public void FatalTenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Warn;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
        }

        /// <summary>
        /// Tests the inner logger writes three arguments.
        /// </summary>
        [Fact]
        public void FatalExceptionTenArgumentsMethod_Should_Write_Message()
        {
            var inner = new TextLogger();
            inner.Level = LogLevel.Debug;
            var logger = new AllocationFreeLoggerBase(inner);
            logger.Fatal(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
        }
    }
}
