#if USE_JURASSIC
using System;
using Jurassic.Library;
using TypeScriptServiceBridge.Hosting;
using NUnit.Framework;

namespace TypeScriptServiceBridge.Tests
{
	[TestFixture]
	public class JurassicLanguageServiceHostTest
	{
		[Test]
		public void InitializeTS ()
		{
			JurassicLanguageServiceHost host = new JurassicLanguageServiceHost ();
			host.Eval<string> (@"
var ls = new Services.TypeScriptServicesFactory ().createLanguageService (
        new Services.LanguageServiceShimHostAdapter (
                new Harness.TypeScriptLS ()));"
			                   );
		}

		[Test]
		public void CreateArrayInstanceFromJson ()
		{
			JurassicLanguageServiceHost host = new JurassicLanguageServiceHost ();
			string json = @"var arr =
[{'name':'foo.ts','kind':'script','kindModifiers':'','matchKind':'prefix','unitIndex':0,'minChar':0,'limChar':94,'containerName':'','containerKind':''},{'name':'Foo','kind':'class','kindModifiers':'','matchKind':'exact','unitIndex':0,'minChar':0,'limChar':94,'containerName':'','containerKind':''},{'name':'foo','kind':'property','kindModifiers':'public','matchKind':'exact','unitIndex':0,'minChar':12,'limChar':32,'containerName':'Foo','containerKind':'class'}]
			";
			host.Execute (json);
			var arr = host.Eval<ArrayInstance> ("arr");
			Assert.IsNotNull (arr, "#1");
			Assert.AreEqual (3, arr.Length, "#2");
		}

		[Test]
		public void UseArrayInstance ()
		{
			JurassicLanguageServiceHost host = new JurassicLanguageServiceHost ();
			host.Execute (@"
			var shimHost = new Harness.TypeScriptLS ();
			var lsHost = new Services.LanguageServiceShimHostAdapter (shimHost);
			var factory = new Services.TypeScriptServicesFactory ();
			var ls = factory.createLanguageService (lsHost);
			shimHost.addScript (""foo.ts"", ""class Foo { public foo : int = 5; public bar (baz: int) : string { return 'hello #' + baz; } }"");");
			var ls = host.Eval<ObjectInstance> ("ls");
			host.Execute (@"
			var search = ls.getNavigateToItems (""foo"");
			");
			var search = host.Eval<ArrayInstance> ("search");
			Assert.IsNotNull (search, "#3");
			Assert.AreEqual (1, search.Length, "#4");
			Assert.AreEqual (10, search [0], "#5");
		}
	}
}

#endif
