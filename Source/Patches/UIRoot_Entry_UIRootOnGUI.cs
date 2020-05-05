// UIRoot_Entry_UIRootOnGUI.cs
// Copyright Karel Kroeze, 2020-2020

using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ShitRimWorldSays.Patches
{
    [HarmonyPatch(typeof(MainMenuDrawer), nameof( MainMenuDrawer.MainMenuOnGUI ) )]
    public class MainMenuDrawer_MainMenuOnGui
    {
        public static void Postfix()
        {
            if ( !ShitRimWorldSays.Settings.tipsOnMainMenu )
                return;

            var width  = Mathf.Min( UI.screenWidth, 400 );
            var height = TipDatabase.CurrentTip.Height( width );
            var x      = ( UI.screenWidth - width ) / 2;
            var y      = UI.screenHeight - height - 8;

            var canvas = new Rect( x, y, width, height );

            Find.WindowStack.ImmediateWindow( 62893997, canvas, WindowLayer.Super,
                                              delegate
                                              { TipDatabase.CurrentTip.Draw( canvas.AtZero() ); } );
        }
    }
}