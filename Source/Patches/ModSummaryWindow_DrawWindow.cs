// ModSummaryWindow_DrawWindow.cs
// Copyright Karel Kroeze, -2020

using HarmonyLib;
using UnityEngine;
using Verse;

namespace ShitRimWorldSays.Patches {
    [HarmonyPatch(typeof(ModSummaryWindow), nameof(ModSummaryWindow.DrawWindow))]
    public static class ModSummaryWindow_DrawWindow {
        public static void Prefix(ref Vector2 offset) {
            // ModSummaryWindow is only ever rendered if a tip is also rendered, 
            // so use the bottomleft position reported by (our override of)
            // GameplayTipWindow as the offset.
            offset = GameplayTipWindow_DrawWindow.bottomLeft + new Vector2(0, 17f);
        }
    }
}
