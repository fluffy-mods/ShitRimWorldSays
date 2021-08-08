using HarmonyLib;
using UnityEngine;
using Verse;

namespace ShitRimWorldSays.Patches {
    [HarmonyPatch(typeof(GameplayTipWindow), nameof(GameplayTipWindow.DrawWindow))]
    public static class GameplayTipWindow_DrawWindow {
        public static int WindowWidth = 776;
        public static Vector2 bottomLeft;

        public static bool Prefix(Vector2 offset, bool useWindowStack) {
            Tip tip = TipDatabase.CurrentTip;
            Rect canvas = new Rect( offset.x, offset.y, WindowWidth, tip.Height( WindowWidth ) );

            // set bottomLeft, so we know where the mod info panel thing starts.
            bottomLeft = canvas.BottomLeft();

            // draw window contents
            if (useWindowStack) {
                // using window stack when available
                Find.WindowStack.ImmediateWindow(62893997, canvas, WindowLayer.Super,
                                                  delegate { tip.Draw(canvas.AtZero()); });
            } else {
                // and manually otherwise
                Widgets.DrawShadowAround(canvas);
                Widgets.DrawWindowBackground(canvas);
                tip.Draw(canvas);
            }

            // this replaces the vanilla version completely, so return false.
            return false;
        }
    }
}
