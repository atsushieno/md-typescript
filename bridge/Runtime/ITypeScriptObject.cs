using System;
using Jurassic.Library;

namespace TypeScriptServiceBridge
{
	public interface ITypeScriptObject : IDisposable
	{
		ObjectInstance Instance { get; set; }
	}
}

