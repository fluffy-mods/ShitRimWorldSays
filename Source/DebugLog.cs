using System.Diagnostics;

namespace ShitRimWorldSays {
    internal static class Log {
        public static void Message(string msg) {

            Verse.Log.Message($"ShitRimWorldSays :: {msg}");
        }

        [Conditional("DEBUG")]
        public static void Debug(string msg) {
            Message(msg);
        }
    }
}
