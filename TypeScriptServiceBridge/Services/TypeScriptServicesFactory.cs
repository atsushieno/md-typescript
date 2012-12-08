using System;
using Jurassic.Library;

namespace TypeScriptServiceBridge.Services
{
	public class TypeScriptServicesFactory : TypeScriptObject
	{
		public TypeScriptServicesFactory (ObjectInstance instance)
			: base (instance)
		{
		}

		public ILanguageService CreateLanguageService (ILanguageServiceHost host)
		{
			return new LanguageService_Impl ((ObjectInstance) Instance.CallMemberFunction ("createLanguageService", host.Instance));
		}
        public ILanguageServiceShim CreateLanguageServiceShim (ILanguageServiceShimHost host)
		{
			return new LanguageServiceShim_Impl ((ObjectInstance) Instance.CallMemberFunction ("createLanguageServiceShim", host.Instance));
		}
        public Classifier CreateClassifier (IClassifierHost host)
		{
			return new Classifier ((ObjectInstance) Instance.CallMemberFunction ("createClassifier", host.Instance));
		}
        public ClassifierShim CreateClassifierShim (IClassifierHost host)
		{
			return new ClassifierShim ((ObjectInstance) Instance.CallMemberFunction ("createClassifierShim", host.Instance));
		}
        public CoreServices CreateCoreServices (ICoreServicesHost host)
		{
			return new CoreServices ((ObjectInstance) Instance.CallMemberFunction ("createCoreServices", host.Instance));
		}
        public CoreServicesShim CreateCoreServicesShim (ICoreServicesHost host)
		{
			return new CoreServicesShim ((ObjectInstance) Instance.CallMemberFunction ("createCoreServicesShim", host.Instance));
		}
	}
}

