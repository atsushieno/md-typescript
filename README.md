md-typescript is an attempt to build TypeScript language binding for
MonoDevelop.

So far it is based on master monodevelop.
Not sure if it will be based on NRefactory (TS is not .NET anyways).

Features so far:

	- It runs tsc to generate javascript output from set of ts sources.
	- It provides simple syntax highlighting.
	- It gives some code completion for members.
	- It provides tooltip for the language item at cursor.

It uses console node.js server to interoperate with typescript language service to retrieve code completion
information and so on.

It used to be based on md-haxebinding:
https://github.com/jgranick/md-haxebinding
Now it's rather based on other bindings such as CSharpBinding).
