// 
// TypeScriptReferenceFinder.cs
//  
// Author:
//	Atsushi Enomoto <atsushieno@veritas-vos-liberabit.com>
// 
// Copyright (c) 2013 Xamarin Inc. (http://xamarin.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;

using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.FindInFiles;
using MonoDevelop.Projects;
using ICSharpCode.NRefactory.TypeSystem;
using System.IO;
using Mono.TextEditor;
using MonoDevelop.TypeScriptBinding.Projects;

namespace MonoDevelop.TypeScriptBinding.Languages
{
	public class TypeScriptReferenceFinder : ReferenceFinder
	{
		public TypeScriptReferenceFinder ()
		{
		}
		
		TypeScriptService GetService (Project project)
		{
			var tp = project as TypeScriptProject;
			return tp != null ? tp.TypeScriptService : null;
		}

		#region implemented abstract members of ReferenceFinder

		public override IEnumerable<MemberReference> FindReferences (Project project, IProjectContent content, IEnumerable<FilePath> files, IEnumerable<object> searchedMembers)
		{
			// This "searchedMembers" depends on NRefactory TypeSystem.
			throw new NotImplementedException ();
		}

		#endregion
	}
}
