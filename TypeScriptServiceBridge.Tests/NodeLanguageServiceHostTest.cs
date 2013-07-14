#if USE_NODE
using System;
using System.Threading;
using Jurassic.Library;
using TypeScriptServiceBridge;
using TypeScriptServiceBridge.Harness;
using TypeScriptServiceBridge.Hosting;
using TypeScriptServiceBridge.Services;
using NUnit.Framework;

namespace TypeScriptServiceBridge.Tests
{
	[TestFixture]
	public class NodeLanguageServiceHostTest
	{
		NodeLanguageServiceHost host;

		[TestFixtureSetUp]
		public void TestFixtureSetUp ()
		{
			host = new NodeLanguageServiceHost ();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown ()
		{
			host.Dispose ();
		}

		[Test]
		public void InitializeTS ()
		{
			host.Eval<string> (@"
new Services.TypeScriptServicesFactory ().createPullLanguageService (
        new Services.LanguageServiceShimHostAdapter (
                new Harness.TypeScriptLS ()))"
			                   );
		}
		
		[Test]
		public void CreateArrayInstanceFromJson ()
		{
			string json = @"
[{'name':'foo.ts','kind':'script','kindModifiers':'','matchKind':'prefix','unitIndex':0,'minChar':0,'limChar':94,'containerName':'','containerKind':''},{'name':'Foo','kind':'class','kindModifiers':'','matchKind':'exact','unitIndex':0,'minChar':0,'limChar':94,'containerName':'','containerKind':''},{'name':'foo','kind':'property','kindModifiers':'public','matchKind':'exact','unitIndex':0,'minChar':12,'limChar':32,'containerName':'Foo','containerKind':'class'}]
			";
			var arr = new TypeScriptArray<TypeScriptObject> (host.Eval (json) as ArrayInstance);
			Assert.IsNotNull (arr, "#1");
			Assert.AreEqual (3, arr.Length, "#2");
		}
		
		[Test]
		public void UseArrayInstance ()
		{
			host.Execute (@"
			shimHost = new Harness.TypeScriptLS ();
			lsHost = new Services.LanguageServiceShimHostAdapter (shimHost);
			factory = new Services.TypeScriptServicesFactory ();
			ls = factory.createPullLanguageService (lsHost);
			shimHost.addScript (""foo.ts"", ""class Foo { public foo : int = 5; public bar (baz: int) : string { return 'hello #' + baz; } }"");
			completionEntries = ls.getCompletionsAtPosition (""foo.ts"", 58, true).entries;
			");
			var searchRaw = host.Eval ("completionEntries");
			Console.WriteLine (searchRaw);
			var search = new TypeScriptArray<TypeScriptObject> (searchRaw as ArrayInstance);
			Assert.IsNotNull (search, "#3");
		}
	}
}

#endif
