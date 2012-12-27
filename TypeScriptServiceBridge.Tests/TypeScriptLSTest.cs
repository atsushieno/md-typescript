#if USE_JURASSIC
using System;
using Jurassic.Library;
using NUnit.Framework;
using TypeScriptServiceBridge.Hosting;
using TypeScriptServiceBridge.Harness;

namespace TypeScriptServiceBridge.Tests
{
	[TestFixture]
	public class TypeScriptLSTest
	{
		[Test]
		public void TestBasics ()
		{
			TestBasicInstance (new TypeScriptLS (LanguageServiceHost.Instance.Eval<ObjectInstance> ("new Harness.TypeScriptLS ();")));
		}

		[Test]
		public void TestDefaultConstructor ()
		{
			TestBasicInstance (new TypeScriptLS ());
		}

		void TestBasicInstance (TypeScriptLS ls)
		{
			Assert.AreEqual (100, ls.MaxScriptVersions, "#1");
			Assert.IsNotNull (ls.Scripts, "#2");
			Assert.AreEqual (0, ls.Scripts.Length, "#3");
			string testts1 = "alert('OK');";
			ls.AddScript ("test.ts", testts1);
			Assert.AreEqual (1, ls.Scripts.Length, "#4");
			Assert.AreEqual (testts1, ls.GetScriptContent (0), "#5");
		}
	}
}

#endif
