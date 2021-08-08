// Tip.cs
// Copyright Karel Kroeze, -2020

using UnityEngine;

namespace ShitRimWorldSays {
    public abstract class Tip {
        protected static Vector2 margin = new Vector2( 15f, 8f );
        public abstract void Draw(Rect rect);
        public abstract float Height(int width);
    }
}
