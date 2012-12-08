using System;
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
	}
}

