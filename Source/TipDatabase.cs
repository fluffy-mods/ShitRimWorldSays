// TipDatabase.cs
// Copyright Karel Kroeze, 2020-2020

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Verse;

namespace ShitRimWorldSays {
    public class TipDatabase: IExposable {
        private static HashSet<Tip_Quote> _quotes = new HashSet<Tip_Quote>();
        private static List<Tip_Gameplay> _vanilla;
        private static List<Tip> _tips;

        public static List<Tip> Tips {
            get {
                _vanilla ??= DefDatabase<TipSetDef>.AllDefsListForReading
                                                 .SelectMany(set => set.tips)
                                                 .Select(tip => (Tip_Gameplay) tip)
                                                 .ToList();

                if (ShitRimWorldSays.Settings.replaceGameTips) {
                    _tips ??= _quotes.InRandomOrder().ToList<Tip>();
                } else {
                    _tips ??= _quotes.Cast<Tip>()
                                   .Concat(_vanilla)
                                   .InRandomOrder()
                                   .ToList();
                }

                return _tips;
            }
        }

        private static int _currentTipIndex;
        private static float _lastUpdateTime;
        public static void Notify_TipsUpdated() {
            _tips = null;
            _vanilla = null;
            _quotes = _quotes.Where(q => q.score >= ShitRimWorldSays.Settings.minimumKarma).ToHashSet();
            _currentTipIndex = 0;
            Notify_ResetTimer(true);
        }

        public static void Notify_ResetTimer(bool force = false) {
            // TODO: find less brute force solution
            if (force || LongEventHandler.AnyEventNowOrWaiting) {
                _lastUpdateTime = -1;
            }
            //            Log.Debug( $"Reset timer: {StackTraceUtility.ExtractStackTrace()}"  );
        }

        public static Tip CurrentTip {
            get {
                if (!Tips.Any()) {
                    return new Tip_Quote("Fluffy", "no quotes found", null, 999);
                }
                if (Time.realtimeSinceStartup - _lastUpdateTime > 17.5 ||
                     _lastUpdateTime < 0) {
                    _currentTipIndex = (_currentTipIndex + 1) % Tips.Count;
                    _lastUpdateTime = Time.realtimeSinceStartup;
                }

                return Tips[_currentTipIndex];
            }
        }

        public static async void FetchNewQuotes() {
            try {
                using WebClient http = new WebClient();
                http.Headers.Add("user-agent", "shit-rimworld-says rimworld mod v0.1");
                string data = await http.DownloadStringTaskAsync( "https://reddit.com/r/ShitRimworldSays/hot/.json?limit=100" );
                JObject json = JObject.Parse( data );
                IEnumerable<Task<Tip_Quote>> quoteTasks = json["data"]["children"].Select( postJson => JsonConvert.DeserializeObject<Post>( postJson["data"].ToString() ) )
                                                             .Where( p => p.score >= ShitRimWorldSays.Settings.minimumKarma )
                                                             .Select( p => p.getQuote() );
                List<Tip_Quote> newQuotes = ( await Task.WhenAll( quoteTasks ) )
                                .Where( q => q != null  )
                                .ToList();
                _quotes.AddRange(newQuotes);

                // all the information is static, but this ensures we don't
                // accidentally load/safe a null instance in the settings.
                ShitRimWorldSays.Settings.database = new TipDatabase();
                ShitRimWorldSays.Settings.Write();
                Notify_TipsUpdated();
            } catch (Exception exception) {
                Log.Debug($"failed fetching quotes:\n{exception}");
            }
        }

        public void ExposeData() {
            Scribe_Collections.Look(ref _quotes, "quotes");
        }
    }
}
