using System;
using Jurassic.Library;
using TypeScriptServiceBridge.Hosting;
using NUnit.Framework;

namespace TypeScriptServiceBridge.Tests
{
	[TestFixture]
	public class JurassicLanguageServiceHostTest
	{
		JurassicLanguageServiceHost host = new JurassicLanguageServiceHost ();

		[Test]
		public void InitializeTS ()
		{
			host.Eval<string> (@"
var ls = new Services.TypeScriptServicesFactory ().createLanguageService (
        new Services.LanguageServiceShimHostAdapter (
                new Harness.TypeScriptLS ()));"
			                   );
		}

		[Test]
		public void UseArrayInstance ()
		{
			var search = host.Eval<ArrayInstance> (@"
			var shimHost = new Harness.TypeScriptLS ();
			var lsHost = new Services.LanguageServiceShimHostAdapter (shimHost);
			var factory = new Services.TypeScriptServicesFactory ();
			var ls = factory.createLanguageService (lsHost);
			shimHost.addScript (""foo.ts"", ""class Foo { public foo : int = 5; public bar (baz: int) : string { return 'hello #' + baz; } }"");
			var search = ls.getNavigateToItems (""foo"");
			");
			Assert.IsNotNull (search, "#3");
			Assert.AreEqual (1, search.Length, "#4");
			Assert.AreEqual (10, search [0], "#5");
		}
	}
}

