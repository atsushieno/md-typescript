using System;
using Jurassic.Library;
using TypeScriptServiceBridge.TypeScript;

namespace TypeScriptServiceBridge.Services
{
	public class CoreServices : TypeScriptObject
	{
		public CoreServices (ObjectInstance instance)
			: base (instance)
		{
		}

		public CoreServices (ICoreServicesHost host)
			: base (CallConstructor ("Services", "CoreServices", host.Instance))
		{
		}

        public IPreProcessedFileInfo GetPreProcessedFileInfo (string scriptId, ISourceText sourceText)
		{
			return new PreProcessedFileInfo_Impl ((ObjectInstance) Instance.CallMemberFunction ("getPreProcessedFileInfo", scriptId, sourceText));
		}

        public CompilationSettings GetDefaultCompilationSettings ()
		{
			return new CompilationSettings ((ObjectInstance) Instance.CallMemberFunction ("getDefaultCompilationSettings"));
        }

        public string DumpMemory ()
		{
			return (string) Instance.CallMemberFunction ("dumpMemory");
		}

		public ArrayInstance GetMemoryInfo ()
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getMemoryInfo");
		}

		public void CollectGarbage ()
		{
			Instance.CallMemberFunction ("collectGarbage");
		}
	}
}

