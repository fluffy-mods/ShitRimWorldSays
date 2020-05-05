// I18n.cs
// Copyright Karel Kroeze, -2020

using UnityEngine;
using Verse;

namespace ShitRimWorldSays
{
    public class I18n
    {
        private static string Translate( string key, params NamedArgument[] args )
        {
            return Key( key ).Translate( args ).Resolve();
        }

        private static string Key( string key )
        {
            return $"Fluffy.ShitRimWorldSays.{key}";
        }

        public static string ShitRimWorldSays       = Translate( "ShitRimWorldSays" );
        public static string ReplaceGameTips        = Translate( "ReplaceGameTips" );
        public static string ReplaceGameTipsTooltip = Translate( "ReplaceGameTips.Tooltip" );
        public static string MinimumKarma           = Translate( "MinimumKarma" );
        public static string TipsOnMainMenu         = Translate( "TipsOnMainMenu" );
        public static string TipsOnMainMenuTooltip  = Translate( "TipsOnMainMenu.Tooltip" );
    }

    [StaticConstructorOnStartup]
    public static class Resources
    {
        public static Texture2D Refresh = ContentFinder<Texture2D>.Get( "UI/Icons/refresh" );
    }
}