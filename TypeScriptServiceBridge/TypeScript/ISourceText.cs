using System;
using Jurassic.Library;

namespace TypeScriptServiceBridge.TypeScript
{
	public interface ISourceText : ITypeScriptObject
	{
        string GetText (double start, double end);
        double GetLength ();
	}

	public class SourceText_Impl : TypeScriptObject, ISourceText
	{
		public SourceText_Impl (ObjectInstance instance)
			: base (instance)
		{
		}
		#region ISourceText implementation
		public string GetText (double start, double end)
		{
			return (string) Instance.CallMemberFunction ("getText", start, end);
		}

		public double GetLength ()
		{
			return (double) Instance.CallMemberFunction ("getLength");
		}
		#endregion
	}
}

