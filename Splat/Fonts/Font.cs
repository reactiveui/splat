using System;

namespace Splat {
    public class Font {
        public string Name { get; private set; }

        public float Points { get; private set; }

        public Font (string name, float points) {
            Name = name;
            Points = points;
        }
    }
}

