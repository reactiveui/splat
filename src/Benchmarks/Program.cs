﻿// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using BenchmarkDotNet.Running;

namespace Splat.Benchmarks
{
    /// <summary>
    /// Class which hosts the main entry point into the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point into the benchmarking application.
        /// </summary>
        /// <param name="args">Arguments from the command line.</param>
        public static void Main(string[] args)
        {
            var results = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            Console.ReadLine();
        }
    }
}
