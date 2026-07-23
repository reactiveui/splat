// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>
/// Shared constants used across the logging test fixtures. These replace the magic numbers and
/// duplicated string literals that the allocation-free logging overload tests would otherwise
/// repeat for every argument count and log level.
/// </summary>
internal static class LoggerTestConstants
{
    /// <summary>The first positional sample argument.</summary>
    internal const int Arg1 = 1;

    /// <summary>The second positional sample argument.</summary>
    internal const int Arg2 = 2;

    /// <summary>The third positional sample argument.</summary>
    internal const int Arg3 = 3;

    /// <summary>The fourth positional sample argument.</summary>
    internal const int Arg4 = 4;

    /// <summary>The fifth positional sample argument.</summary>
    internal const int Arg5 = 5;

    /// <summary>The sixth positional sample argument.</summary>
    internal const int Arg6 = 6;

    /// <summary>The seventh positional sample argument.</summary>
    internal const int Arg7 = 7;

    /// <summary>The eighth positional sample argument.</summary>
    internal const int Arg8 = 8;

    /// <summary>The ninth positional sample argument.</summary>
    internal const int Arg9 = 9;

    /// <summary>The tenth positional sample argument.</summary>
    internal const int Arg10 = 10;

    /// <summary>The composite format string for a single argument.</summary>
    internal const string Format1 = "{0}";

    /// <summary>The composite format string for two arguments.</summary>
    internal const string Format2 = "{0}, {1}";

    /// <summary>The composite format string for three arguments.</summary>
    internal const string Format3 = "{0}, {1}, {2}";

    /// <summary>The composite format string for four arguments.</summary>
    internal const string Format4 = "{0}, {1}, {2}, {3}";

    /// <summary>The composite format string for five arguments.</summary>
    internal const string Format5 = "{0}, {1}, {2}, {3}, {4}";

    /// <summary>The composite format string for six arguments.</summary>
    internal const string Format6 = "{0}, {1}, {2}, {3}, {4}, {5}";

    /// <summary>The composite format string for seven arguments.</summary>
    internal const string Format7 = "{0}, {1}, {2}, {3}, {4}, {5}, {6}";

    /// <summary>The composite format string for eight arguments.</summary>
    internal const string Format8 = "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}";

    /// <summary>The composite format string for nine arguments.</summary>
    internal const string Format9 = "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";

    /// <summary>The composite format string for ten arguments.</summary>
    internal const string Format10 = "{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}";

    /// <summary>The expected rendered output for a single argument.</summary>
    internal const string Expected1 = "1";

    /// <summary>The expected rendered output for two arguments.</summary>
    internal const string Expected2 = "1, 2";

    /// <summary>The expected rendered output for three arguments.</summary>
    internal const string Expected3 = "1, 2, 3";

    /// <summary>The expected rendered output for four arguments.</summary>
    internal const string Expected4 = "1, 2, 3, 4";

    /// <summary>The expected rendered output for five arguments.</summary>
    internal const string Expected5 = "1, 2, 3, 4, 5";

    /// <summary>The expected rendered output for six arguments.</summary>
    internal const string Expected6 = "1, 2, 3, 4, 5, 6";

    /// <summary>The expected rendered output for seven arguments.</summary>
    internal const string Expected7 = "1, 2, 3, 4, 5, 6, 7";

    /// <summary>The expected rendered output for eight arguments.</summary>
    internal const string Expected8 = "1, 2, 3, 4, 5, 6, 7, 8";

    /// <summary>The expected rendered output for nine arguments.</summary>
    internal const string Expected9 = "1, 2, 3, 4, 5, 6, 7, 8, 9";

    /// <summary>The expected rendered output for ten arguments.</summary>
    internal const string Expected10 = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10";

    /// <summary>The suffix appended when the rendered output also includes the test exception.</summary>
    internal const string ExceptionSuffix = " System.Exception: Exception of type 'System.Exception' was thrown.";

    /// <summary>The expected rendered output for a single argument followed by the exception.</summary>
    internal const string ExpectedException1 = Expected1 + ExceptionSuffix;

    /// <summary>The expected rendered output for two arguments followed by the exception.</summary>
    internal const string ExpectedException2 = Expected2 + ExceptionSuffix;

    /// <summary>The expected rendered output for three arguments followed by the exception.</summary>
    internal const string ExpectedException3 = Expected3 + ExceptionSuffix;

    /// <summary>The expected rendered output for four arguments followed by the exception.</summary>
    internal const string ExpectedException4 = Expected4 + ExceptionSuffix;

    /// <summary>The expected rendered output for five arguments followed by the exception.</summary>
    internal const string ExpectedException5 = Expected5 + ExceptionSuffix;

    /// <summary>The expected rendered output for six arguments followed by the exception.</summary>
    internal const string ExpectedException6 = Expected6 + ExceptionSuffix;

    /// <summary>The expected rendered output for seven arguments followed by the exception.</summary>
    internal const string ExpectedException7 = Expected7 + ExceptionSuffix;

    /// <summary>The expected rendered output for eight arguments followed by the exception.</summary>
    internal const string ExpectedException8 = Expected8 + ExceptionSuffix;

    /// <summary>The expected rendered output for nine arguments followed by the exception.</summary>
    internal const string ExpectedException9 = Expected9 + ExceptionSuffix;

    /// <summary>The expected rendered output for ten arguments followed by the exception.</summary>
    internal const string ExpectedException10 = Expected10 + ExceptionSuffix;

    /// <summary>The canonical test message reused by the simple write tests.</summary>
    internal const string TestMessage = "This is a test.";
}
