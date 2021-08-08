// Tip_Quote.cs
// Copyright Karel Kroeze, -2020

using System;
using UnityEngine;
using Verse;

namespace ShitRimWorldSays {
    public class Tip_Quote: Tip, IExposable, IEquatable<Tip_Quote> {
        public string author;
        public string body;
        public string permalink;
        public int score;

        public Tip_Quote() {
            // scribe only
        }

        public Tip_Quote(string author, string body, string permalink, int score) {
            this.author = author;
            this.body = body;
            this.permalink = permalink;
            this.score = score;
        }

        public static implicit operator Tip_Quote((string, string, string, int) tuple) {
            return new Tip_Quote(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }

        public void ExposeData() {
            Scribe_Values.Look(ref author, "author");
            Scribe_Values.Look(ref body, "body");
            Scribe_Values.Look(ref permalink, "permalink");
            Scribe_Values.Look(ref score, "score");
        }

        public bool Equals(Tip_Quote other) {
            if (other is null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return string.Equals(permalink, other.permalink);
        }

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != GetType()) {
                return false;
            }

            return Equals((Tip_Quote) obj);
        }

        public override int GetHashCode() {
            return permalink != null ? permalink.GetHashCode() : 0;
        }

        public static bool operator ==(Tip_Quote left, Tip_Quote right) {
            return Equals(left, right);
        }

        public static bool operator !=(Tip_Quote left, Tip_Quote right) {
            return !Equals(left, right);
        }

        public override void Draw(Rect rect) {
            Rect bodyRect   = rect.ContractedBy( margin ),
                 authorRect = rect.ContractedBy( margin );
            bodyRect.yMax -= 30;
            authorRect.yMin = bodyRect.yMax;

            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(bodyRect, body);
            Text.Anchor = TextAnchor.LowerRight;
            GUI.color = Mouse.IsOver(authorRect) ? GenUI.MouseoverColor : GenUI.MouseoverColor.Darken(.2f);
            Widgets.Label(authorRect, $" - {author}".Italic());
            if (!permalink.NullOrEmpty() && Widgets.ButtonInvisible(authorRect)) {
                Application.OpenURL($"https://reddit.com/{permalink}");
            }

            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;

            Rect refreshRect = new Rect( rect.xMax - 24 - 4, rect.yMin + 4, 24, 24 );
            if (Mouse.IsOver(rect) && Widgets.ButtonImage(refreshRect, Resources.Refresh)) {
                TipDatabase.Notify_ResetTimer(true);
            }
        }

        public override float Height(int width) {
            return Text.CalcHeight(body, width - (2 * margin.x)) + 30 + (Window.StandardMargin * 2);
        }
    }
}
