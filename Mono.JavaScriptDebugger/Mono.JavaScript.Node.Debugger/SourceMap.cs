// Not working at all.
using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.Library;
using Jurassic;

namespace Mono.JavaScript.Debugger
{
	public class SourceMap
	{
		public struct Entry
		{
			public readonly string Name;
			public readonly string Source;
			public readonly int SourceLine;
			public readonly int SourceColumn;
			public readonly int GeneratedPosition;

			public Entry (int generatedPosition, string source, int sourceLine, int sourceColumn, string name)
			{
				GeneratedPosition = generatedPosition;
				Source = source;
				SourceLine = sourceLine;
				SourceColumn = sourceColumn;
				Name = name;
			}
		}

		public struct RawEntry
		{
			public string RawString;
			public readonly int GeneratedPosition;
			public readonly int SourceIndex;
			public readonly int SourceLine;
			public readonly int SourceColumn;
			public readonly int NameIndex;

			public RawEntry (string rawString, int generatedPosition, int sourceIndex, int sourceLine, int sourceColumn, int nameIndex)
			{
				RawString = rawString;
				GeneratedPosition = generatedPosition;
				SourceIndex = sourceIndex;
				SourceLine = sourceLine;
				SourceColumn = sourceColumn;
				NameIndex = nameIndex;
			}
		}

		public SourceMap (ScriptEngine engine)
		{
			if (engine == null)
				throw new ArgumentNullException ("engine");
			this.engine = engine;
			Sources = new List<string> ();
			Names = new List<string> ();
			RawMappings = new List<RawEntry> ();
		}

		ScriptEngine engine;

		public double Version { get; set; }
		public string File { get; set; }
		public string SourceRoot { get; set; }
		public IList<string> Sources { get; private set; }
		public IList<string> Names { get; private set; }
		public IList<RawEntry> RawMappings { get; private set; }

		IEnumerable<RawEntry> ParseMappings (string value)
		{
			// FIXME: ToArray() is only for debugging
			return value.Split (';').SelectMany (segment => ParseMappingSegment (segment.Split (',').ToArray ()));
		}

		IEnumerable<int> ParseVlq (string vlq)
		{
			int sum = 0;
			for (int i = 0; i < vlq.Length; i++) {
				var v = FromBase64Char (vlq [i]);
				if (v < 32) {
					yield return (sum << 5) + v;
					sum = 0;
				}
				else
					sum = (sum << 5) + v - 32;
			}
			if (sum != 0)
				throw new ArgumentException ("Invalid VLQ value: " + vlq);
		}

		int FromBase64Char (char c)
		{
			if ('A' <= c && c <= 'Z')
				return c - 'A';
			else if ('a' <= c && c <= 'z')
				return c - 'a' + 26;
			else if ('0' <= c && c <= '9')
				return c - '0' + 52;
			else {
				switch (c) {
				case '+': return 62;
				case '/': return 63;
				}
				throw new ArgumentException (string.Format ("Invalid BASE64 character: 0x{0:X}", (int) c));
			}
		}

		IEnumerable<RawEntry> ParseMappingSegment (string [] vlqs)
		{
			int gencol = 0;
			int srcidx = 0;
			int srcline = 0;
			int srccol = 0;
			bool first = true;
			foreach (var vlq in vlqs) {
				var vals = ParseVlq (vlq).GetEnumerator ();
				if (first) {
					first = false;
					yield return new RawEntry (vlq,
					                           gencol = (vals.MoveNext () ? vals.Current : 0),
					                           srcidx = (vals.MoveNext () ? vals.Current : 0),
					                           srcline = (vals.MoveNext () ? vals.Current : 0),
					                           srccol = (vals.MoveNext () ? vals.Current : 0),
					                           vals.MoveNext () ? vals.Current : -1);
				} else {
					yield return new RawEntry (vlq,
					                           gencol += (vals.MoveNext () ? vals.Current : 0),
					                           srcidx += (vals.MoveNext () ? vals.Current : 0),
					                           srcline += (vals.MoveNext () ? vals.Current : 0),
					                           srccol += (vals.MoveNext () ? vals.Current : 0),
					                           vals.MoveNext () ? vals.Current : -1);
				}
			}
		}

		public void Read (string json)
		{
			var obj = (ObjectInstance) JSONObject.Parse (engine, json);
			Version = (int) (double) obj ["version"];
			if (Version != 3)
				throw new ArgumentException ("sourcemap version 3 is expected, but was " + (obj ["version"] ?? "empty"));
			File = (string) obj ["file"];
			SourceRoot = (string) obj ["sourceRoot"];
			Sources.Clear ();
			var sources = (ArrayInstance)obj ["sources"];
			for (int i = 0; i < sources.Length; i++)
				Sources.Add ((string) sources [i]);
			Names.Clear ();
			var names = (ArrayInstance)obj ["names"];
			for (int i = 0; i < names.Length; i++)
				Names.Add ((string) names [i]);
			RawMappings = ParseMappings ((string) obj ["mappings"]).ToArray ();
		}

		public IEnumerable<Entry> Entries {
			get {
				foreach (var raw in RawMappings)
					yield return new Entry (raw.GeneratedPosition,
					                        Sources [raw.SourceIndex],
					                        raw.SourceLine,
					                        raw.SourceColumn,
					                        raw.NameIndex >= 0 ? Names [raw.NameIndex] : null);
			}
		}
	}
}
