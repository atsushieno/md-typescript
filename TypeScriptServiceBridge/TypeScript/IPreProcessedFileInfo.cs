using System;
using Jurassic.Library;

namespace TypeScriptServiceBridge.TypeScript
{
	public interface IPreProcessedFileInfo : ITypeScriptObject
	{
		CompilationSettings Settings { get; set; }
		ArrayInstance ReferencedFiles { get; set; }
		ArrayInstance ImportedFiles { get; set; }
		bool IsLibFile { get; set; }
	}

	public class PreProcessedFileInfo_Impl : TypeScriptObject, IPreProcessedFileInfo
	{
		public PreProcessedFileInfo_Impl (ObjectInstance instance)
			: base (instance)
		{
		}
		#region IPreProcessedFileInfo implementation
		public CompilationSettings Settings {
			get { return new CompilationSettings ((ObjectInstance) Instance.GetPropertyValue ("compilationSettings")); }
			set { Instance.SetPropertyValue ("compilationSettings", value.Instance, true); }
		}

		public ArrayInstance ReferencedFiles {
			get { return (ArrayInstance) Instance.GetPropertyValue ("referencedFiles"); }
			set { Instance.SetPropertyValue ("referencedFiles", value, true); }
		}

		public ArrayInstance ImportedFiles {
			get { return (ArrayInstance) Instance.GetPropertyValue ("importedFiles"); }
			set { Instance.SetPropertyValue ("importedFiles", value, true); }
		}

		public bool IsLibFile {
			get { return (bool) Instance.GetPropertyValue ("isLibFile"); }
			set { Instance.SetPropertyValue ("isLibFile", value, true); }
		}
		#endregion

	}
}

