#if USE_JURASSIC
using System;
using Jurassic;
using TypeScriptServiceBridge.Harness;
using TypeScriptServiceBridge.Services;
using NUnit.Framework;

namespace TypeScriptServiceBridge.Tests
{
	[TestFixture]
	public class LanguageServiceTest
	{
		[Test]
		public void SimpleTest ()
		{
			var shimHost = new TypeScriptLS ();
			var lsHost = new LanguageServiceShimHostAdapter (shimHost);
			var ls = new TypeScriptServicesFactory ().CreateLanguageService (lsHost);
			Assert.AreEqual (lsHost.Instance, ls.Host.Instance, "#1");
			shimHost.AddScript ("foo.ts", "class Foo { public foo : int = 5; public bar (baz: int) : string { return 'hello #' + baz; } }");
			var search = ls.GetNavigateToItems ("foo");
			Assert.IsNotNull (search, "#3");
			Assert.AreEqual (1, search.Length, "#4");
			Assert.AreEqual (10, search [0], "#5");
		}

		[Test]
		[ExpectedException (typeof (JavaScriptException))]
		public void AttemptToAccessNonExistentResource ()
		{
			var shimHost = new TypeScriptLS ();
			var lsHost = new LanguageServiceShimHostAdapter (shimHost);
			var ls = new TypeScriptServicesFactory ().CreateLanguageService (lsHost);
			Assert.AreEqual (lsHost.Instance, ls.Host.Instance, "#1");
			// attempt to get non-existent definition
			Assert.IsNull (ls.GetDefinitionAtPosition ("nonexistent", 0), "#2");
		}
	}
}

#endif

