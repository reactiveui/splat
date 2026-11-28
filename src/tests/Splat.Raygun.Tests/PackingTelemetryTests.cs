// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections;
using Mindscape.Raygun4Net;

namespace Splat.Tests.ApplicationPerformanceMonitoring;

/// <summary>Runs the label printer example with a local Raygun client.</summary>
public sealed class PackingTelemetryTests
{
    /// <summary>Checks that the client receives the original printer exception.</summary>
    /// <returns>Completion after inspecting the collected exception.</returns>
    [Test]
    public async Task PackingExampleReportsPrinterFault()
    {
        var settings = new RaygunSettings();
        var client = new PrinterLog(settings);
        client.SendingMessage += static (_, message) => message.Cancel = true;
        var error = new IOException("The printer is out of labels.");
        DispatchTelemetry.RaygunPacking.RecordPrinterFault(client, settings, error);
        await Assert.That(client.Error).IsSameReferenceAs(error);
    }

    /// <summary>Collects the printer fault locally.</summary>
    /// <param name="settings">The settings supplied to the example.</param>
    private sealed class PrinterLog(RaygunSettings settings) : RaygunClient(settings)
    {
        /// <summary>Gets the error reported by the label operation.</summary>
        internal Exception? Error { get; private set; }

        /// <inheritdoc/>
        public override Task SendInBackground(Exception exception, IList<string>? tags = null, IDictionary? userCustomData = null, RaygunIdentifierMessage? userInfo = null)
        {
            Error = exception;
            return Task.CompletedTask;
        }
    }
}
