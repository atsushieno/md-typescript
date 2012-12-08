using System;
using Jurassic.Library;

namespace TypeScriptServiceBridge
{
	public class ClassifierShim : TypeScriptObject
	{
		public ClassifierShim (ObjectInstance instance)
			: base (instance)
		{
		}

        public Classifier Classifier {
			get { return new Classifier ((ObjectInstance) Instance.GetPropertyValue ("classifier")); }
			set { Instance.SetPropertyValue ("classifier", value.Instance, true); }
		}

		public ClassifierShim (IClassifierHost host)
			: base ((ObjectInstance) ((ObjectInstance) host.Instance.Engine.GetGlobalValue ("Services")).CallMemberFunction ("ClassifierShim", host.Instance))
		{
		}

        /// COLORIZATION
        public string GetClassificationsForLine (string text, LexState lexState)
		{
			return (string)Instance.CallMemberFunction ("getClassificationsForLine", text, lexState.Instance);
		}
	}
}

