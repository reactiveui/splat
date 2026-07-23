// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Drawing;
using System.Runtime.CompilerServices;

#if !UIKIT
[assembly: TypeForwardedTo(typeof(Color))]
[assembly: TypeForwardedTo(typeof(KnownColor))]
#endif

[assembly: TypeForwardedTo(typeof(Point))]
[assembly: TypeForwardedTo(typeof(PointF))]
[assembly: TypeForwardedTo(typeof(Rectangle))]
[assembly: TypeForwardedTo(typeof(RectangleF))]
[assembly: TypeForwardedTo(typeof(Size))]
[assembly: TypeForwardedTo(typeof(SizeF))]
