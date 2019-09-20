// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Splat
{
    internal static class DispatcherMixin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Passed to completion source")]
        public static async Task<T> RunTaskAsync<T>(this CoreDispatcher dispatcher, Func<Task<T>> func, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            await dispatcher.RunAsync(priority, async () =>
            {
                try
                {
                    taskCompletionSource.SetResult(await func().ConfigureAwait(false));
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            });
            return await taskCompletionSource.Task;
        }

        // There is no TaskCompletionSource<void> so we use a bool that we throw away.
        public static Task RunTaskAsync(this CoreDispatcher dispatcher, Func<Task> func, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            return RunTaskAsync(
                dispatcher,
                async () =>
                {
                    await func().ConfigureAwait(false);
                    return false;
                },
                priority);
        }
    }
}
