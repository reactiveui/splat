using System;
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