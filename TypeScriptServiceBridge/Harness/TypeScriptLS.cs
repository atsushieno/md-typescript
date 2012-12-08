using System;
using Jurassic.Library;
using TypeScriptServiceBridge.Hosting;
using TypeScriptServiceBridge.TypeSystem;

namespace TypeScriptServiceBridge.Harness
{
	public class TypeScriptLS : TypeScriptObject
	{
		ObjectInstance instance;

		public TypeScriptLS ()
		{
			instance = Eval<ObjectInstance> ("new Harness.TypeScriptLS ();");
			Label = AllocateVariable (instance);
		}

		public string Label { get; private set; }

		public override void Dispose ()
		{
			ReleaseVariable (Label);
		}

		#region ts public members

        public ArrayInstance Scripts { // ScriptInfo[]
			get { return (ArrayInstance) instance.GetPropertyValue ("scripts"); }
			set { instance.SetPropertyValue ("scripts", value, true); }
		}

        public int MaxScriptVersions {
			get { return (int) instance.GetPropertyValue ("maxScriptVersions"); }
			set { instance.SetPropertyValue ("maxScriptVersions", value, true); }
		}

        public void AddDefaultLibrary () 
		{
            //this.addScript("lib.d.ts", Harness.Compiler.libText, true);
        }

        public void AddFile (string name, bool isResident = false) 
		{
			instance.CallMemberFunction ("addFile", name, isResident);
        }

        public void AddScript (string name, string content, bool isResident = false)
		{
			instance.CallMemberFunction ("addScript", name, content, isResident);
        }

		public void UpdateScript (string name, string content, bool isResident = false)
		{
			instance.CallMemberFunction ("updateScript", name, content, isResident);
        }

		public void EditScript (string name, double minChar, double limChar, string newText)
		{
			instance.CallMemberFunction ("editScript", name, minChar, limChar, newText);
        }

		public string GetScriptContent (double scriptIndex)
		{
			return (string) instance.CallMemberFunction ("getScriptContent", scriptIndex);
		}

        //////////////////////////////////////////////////////////////////////
        // ILogger implementation
        //
        public bool Information () { return (bool) instance.CallMemberFunction ("information"); }
        public bool Debug () { return (bool) instance.CallMemberFunction ("debug"); }
        public bool Warning () { return (bool) instance.CallMemberFunction ("warning"); }
        public bool Error () { return (bool) instance.CallMemberFunction ("error"); }
        public bool Fatal () { return (bool) instance.CallMemberFunction ("fatal"); }

        public void Log (string s)
		{
			instance.CallMemberFunction ("log", s);
        }

        //////////////////////////////////////////////////////////////////////
        // ILanguageServiceShimHost implementation
        //

        public string GetCompilationSettings ()
		{
			return (string) instance.CallMemberFunction ("getCompilationSettings");
        }

        public double GetScriptCount ()
		{
			return (double) instance.CallMemberFunction ("getScriptCount");
        }

		public string GetScriptSourceText (double scriptIndex, double start, double end)
		{
			return (string) instance.CallMemberFunction ("getScriptSourceText", scriptIndex, start, end);
		}

		public double GetScriptSourceLength (double scriptIndex)
		{
			return (double) instance.CallMemberFunction ("getScriptSourceLength", scriptIndex);
		}

		public string GetScriptId (double scriptIndex)
		{
			return (string) instance.CallMemberFunction ("getScriptId", scriptIndex);
		}

		public bool GetScriptIsResident (double scriptIndex)
		{
			return (bool) instance.CallMemberFunction ("getScriptIsResident", scriptIndex);
		}

		public double GetScriptVersion (double scriptIndex)
		{
			return (double) instance.CallMemberFunction ("getScriptVersion", scriptIndex);
		}

		public string GetScriptEditRangeSinceVersion (double scriptIndex, double scriptVersion)
		{
			return (string) instance.CallMemberFunction ("getScriptEditRangeSinceVersion", scriptIndex, scriptVersion);
		}

        //
        // Return a new instance of the language service shim, up-to-date wrt to typecheck.
        // To access the non-shim (i.e. actual) language service, use the "ls.languageService" property.
        //
		public ObjectInstance GetLanguageService ()
		{
			return (ObjectInstance) instance.CallMemberFunction ("getLanguageService");
        }

        //
        // Parse file given its source text
        //
		public ObjectInstance ParseSourceText (string fileName, ObjectInstance sourceText)
		{
			return (ObjectInstance) instance.CallMemberFunction ("parseSourceText", fileName, sourceText);
        }

        //
        // Parse a file on disk given its filename
        //
		public ObjectInstance ParseFile (string fileName)
		{
			return (ObjectInstance) instance.CallMemberFunction ("parseFile", fileName);
        }

        //
        // line and column are 1-based
        //
		public double LineColToPosition (string fileName, double line, double col)
		{
			return (double) instance.CallMemberFunction ("lineColToPosition", fileName, line, col);
		}

        //
        // line and column are 1-based
        //
		public ObjectInstance PositionToLineCol (string fileName, double position)
		{
			return (ObjectInstance) instance.CallMemberFunction ("positionToLineCol", fileName, position);
        }

        //
        // Verify that applying edits to "sourceFileName" result in the content of the file
        // "baselineFileName"
        //
		public void CheckEdits (string sourceFileName, string baselineFileName, ArrayInstance edits)
		{
			instance.CallMemberFunction ("checkEdits", sourceFileName, baselineFileName, edits);
        }

        //
        // Apply an array of text edits to a string, and return the resulting string.
        //
		public string ApplyEdits (string content, ArrayInstance edits)
		{
			return (string) instance.CallMemberFunction ("applyEdits", content, edits);
		}
		#endregion
	}
}

