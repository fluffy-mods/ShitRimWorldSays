// Post.cs
// Copyright Karel Kroeze, -2020

using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShitRimWorldSays {
    public class Post {
        private static readonly Regex urlRegex = new Regex( @"reddit\.com\/r\/(?<sub>\w+)\/comments\/(?<post>(\w|\d)+)\/.+?\/(?<reply>(\w|\d)+)?" );

        public string title { get; set; }
        public string author { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string permalink { get; set; }
        public int score { get; set; }
        public string selftext { get; set; }
        public bool is_self { get; set; }

        public async Task<Tip_Quote> getQuote() {
            // no link at all
            if (string.IsNullOrEmpty(url)) {
                return (author, title, permalink, score);
            }

            // non-reddit link
            Match match = urlRegex.Match( url );
            if (!match.Success) {
                return (author, title, permalink, score);
            }

            string sub   = match.Groups["sub"].Value;
            string post  = match.Groups["post"].Value;
            string reply = match.Groups["reply"].Value;

            if (reply != string.Empty) {
                return await getQuoteFromReply(sub, reply);
            }

            return await getQuoteFromPost(sub, post);
        }

        private async Task<Tip_Quote> getQuoteFromReply(string sub, string replyId) {
            string url = $"https://reddit.com/r/{sub}/api/info.json?id=t1_{replyId}";
            try {
                using WebClient http = new WebClient();
                http.Headers.Add("user-agent", "shit-rimworld-says rimworld mod v0.1");
                string data  = await http.DownloadStringTaskAsync( url );
                JObject json  = JObject.Parse( data );
                Reply reply = JsonConvert.DeserializeObject<Reply>( json["data"]["children"][0]["data"].ToString() );
                Tip_Quote quote = new Tip_Quote( reply.author, reply.body, reply.permalink, score );
                if (quote.body == "[deleted]") {
                    return null;
                }

                return quote;
            } catch (Exception exception) {
                Log.Debug($"failed to fetch quote from reply:\n{exception}");
                return null;
            }
        }

        private async Task<Tip_Quote> getQuoteFromPost(string sub, string postId) {
            string url = $"https://reddit.com/r/{sub}/api/info.json?id=t3_{postId}";
            try {
                using WebClient http = new WebClient();
                http.Headers.Add("user-agent", "shit-rimworld-says rimworld mod v0.1");
                string data = await http.DownloadStringTaskAsync( url );
                JObject json = JObject.Parse( data );
                Post post = JsonConvert.DeserializeObject<Post>( json["data"]["children"][0]["data"].ToString() );
                Tip_Quote quote = new Tip_Quote( post.author, post.is_self ? post.selftext : post.title, post.permalink, score );
                if (quote.body == "[deleted]") {
                    return null;
                }

                return quote;
            } catch (Exception exception) {
                Log.Debug($"failed to fetch quote from post:\n{exception}");
                return null;
            }
        }

        public override string ToString() {
            return $"{title}{(is_self ? "\n" + selftext : "")} \n\t - {author}\n{url}({name})";
        }
    }
}
