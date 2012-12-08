using System;
using Jurassic.Library;
using TypeScriptServiceBridge.Harness;

namespace TypeScriptServiceBridge.Services
{
	public class LanguageServiceShimHostAdapter : TypeScriptObject
	{
		TypeScriptLS shim; // to keep managed reference
		ObjectInstance instance;
		string label;

		public LanguageServiceShimHostAdapter (TypeScriptLS shim)
		{
			this.shim = shim;
			instance = Eval<ObjectInstance> ("new Services.LanguageServiceShimHostAdapter (" + shim.Label + ");");
			label = AllocateVariable (instance);
		}

		public override void Dispose ()
		{
			ReleaseVariable (label);
		}
	}
}

