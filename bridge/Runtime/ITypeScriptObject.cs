using System;
using Jurassic.Library;

namespace TypeScriptServiceBridge
{
	public interface ITypeScriptObject : IDisposable
	{
		ITypeScriptObject CreateLocalCache ();
		T CreateLocalCache<T> () where T : ITypeScriptObject;
		ObjectInstance Instance { get; set; }
	}
}

