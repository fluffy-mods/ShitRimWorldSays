// GameplayTipWindow_ResetTipTimer.cs
// Copyright Karel Kroeze, -2020

using HarmonyLib;
using Verse;

namespace ShitRimWorldSays.Patches
{
    [HarmonyPatch( typeof( GameplayTipWindow ), nameof( GameplayTipWindow.ResetTipTimer ) )]
    public static class GameplayTipWindow_ResetTipTimer
    {
        public static void Postfix()
        {
            TipDatabase.Notify_ResetTimer();
        }
    }
}