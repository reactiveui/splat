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
    [SuppressMessage("Naming", "CA1034: Do not nest type", Justification = "Deliberate usage")]
    public static class AllocationFreeLoggerBaseTests
    {
        private static char[] NewLine => Environment.NewLine.ToCharArray();

        private static Exception Exception => new Exception();

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugOneArgumentMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug("{0}", 1);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugExceptionOneArgumentMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}", 1);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with five arguments.
        /// </summary>
        public class DebugTwoArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug("{0}, {1}", 1, 2);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugExceptionTwoArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}, {1}", 1, 2);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with six arguments.
        /// </summary>
        public class DebugThreeArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug("{0}, {1}, {2}", 1, 2, 3);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugExceptionThreeArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}, {1}, {2}", 1, 2, 3);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with four arguments.
        /// </summary>
        public class DebugFourArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug("{0}, {1}, {2}, {3}", 1, 2, 3, 4);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugExceptionFourArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with five arguments.
        /// </summary>
        public class DebugFiveArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugExceptionFiveArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with six arguments.
        /// </summary>
        public class DebugSixArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugExceptionSixArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with seven arguments.
        /// </summary>
        public class DebugSevenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes seven arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugExceptionSevenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with eight arguments.
        /// </summary>
        public class DebugEightArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes eight arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugExceptionEightArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with nine arguments.
        /// </summary>
        public class DebugNineArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes nine arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugExceptionNineArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with ten arguments.
        /// </summary>
        public class DebugTenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes ten arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class DebugExceptionTenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the info method with one argument.
        /// </summary>
        public class InfoOneArgumentMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}", 1);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class InfoExceptionOneArgumentMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}", 1);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the info method with two arguments.
        /// </summary>
        public class InfoTwoArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}", 1, 2);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class InfoExceptionTwoArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}", 1, 2);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the info method with three arguments.
        /// </summary>
        public class InfoThreeArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}", 1, 2, 3);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class InfoExceptionThreeArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}", 1, 2, 3);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with four arguments.
        /// </summary>
        public class InfoFourArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class InfoExceptionFourArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with five arguments.
        /// </summary>
        public class InfoFiveArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class InfoExceptionFiveArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with six arguments.
        /// </summary>
        public class InfoSixArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class InfoExceptionSixArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the info method with seven arguments.
        /// </summary>
        public class InfoSevenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes seven arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class InfoExceptionSevenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Info;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Debug(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the info method with eight arguments.
        /// </summary>
        public class InfoEightArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes eight arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class InfoExceptionEightArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the info method with nine arguments.
        /// </summary>
        public class InfoNineArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes nine arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class InfoExceptionNineArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the info method with ten arguments.
        /// </summary>
        public class InfoTenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes ten arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class InfoExceptionTenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the warn method with one argument.
        /// </summary>
        public class WarnOneArgumentMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Error;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn("{0}", 1);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class WarnExceptionOneArgumentMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn(Exception, "{0}", 1);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the warn method with two arguments.
        /// </summary>
        public class WarnTwoArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Error;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn("{0}, {1}", 1, 2);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class WarnExceptionTwoArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn(Exception, "{0}, {1}", 1, 2);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the warn method with three arguments.
        /// </summary>
        public class WarnThreeArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Error;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn("{0}, {1}, {2}", 1, 2, 3);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class WarnExceptionThreeArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn(Exception, "{0}, {1}, {2}", 1, 2, 3);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with four arguments.
        /// </summary>
        public class WarnFourArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Error;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class WarnExceptionFourArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with five arguments.
        /// </summary>
        public class WarnFiveArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Error;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class WarnExceptionFiveArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with six arguments.
        /// </summary>
        public class WarnSixArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Error;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class WarnExceptionSixArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the warn method with seven arguments.
        /// </summary>
        public class WarnSevenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes seven arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class WarnExceptionSevenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the warn method with eighth arguments.
        /// </summary>
        public class WarnEightArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes eighth arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class WarnExceptionEightArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the warn method with nine arguments.
        /// </summary>
        public class WarnNineArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes nine arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class WarnExceptionNineArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the warn method with ten arguments.
        /// </summary>
        public class WarnTenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes ten arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class WarnExceptionTenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Warn(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorOneArgumentMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error("{0}", 1);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorExceptionOneArgumentMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error(Exception, "{0}", 1);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with two arguments.
        /// </summary>
        public class ErrorTwoArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error("{0}, {1}", 1, 2);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorExceptionTwoArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error(Exception, "{0}, {1}", 1, 2);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with three arguments.
        /// </summary>
        public class ErrorThreeArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error("{0}, {1}, {2}", 1, 2, 3);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorExceptionThreeArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error(Exception, "{0}, {1}, {2}", 1, 2, 3);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with four arguments.
        /// </summary>
        public class ErrorFourArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Error;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}", 1, 2, 3, 4);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorExceptionFourArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with five arguments.
        /// </summary>
        public class ErrorFiveArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Error;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorExceptionFiveArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with six arguments.
        /// </summary>
        public class ErrorSixArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Error;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorExceptionSixArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with seven arguments.
        /// </summary>
        public class ErrorSevenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes seven arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorExceptionSevenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with eighth arguments.
        /// </summary>
        public class ErrorEightArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes eighth arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorExceptionEightArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with nine arguments.
        /// </summary>
        public class ErrorNineArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes nine arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorExceptionNineArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with ten arguments.
        /// </summary>
        public class ErrorTenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes ten arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the error method with one argument.
        /// </summary>
        public class ErrorExceptionTenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
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
            public void Should_Not_Write_If_Higher_Level()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Fatal;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Error(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Null(inner.Value);
            }
        }

        /// <summary>
        /// Tests that check the functionality of the fatal method with one argument.
        /// </summary>
        public class FatalOneArgumentMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal("{0}", 1);

                Assert.Equal("1", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class FatalExceptionOneArgumentMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Debug;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal(Exception, "{0}", 1);

                Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the fatal method with two arguments.
        /// </summary>
        public class FatalTwoArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal("{0}, {1}", 1, 2);

                Assert.Equal("1, 2", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class FatalExceptionTwoArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Debug;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal(Exception, "{0}, {1}", 1, 2);

                Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the fatal method with three arguments.
        /// </summary>
        public class FatalThreeArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal("{0}, {1}, {2}", 1, 2, 3);

                Assert.Equal("1, 2, 3", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class FatalExceptionThreeArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Debug;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal(Exception, "{0}, {1}, {2}", 1, 2, 3);

                Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with four arguments.
        /// </summary>
        public class FatalFourArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal("{0}, {1}, {2}, {3}", 1, 2, 3, 4);

                Assert.Equal("1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class FatalExceptionFourArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Debug;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}, {3}", 1, 2, 3, 4);

                Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with five arguments.
        /// </summary>
        public class FatalFiveArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal("{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);

                Assert.Equal("1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class FatalExceptionFiveArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Debug;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal(Exception, "{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);

                Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with six arguments.
        /// </summary>
        public class FatalSixArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);

                Assert.Equal("1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class FatalExceptionSixArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Debug;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Info(Exception, "{0}, {1}, {2}, {3}, {4}, {5}", 1, 2, 3, 4, 5, 6);

                Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the fatal method with seven arguments.
        /// </summary>
        public class FatalSevenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes seven arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);

                Assert.Equal("1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class FatalExceptionSevenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Debug;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}", 1, 2, 3, 4, 5, 6, 7);

                Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the fatal method with eighth arguments.
        /// </summary>
        public class FatalEightArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes eighth arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);

                Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class FatalExceptionEightArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Debug;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 1, 2, 3, 4, 5, 6, 7, 8);

                Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the fatal method with nine arguments.
        /// </summary>
        public class FatalNineArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes nine arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);

                Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class FatalExceptionNineArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Debug;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", 1, 2, 3, 4, 5, 6, 7, 8, 9);

                Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the fatal method with ten arguments.
        /// </summary>
        public class FatalTenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes ten arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Warn;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Equal("1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
            }
        }

        /// <summary>
        /// Tests that check the functionality of the debug method with one argument.
        /// </summary>
        public class FatalExceptionTenArgumentsMethod
        {
            /// <summary>
            /// Tests the inner logger writes three arguments.
            /// </summary>
            [Fact]
            public void Should_Write_Message()
            {
                var inner = new TextLogger();
                inner.Level = LogLevel.Debug;
                var logger = new AllocationFreeLoggerBase(inner);

                logger.Fatal(Exception, "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

                Assert.Equal("System.Exception: Exception of type 'System.Exception' was thrown.: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10", inner.Value.TrimEnd(NewLine));
            }
        }
    }
}
