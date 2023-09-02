// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Tests.Mocks;

namespace Splat.Tests.Logging;

/// <summary>
/// Tests the <see cref="WrappingPrefixLogger"/> class.
/// </summary>
public class WrappingPrefixLoggerTests
{
    private static char[] NewLine => Environment.NewLine.ToCharArray();

    /// <summary>
    /// Test to make sure the message writes.
    /// </summary>
    [Fact]
    public void Write_Should_Write_Message()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Write("This is a test.", LogLevel.Debug);

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Write_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Write("This is a test.", typeof(DummyObjectClass1), LogLevel.Debug);

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Debug_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Debug<DummyObjectClass1>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Debug_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Debug<DummyObjectClass2>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Info_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Info<DummyObjectClass1>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Info_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Info<DummyObjectClass2>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Warn_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Warn<DummyObjectClass1>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Warn_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Warn<DummyObjectClass2>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Error_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Error<DummyObjectClass1>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Error_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Error<DummyObjectClass2>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Fatal_With_Generic_Type_Should_Write_Message_And_Type()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Fatal<DummyObjectClass1>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass1)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }

    /// <summary>
    /// Test to make sure the generic type parameter is passed to the logger.
    /// </summary>
    [Fact]
    public void Fatal_With_Generic_Type_Should_Write_Message_And_Type_Provided()
    {
        var textLogger = new TextLogger();
        var logger = new WrappingFullLogger(new WrappingPrefixLogger(textLogger, typeof(DummyObjectClass1)));

        logger.Fatal<DummyObjectClass2>("This is a test.");

        Assert.Equal($"{nameof(DummyObjectClass2)}: This is a test.", textLogger.Logs.Last().message.Trim(NewLine).Trim());
    }
}
