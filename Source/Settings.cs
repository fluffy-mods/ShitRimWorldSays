
using UnityEngine;
using Verse;

namespace ShitRimWorldSays {
    public class Settings: ModSettings {
        public TipDatabase database = new TipDatabase(); // overridden when scribe loads settings
        public bool replaceGameTips = true;
        public bool tipsOnMainMenu = true;
        public int minimumKarma = 150;
        private string _minimumKarmaBuffer;

        public override void ExposeData() {
            base.ExposeData();

            Scribe_Deep.Look(ref database, "database");
            Scribe_Values.Look(ref replaceGameTips, "replaceGameTips", false);
            Scribe_Values.Look(ref tipsOnMainMenu, "tipsOnMainMenu", true);
            Scribe_Values.Look(ref minimumKarma, "minimumKarma", 150);
        }

        public void DoWindowContents(Rect canvas) {
            Listing_Standard options = new Listing_Standard();
            options.Begin(canvas);
            options.CheckboxLabeled(I18n.ReplaceGameTips, ref replaceGameTips, I18n.ReplaceGameTipsTooltip);
            options.CheckboxLabeled(I18n.TipsOnMainMenu, ref tipsOnMainMenu, I18n.TipsOnMainMenuTooltip);
            options.TextFieldNumericLabeled(I18n.MinimumKarma, ref minimumKarma, ref _minimumKarmaBuffer, 0, 999);
            options.End();
        }
    }
}
