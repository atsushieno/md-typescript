using System;
using NUnit.Framework;
using TypeScriptServiceBridge.Harness;

namespace TypeScriptServiceBridge.Tests
{
	[TestFixture]
	public class TypeScriptLSTest
	{
		[Test]
		public void TestBasics ()
		{
			var ls = new TypeScriptLS ();
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

