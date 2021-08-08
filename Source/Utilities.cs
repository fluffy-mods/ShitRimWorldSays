// Utilities.cs
// Copyright Karel Kroeze, -2020

using UnityEngine;

namespace ShitRimWorldSays {
    public static class Utilities {
        public static Rect ContractedBy(this Rect rect, Vector2 margin) {
            return new Rect(rect.min + margin, rect.size - (2 * margin));
        }

        public static Vector2 BottomLeft(this Rect rect) {
            return new Vector2(rect.xMin, rect.yMax);
        }

        public static string Italic(this string msg) {
            return $"<i>{msg}</i>";
        }

        public static Color Darken(this Color color, float amount) {
            return color * (1 - amount);
        }
    }
}
