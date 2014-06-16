using System;
using MonoTouch.UIKit;

namespace Splat {

    public static class FontExtensions {

        public static UIFont ToNative(this Font This) {
            return UIFont.FromName(This.Name, This.Points);
        }
    }
}

