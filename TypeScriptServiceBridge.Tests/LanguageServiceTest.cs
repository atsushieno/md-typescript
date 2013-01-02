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
			//Assert.AreEqual (lsHost.Instance, ls.Host.Instance, "#1");
			shimHost.AddScript ("foo.ts", "class Foo { public foo : int = 5; public bar (baz: int) : string { return 'hello #' + baz; } }");
			
			// search foo -> matches both class Foo and field foo.
			var search = ls.GetNavigateToItems ("foo");
			Assert.IsNotNull (search, "#3");
			Assert.AreEqual (3, search.Length, "#4");
			Assert.AreEqual (0, search [0].MinChar, "#5.1"); // matched prefix
			Assert.AreEqual (94, search [0].LimChar, "#5.2");
			Assert.AreEqual ("prefix", search [0].MatchKind, "#5.3");
			Assert.AreEqual (0, search [1].MinChar, "#6.1"); // matched class Foo (case insensitive)
			Assert.AreEqual (94, search [1].LimChar, "#6.2");
			Assert.AreEqual ("exact", search [1].MatchKind, "#6.3");
			Assert.AreEqual (12, search [2].MinChar, "#7.1"); // matched public foo
			Assert.AreEqual (32, search [2].LimChar, "#7.2");
			Assert.AreEqual ("exact", search [2].MatchKind, "#7.3");
			search.Dispose ();
			
			// search bar -> matches function bar.
			search = ls.GetNavigateToItems ("bar");
			Assert.IsNotNull (search, "#8");
			Assert.AreEqual (1, search.Length, "#9");
			Assert.AreEqual (34, search [0].MinChar, "#10.1"); // matched public foo
			Assert.AreEqual (92, search [0].LimChar, "#10.2");
			Assert.AreEqual ("exact", search [0].MatchKind, "#10.3");
			
			// search baz -> no hit (argument name is out of scope)
			search = ls.GetNavigateToItems ("baz");
			Assert.IsNotNull (search, "#11");
			Assert.AreEqual (0, search.Length, "#12");
		}

		[Test]
		[ExpectedException (typeof (JavaScriptException))]
		public void AttemptToAccessNonExistentResource ()
		{
			var shimHost = new TypeScriptLS ();
			var lsHost = new LanguageServiceShimHostAdapter (shimHost);
			var ls = new TypeScriptServicesFactory ().CreateLanguageService (lsHost);
			//Assert.AreEqual (lsHost.Instance, ls.Host.Instance, "#1");
			// attempt to get non-existent definition
			Assert.IsNull (ls.GetDefinitionAtPosition ("nonexistent", 0), "#2");
		}

		[Test]
		public void GetCompletionsAtPosition ()
		{
			string script = @"
class Greeter {
	greeting: string;
	constructor (message: string) {
		this.greeting = message;
	}
	greet() {
		return 'Hello, ' + this.greeting;
	}
}   

var greeter = new Greeter('world');

var button = document.createElement('button')
button.innerText = 'Say Hello'
button.onclick = function() {
	alert(greeter.greet())
}

document.body.appendChild(button)
";
			var shimHost = new TypeScriptLS ();
			var lsHost = new LanguageServiceShimHostAdapter (shimHost);
			var ls = new TypeScriptServicesFactory ().CreateLanguageService (lsHost);
			shimHost.AddScript ("foo.ts", script);
			for (int i = 72; i < 77; i++) { // his.greeting at line 5
				var list = ls.GetCompletionsAtPosition ("foo.ts", i, true);
				Assert.AreEqual (2, list.Entries.Length, "#1." + i);
			}
			/*
			for (int i = 77; i < 132; i++) {
				var list = ls.GetCompletionsAtPosition ("foo.ts", i, true);
				Assert.AreEqual (0, list.Entries.Length, "#2." + i);
			}
			*/
			for (int i = 132; i < 137; i++) { // his.greeting at line 8
				var list = ls.GetCompletionsAtPosition ("foo.ts", i, true);
				Assert.AreEqual (2, list.Entries.Length, "#3." + i);
			}
			/*
			for (int i = 137; i < 307; i++) {
				var list = ls.GetCompletionsAtPosition ("foo.ts", i, true);
				Assert.AreEqual (0, list.Entries.Length, "#4." + i);
			}
			*/
			for (int i = 307; i < 315; i++) { // reeter.greet() at line 17
				var list = ls.GetCompletionsAtPosition ("foo.ts", i, true);
				Assert.AreEqual (2, list.Entries.Length, "#5." + i);
			}
			for (int i = 315; i < script.Length; i++) {
				var list = ls.GetCompletionsAtPosition ("foo.ts", i, true);
				Assert.AreEqual (0, list.Entries.Length, "#6 " + i + " : " + script.Substring (i));
			}
		}
	}
}
