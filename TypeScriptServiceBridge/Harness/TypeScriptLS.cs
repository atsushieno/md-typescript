using System;
using Jurassic.Library;
using TypeScriptServiceBridge.Hosting;
using TypeScriptServiceBridge.Services;

namespace TypeScriptServiceBridge.Harness
{
	public class TypeScriptLS : TypeScriptObject, ILanguageServiceShimHost
	{
		public TypeScriptLS (ObjectInstance instance)
			: base (instance)
		{
		}

		#region ts public members

        public ArrayInstance Scripts { // ScriptInfo[]
			get { return (ArrayInstance) Instance.GetPropertyValue ("scripts"); }
			set { Instance.SetPropertyValue ("scripts", value, true); }
		}

        public int MaxScriptVersions {
			get { return (int) Instance.GetPropertyValue ("maxScriptVersions"); }
			set { Instance.SetPropertyValue ("maxScriptVersions", value, true); }
		}

        public void AddDefaultLibrary () 
		{
			Instance.CallMemberFunction ("addDefaultLibrary");
        }

        public void AddFile (string name, bool isResident = false) 
		{
			Instance.CallMemberFunction ("addFile", name, isResident);
        }

        public void AddScript (string name, string content, bool isResident = false)
		{
			Instance.CallMemberFunction ("addScript", name, content, isResident);
        }

		public void UpdateScript (string name, string content, bool isResident = false)
		{
			Instance.CallMemberFunction ("updateScript", name, content, isResident);
        }

		public void EditScript (string name, double minChar, double limChar, string newText)
		{
			Instance.CallMemberFunction ("editScript", name, minChar, limChar, newText);
        }

		public string GetScriptContent (double scriptIndex)
		{
			return (string) Instance.CallMemberFunction ("getScriptContent", scriptIndex);
		}

        //////////////////////////////////////////////////////////////////////
        // ILogger implementation
        //
        public bool Information () { return (bool) Instance.CallMemberFunction ("information"); }
        public bool Debug () { return (bool) Instance.CallMemberFunction ("debug"); }
        public bool Warning () { return (bool) Instance.CallMemberFunction ("warning"); }
        public bool Error () { return (bool) Instance.CallMemberFunction ("error"); }
        public bool Fatal () { return (bool) Instance.CallMemberFunction ("fatal"); }

        public void Log (string s)
		{
			Instance.CallMemberFunction ("log", s);
        }

        //////////////////////////////////////////////////////////////////////
        // ILanguageServiceShimHost implementation
        //

        public string GetCompilationSettings ()
		{
			return (string) Instance.CallMemberFunction ("getCompilationSettings");
        }

        public double GetScriptCount ()
		{
			return (double) Instance.CallMemberFunction ("getScriptCount");
        }

		public string GetScriptSourceText (double scriptIndex, double start, double end)
		{
			return (string) Instance.CallMemberFunction ("getScriptSourceText", scriptIndex, start, end);
		}

		public double GetScriptSourceLength (double scriptIndex)
		{
			return (double) Instance.CallMemberFunction ("getScriptSourceLength", scriptIndex);
		}

		public string GetScriptId (double scriptIndex)
		{
			return (string) Instance.CallMemberFunction ("getScriptId", scriptIndex);
		}

		public bool GetScriptIsResident (double scriptIndex)
		{
			return (bool) Instance.CallMemberFunction ("getScriptIsResident", scriptIndex);
		}

		public double GetScriptVersion (double scriptIndex)
		{
			return (double) Instance.CallMemberFunction ("getScriptVersion", scriptIndex);
		}

		public string GetScriptEditRangeSinceVersion (double scriptIndex, double scriptVersion)
		{
			return (string) Instance.CallMemberFunction ("getScriptEditRangeSinceVersion", scriptIndex, scriptVersion);
		}

        //
        // Return a new Instance of the language service shim, up-to-date wrt to typecheck.
        // To access the non-shim (i.e. actual) language service, use the "ls.languageService" property.
        //
		public ILanguageServiceShim GetLanguageService ()
		{
			return new LanguageServiceShim_Impl ((ObjectInstance) Instance.CallMemberFunction ("getLanguageService"));
        }

        //
        // Parse file given its source text
        //
		public ObjectInstance ParseSourceText (string fileName, ObjectInstance sourceText)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("parseSourceText", fileName, sourceText);
        }

        //
        // Parse a file on disk given its filename
        //
		public ObjectInstance ParseFile (string fileName)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("parseFile", fileName);
        }

        //
        // line and column are 1-based
        //
		public double LineColToPosition (string fileName, double line, double col)
		{
			return (double) Instance.CallMemberFunction ("lineColToPosition", fileName, line, col);
		}

        //
        // line and column are 1-based
        //
		public ObjectInstance PositionToLineCol (string fileName, double position)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("positionToLineCol", fileName, position);
        }

        //
        // Verify that applying edits to "sourceFileName" result in the content of the file
        // "baselineFileName"
        //
		public void CheckEdits (string sourceFileName, string baselineFileName, ArrayInstance edits)
		{
			Instance.CallMemberFunction ("checkEdits", sourceFileName, baselineFileName, edits);
        }

        //
        // Apply an array of text edits to a string, and return the resulting string.
        //
		public string ApplyEdits (string content, ArrayInstance edits)
		{
			return (string) Instance.CallMemberFunction ("applyEdits", content, edits);
		}
		#endregion
	}
}

