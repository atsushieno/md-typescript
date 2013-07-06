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
#if USE_JURASSIC
		[Test]
		public void TestBasics ()
		{
			TestBasicInstance (new TypeScriptLS (LanguageServiceHost.Instance.Eval<ObjectInstance> ("new Harness.TypeScriptLS ();")));
		}
#endif

		[Test]
		public void TestDefaultConstructor ()
		{
			TestBasicInstance (new TypeScriptLS ());
		}

		void TestBasicInstance (TypeScriptLS ls)
		{
			/*
			string testts1 = "alert('OK');";
			ls.AddScript ("test.ts", testts1);
			Assert.AreEqual (1, ls.Scripts.Length, "#4");
			var sshot = ls.GetScriptSnapshot ("tests.ts");
			Assert.AreEqual (testts1, sshot.GetText (0, sshot.GetLength ()), "#5");
			*/
		}
	}
}
