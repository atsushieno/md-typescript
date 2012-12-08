using System;
using Jurassic.Library;
using TypeScriptServiceBridge.TypeScript;

namespace TypeScriptServiceBridge.Services
{
	public class CoreServicesShim : TypeScriptObject
	{
		public CoreServicesShim (ObjectInstance instance)
			: base (instance)
		{
		}

        public ILogger Logger {
			get { return new Logger_Impl ((ObjectInstance) Instance.GetPropertyValue ("logger")); }
			set { Instance.SetPropertyValue ("logger", value.Instance, true); }
		}

		public CoreServicesShim (ICoreServicesHost host)
			: base ((ObjectInstance) host.Instance.Engine.CallGlobalFunction ("CoreServicesShim", host.Instance))
		{
		}

        ///
        /// getPreProcessedFileInfo
        ///
        public string GetPreProcessedFileInfo (string scriptId, ISourceText sourceText)
		{
			return (string) Instance.CallMemberFunction ("getPreProcessedFileInfo", scriptId, sourceText);
        }

        ///
        /// getDefaultCompilationSettings
        ///
		public string GetDefaultCompilationSettings ()
		{
			return (string) Instance.CallMemberFunction ("getDefaultCompilationSettings");
        }

        ///
        /// dumpMemory
        ///
        public string dumpMemory (object dummy)
		{
			return (string) Instance.CallMemberFunction ("dumpMemory", TypeScriptObject.ToNative (dummy));
        }

        ///
        /// getMemoryInfo
        ///
        public string GetMemoryInfo (object dummy)
		{
			return (string) Instance.CallMemberFunction ("getMemoryInfo", TypeScriptObject.ToNative (dummy));
		}
	}
}

