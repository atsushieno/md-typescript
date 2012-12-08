using System;
using Jurassic.Library;
using TypeScriptServiceBridge.TypeScript;

namespace TypeScriptServiceBridge
{
	public interface IClassifierHost : ILogger
	{
	}

	public class ClassifierHost_Impl : Logger_Impl, IClassifierHost
	{
		public ClassifierHost_Impl (ObjectInstance instance)
			: base (instance)
		{
		}
	}
}

