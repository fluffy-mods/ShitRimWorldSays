using System.Security.Permissions;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ShitRimWorldSays
{
	public class ShitRimWorldSays : Mod
	{
		public ShitRimWorldSays(ModContentPack content) : base(content)
        {
            // keep ref to instance
            Instance = this;

            // init settings 
            GetSettings<Settings>();

            // fetch (new) quotes
            TipDatabase.FetchNewQuotes();

            // apply harmony patches
            var harmony = new Harmony( "Fluffy.ShitRimWorldSays" );
            harmony.PatchAll();
        }

        public static ShitRimWorldSays Instance { get; protected set; }

        public static Settings Settings => Instance.GetSettings<Settings>();

        public override string SettingsCategory() => I18n.ShitRimWorldSays;

        public override void DoSettingsWindowContents( Rect canvas )
        {
            Settings.DoWindowContents( canvas );
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
            TipDatabase.Notify_TipsUpdated();
        }
    }
}