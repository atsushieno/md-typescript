#if USE_NODE
using System;
using System.Threading;
using Jurassic.Library;
using TypeScriptServiceBridge.Harness;
using TypeScriptServiceBridge.Hosting;
using TypeScriptServiceBridge.Services;
using NUnit.Framework;

namespace TypeScriptServiceBridge.Tests
{
	[TestFixture]
	public class NodeLanguageServiceHostTest
	{
		LanguageServiceHost host;

		[TestFixtureSetUp]
		public void TestFixtureSetUp ()
		{
			host = LanguageServiceHost.Instance; //new NodeLanguageServiceHost ();
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
new Services.TypeScriptServicesFactory ().createLanguageService (
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
			var arr = host.Eval<ArrayInstance> (json);
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
			ls = factory.createLanguageService (lsHost);
			shimHost.addScript (""foo.ts"", ""class Foo { public foo : int = 5; public bar (baz: int) : string { return 'hello #' + baz; } }"");
			search = ls.getNavigateToItems (""foo"");
			");
			var searchRaw = host.Eval ("search");
			Console.WriteLine (searchRaw);
			var search = Jurassic.TypeConverter.ConvertTo<ArrayInstance> (JurassicTypeHosting.Engine, searchRaw);
			Assert.IsNotNull (search, "#3");
			Assert.AreEqual (3, search.Length, "#4");
			Assert.AreEqual (94, ((ObjectInstance) search [0]) ["limChar"], "#5");
		}
	}
}

#endif
