using System;
using Jurassic.Library;
using TypeScriptServiceBridge.TypeScript;

namespace TypeScriptServiceBridge.Services
{
	public interface ICoreServicesHost : ITypeScriptObject
	{
		ILogger Logger { get; set; }
	}

	public class CoreServicesHost_Impl : TypeScriptObject, ICoreServicesHost
	{
		public CoreServicesHost_Impl (ObjectInstance instance)
			: base (instance)
		{
		}

		public ILogger Logger {
			get { return new Logger_Impl ((ObjectInstance)Instance.GetPropertyValue ("logger")); }
			set { Instance.SetPropertyValue ("logger", value.Instance, true); }
		}
	}
}

