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

		[Test]
		public void SimpleTest ()
		{
			var shimHost = new TypeScriptLS ();
			var lsHost = new LanguageServiceShimHostAdapter (shimHost);
			var ls = new TypeScriptServicesFactory ().CreateLanguageService (lsHost);
			//Assert.AreEqual (lsHost.Instance, ls.Host.Instance, "#1");
			shimHost.AddScript ("foo.ts", "class Foo { public foo : int = 5; public bar (baz: int) : string { return 'hello #' + baz; } }");
			var search = ls.GetNavigateToItems ("foo");
			Assert.IsNotNull (search, "#3");
			Assert.AreEqual (3, search.Length, "#4");
			Assert.AreEqual (94, search [0].LimChar, "#5");
		}
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
