using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShitRimWorldSays
{
	static class Log
	{
		[System.Diagnostics.Conditional("DEBUG")]
		public static void Message(string msg )
		{
			Verse.Log.Message( $"ShitRimWorldSays :: {msg}");
		}
	}
}
