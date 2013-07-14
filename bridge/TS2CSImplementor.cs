using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public class TS2CSImplementor
{
	public static void Main (string [] args)
	{
		new TS2CSImplementor ().Run (args);
	}
	
	TextWriter output = Console.Out;
	Dictionary<string, List<Type>> types = new Dictionary<string, List<Type>> ();
	
	public void Run (string [] args)
	{
		foreach (var file in args) {
			foreach (var type in Assembly.LoadFrom (file).GetTypes ()) {
				if (typeof (Attribute).IsAssignableFrom (type))
					continue; // BaseTypeAttribute or ImplementsAttribute
				List<Type> l;
				string ns = type.Namespace ?? "";
				if (!types.TryGetValue (ns, out l)) {
					l = new List<Type> ();
					types.Add (ns, l);
				}
				l.Add (type);
			}
		}
		
		output.WriteLine (@"using System;
using System.Collections.Generic;
using Jurassic;
using Jurassic.Library;
using TypeScriptServiceBridge;
");

		foreach (var ns in types.Keys)
			output.WriteLine ("using TypeScriptServiceBridge{0};", ns.Length > 0 ? "." + ns : "");

		output.WriteLine (@"

namespace TypeScriptServiceBridge {
	public class TypeScriptBridgeAttribute : Attribute
	{
		public TypeScriptBridgeAttribute (string name)
		{
			Name = name;
		}
		
		public string Name { get; set; }
	}
}
");
		
		foreach (var ns in types.Keys) {
			output.WriteLine ("namespace TypeScriptServiceBridge{0}", ns.Length > 0 ? "." + ns : "");
			output.WriteLine ("{");
			foreach (var type in types [ns]) {
				if (type.IsEnum)
					GenerateEnum (type);
				else if (type.IsInterface)
					GenerateInterface (type);
				else
					GenerateClass (type);
			}
			output.WriteLine ("}");
		}
	}
	
	const BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
	
	void GenerateEnum (Type type)
	{
		output.WriteLine ("\tpublic enum " + type.GetTypeReferenceName ());
		output.WriteLine ("\t{");
		foreach (var n in Enum.GetNames (type))
			output.WriteLine ("\t\t{0} = {1},", n, ((IConvertible) Enum.Parse (type, n)).ToInt32 (null));
		output.WriteLine ("\t}");
	}
	
	void GenerateFieldSignature (Type type, PropertyInfo p, string access)
	{
		output.WriteLine ("\t\t[TypeScriptBridge (\"{0}\")]", p.Name);
		output.Write ("\t\t{0} {1} {2}", access, GetImplementedType (p.PropertyType), ToImplementorName (type, p.Name));
	}
	
	void GenerateMethodSignature (Type type, MethodInfo m, string access)
	{
		output.WriteLine ("\t\t[TypeScriptBridge (\"{0}\")]", m.Name);
		output.Write ("\t\t{0} {1} {2}{3} ({4})", access, GetImplementedType (m.ReturnType), ToImplementorName (type, m.Name), m.GetGenericArgumentsString (), GetArgumentsSignature (m));
	}
	
	string GetArgumentsSignature (MethodBase m)
	{
		return string.Join (", ", m.GetParameters ().Select (p => GetArgumentAttributes (p) + GetImplementedType (p.ParameterType) + " " + EscapeIdentifier (p.Name) + GetArgumentOptionalInitializer (p)).ToArray ());
	}
	
	string GetArgumentAttributes (ParameterInfo p)
	{
		return string.Join ("", p.GetCustomAttributes (true).Cast<Attribute> ().Where (a => a.GetType ().GetTypeReferenceName () == "DangerousDefaultValueAttribute").Select (a => "[DangerousDefaultValueAttribute]"));
	}
	
	string GetArgumentOptionalInitializer (ParameterInfo p)
	{
		if (!p.IsOptional)
			return null;
		return " = " + GetObjectLiteral (p.RawDefaultValue);
	}
	
	string GetObjectLiteral (object o)
	{
		if (o == null)
			return "null";
		if (o.GetType ().IsEnum)
			// FIXME: how to handle flag value? cast twice?
			return o.GetType ().FullName + "." + o;
		switch (Type.GetTypeCode (o.GetType ())) {
		case TypeCode.Boolean:
			return (bool) o == true ? "true" : "false";
		case TypeCode.String:
			return "\"" + o + '"';
		}
		return o.ToString ();
	}
	
	string GetMarshaledCallArgument (Type type, string name)
	{
		/*
		if (IsObjectInstance (type))
			return EscapeIdentifier (name) + " != null ? " + EscapeIdentifier (name) + ".Instance : null";
		else
		*/
			return EscapeIdentifier (name);
	}
	
	string GetMarshaledCallArguments (MethodBase m)
	{
		return string.Join ("", m.GetParameters ().Select (p => ", " + GetMarshaledCallArgument (p.ParameterType, p.Name)).ToArray ());
	}
	
	string GetBaseTypeFromAttribute (Type type)
	{
		return type.GetCustomAttributes (false)
			.Where (a => a.GetType ().GetTypeReferenceName () == "BaseTypeAttribute")
			.Cast<Attribute> ()
			.Select (a => a.GetType ().GetProperty ("Type").GetValue (a, null))
			.Cast<string> ()
			.FirstOrDefault ();
	}
	
	IEnumerable<string> GetImplements (Type type)
	{
		return type.GetCustomAttributes (false)
			.Where (a => a.GetType ().GetTypeReferenceName () == "ImplementsAttribute")
			.Cast<Attribute> ()
			.Select (a => a.GetType ().GetProperty ("Type").GetValue (a, null))
			.Cast<string> ();
	}
	
	void GenerateImplements (Type type)
	{
		output.WriteLine ("\t\t\t" + string.Join ("", GetImplements (type).Select (s => ", " + s).ToArray ()));
	}
	
	void GenerateInterface (Type type)
	{
		output.WriteLine ("\tpublic interface {0} : ITypeScriptObject", type.GetTypeReferenceName ());
		GenerateImplements (type);
		
		output.WriteLine ("\t{");
		foreach (var p in EnumerateProperties (type)) {
			GenerateFieldSignature (type, p, null);
			output.WriteLine (" { get; set; }");
		}
		foreach (var m in EnumerateMethods (type)) {
			GenerateMethodSignature (type, m, null);
			output.WriteLine (";");
		}
		output.WriteLine ("\t}");
		
		output.WriteLine ("\tpublic class {0} : TypeScriptObject, {1}", type.GetTypeReferenceName ("_Impl"), type.GetTypeReferenceName ());
		output.WriteLine ("\t{");
		output.WriteLine ("\t\tpublic {0}_Impl (ObjectInstance instance) : base (instance) {{}}", type.GetPrimaryName ());
		
		ImplementProperties (type);
		ImplementMethods (type);
		
		output.WriteLine ("\t}");
	}
	
	void GenerateClass (Type type)
	{
		output.Write ("\tpublic class " + type.GetTypeReferenceName () + " : ");
		string baseType = GetBaseTypeFromAttribute (type);
		if (baseType == null)
			output.WriteLine ("TypeScriptObject");
		else
			output.WriteLine (baseType);
		GenerateImplements (type);

		output.WriteLine ("\t{");

		output.WriteLine ("\t\tpublic {0} (ObjectInstance instance) : base (instance) {{}}", type.GetPrimaryName ());

		foreach (var c in type.GetConstructors (flags)) {
			if (!c.IsPublic)
				continue;

			// FIXME: they are workarounds for conflict betweeen
			// ctor(Object regex) and ctor(object instance)...
			if (type.GetTypeReferenceName () == "RegexLiteral" || type.GetTypeReferenceName () == "RegularExpressionLiteralToken")
				continue;
			
			output.WriteLine ("\t\tpublic {0} ({1})", type.GetPrimaryName (), GetArgumentsSignature (c));
			var args = GetMarshaledCallArguments (c);
			output.WriteLine ("\t\t\t : base (CallConstructor (\"{0}\", \"{1}\"{2}))", type.Namespace, type.GetTypeReferenceName (), args);
			output.WriteLine ("\t\t{");
			output.WriteLine ("\t\t}");
		}
		
		ImplementProperties (type);
		ImplementMethods (type);
		
		ImplementInterfaces (type);

		output.WriteLine ("\t}");
	}
	
	static readonly Assembly corlib = typeof (int).Assembly;
	
	bool IsCallType (Type type)
	{
		return type.Assembly == corlib && (type.FullName.StartsWith ("System.Action") || type.FullName.StartsWith ("System.Func"));
	}
	
	IEnumerable<PropertyInfo> EnumerateProperties (Type type)
	{
		foreach (var p in type.GetProperties (flags)) {
			// TextEditInfo has case-conflicting properties (e.g. position and Position). Do not generate member-collision.
			if (Char.IsUpper (p.Name [0]) && type.GetProperty (Char.ToLower (p.Name [0]) + p.Name.Substring (1)) != null)
				continue;

			// It is not really doable or at least very easy to support
			// callbacks. So, ignore them so far.
			if (IsCallType (p.PropertyType))
				continue;
			
			// FIXME: in this type, there are complicated variable between object and call (Action/Func), so I disabled all members at all.
			if (type.GetTypeReferenceName () == "AstWalkerDetailCallback")
				continue;
			
			yield return p;
		}
	}
			
	void ImplementProperties (Type type)
	{
		foreach (var p in EnumerateProperties (type)) {
			bool isStatic = p.GetGetMethod ().IsStatic;
			GenerateFieldSignature (type, p, "public" + (isStatic ? " static" : ""));
			output.WriteLine (" {");
			if (IsStronglyTypedArray (p.PropertyType)) {
				if (isStatic) {
					output.WriteLine ("\t\t\tget {{ return new {0} ((ArrayInstance) GetStaticPropertyValue (typeof ({1}), \"{2}\")); }}", GetImplementedType (p.PropertyType), GetImplementedType (type), p.Name);
					output.WriteLine ("\t\t\tset {{ SetStaticPropertyValue (typeof ({0}), \"{1}\", value != null ? value.Instance : null); }}", GetImplementedType (type), p.Name);
				} else {
					output.WriteLine ("\t\t\tget {{ return new {0} ((ArrayInstance) GetPropertyValue (\"{1}\")); }}", GetImplementedType (p.PropertyType), p.Name);
					output.WriteLine ("\t\t\tset {{ SetPropertyValue (\"{0}\", value != null ? value.Instance : null); }}", p.Name);
				}
			} else if (IsObjectInstance (p.PropertyType)) {
				if (isStatic) {
					output.WriteLine ("\t\t\tget {{ return TypeScriptObject.Create<{0}> ((ObjectInstance) GetStaticPropertyValue (typeof ({1}), \"{2}\")); }}", GetImplementedType (p.PropertyType, true), GetImplementedType (type), p.Name);
					output.WriteLine ("\t\t\tset {{ SetStaticPropertyValue (typeof ({0}), \"{1}\", value != null ? value.Instance : null); }}", GetImplementedType (type), p.Name);
				} else {
					output.WriteLine ("\t\t\tget {{ return TypeScriptObject.Create<{0}> ((ObjectInstance) GetPropertyValue (\"{1}\")); }}", GetImplementedType (p.PropertyType, true), p.Name);
					output.WriteLine ("\t\t\tset {{ SetPropertyValue (\"{0}\", value != null ? value.Instance : null); }}", p.Name);
				}
			} else {
				if (isStatic) {
					output.WriteLine ("\t\t\tget {{ return TypeConverter.ConvertTo<{0}> (JurassicTypeHosting.Engine, GetStaticPropertyValue (typeof ({1}), \"{2}\")); }}", GetImplementedType (p.PropertyType), GetImplementedType (type), p.Name);
					output.WriteLine ("\t\t\tset {{ SetStaticPropertyValue (typeof ({0}), \"{1}\", value); }}", GetImplementedType (type), p.Name);
				} else {
					output.WriteLine ("\t\t\tget {{ return TypeConverter.ConvertTo<{0}> (JurassicTypeHosting.Engine, GetPropertyValue (\"{1}\")); }}", GetImplementedType (p.PropertyType), p.Name);
					output.WriteLine ("\t\t\tset {{ SetPropertyValue (\"{0}\", value); }}", p.Name);
				}
			}
			output.WriteLine ("\t\t}");
		}
	}
	
	IEnumerable<MethodInfo> EnumerateMethods (Type type)
	{
		foreach (var m in type.GetMethods (flags)) {
			// It is not really doable or at least very easy to support
			// callbacks. So, ignore them so far.
			if (IsCallType (m.ReturnType) || m.GetParameters ().Select (p => p.ParameterType).Any (t => IsCallType (t)))
				continue;
			
			// FIXME: in this type, there are complicated variable between object and call (Action/Func), so I disabled all members at all.
			if (type.GetTypeReferenceName () == "AstWalkerDetailCallback")
				continue;
			
			yield return m;
		}
	}
	
	void ImplementMethods (Type type)
	{
		foreach (var m in EnumerateMethods (type)) {
			
			// FIXME: it should generate overrides for any "optional" parameters as omitted.
			// Not all TypeScript optional parameters can be represented in C#
			// (only constants or default can be specified), so we specified
			// default(T) instead of the actual expression.
			
			GenerateMethodSignature (type, m, "public");
			output.WriteLine ("");
			output.WriteLine ("\t\t{");
			if (IsStronglyTypedArray (m.ReturnType))
				output.WriteLine (@"			var ret = ((ArrayInstance) CallMemberFunction (""{1}""{2}));
			return ret != null ? new {0} (ret) : null;", GetImplementedType (m.ReturnType), m.Name, GetMarshaledCallArguments (m));
			else if (IsObjectInstance (m.ReturnType))
				output.WriteLine ("\t\t\treturn TypeScriptObject.Create<{0}>  ((ObjectInstance) CallMemberFunction (\"{1}\"{2}));",
					GetImplementedType (m.ReturnType, true), m.Name, GetMarshaledCallArguments (m));
			else if (m.ReturnType == typeof (void))
				output.WriteLine ("\t\t\tCallMemberFunction (\"{0}\"{1});", m.Name, GetMarshaledCallArguments (m));
			else
				output.WriteLine ("\t\t\treturn TypeConverter.ConvertTo<{0}> (JurassicTypeHosting.Engine, CallMemberFunction (\"{1}\"{2}));", GetImplementedType (m.ReturnType), m.Name, GetMarshaledCallArguments (m));
			output.WriteLine ("\t\t}");
		}
	}
	
	// somewhat hacky type lookup.
	Type LookupType (Type context, string name)
	{
		Type ret = Type.GetType (name);
		if (ret != null)
			return ret;
		int idx = name.LastIndexOf ('.');
		if (idx < 0 || !types.ContainsKey (name.Substring (0, idx)))
			ret = types [context.Namespace ?? ""].FirstOrDefault (t => t.GetTypeReferenceName () == name);
		else
			ret = types [name.Substring (0, idx)].FirstOrDefault (t => t.GetTypeReferenceName () == name.Substring (idx + 1));
		if (ret == null)
			ret = types.SelectMany (p => p.Value).FirstOrDefault (t => t.FullName == name || t.FullName.EndsWith (name) && t.FullName [t.FullName.Length - name.Length - 1] == '.');
		if (ret == null) {
foreach (var t in types.SelectMany (p => p.Value)) Console.WriteLine (t.FullName + (t.FullName.EndsWith (name) ? t.FullName [t.FullName.Length - name.Length - 1] : '*'));
			throw new Exception (string.Format ("Type {0} not found in context {1}", name, context));
		}
		return ret;
	}
	
	void ImplementInterfaces (Type classType)
	{
		foreach (var iname in GetImplements (classType)) {
			var itype = LookupType (classType, iname);
			ImplementInterfaceProperties (classType, itype);
			ImplementInterfaceMethods (classType, itype);
		}
	}
	
	void ImplementInterfaceProperties (Type classType, Type type)
	{
		foreach (var p in EnumerateProperties (type)) {
			string name = ToImplementorName (type, p.Name);
			var cp = GetInheritanceChain (classType).SelectMany (t => EnumerateProperties (t)).FirstOrDefault (c => c.Name == p.Name);
			if (cp == null) {
				output.WriteLine ("// !!!!! warning !!!!! no implementation for " + p);
				continue;
			}
			output.Write ("\t\t{0} {1}.{2}", GetImplementedType (p.PropertyType), GetImplementedType (type), name);
			output.WriteLine (" {");
			output.WriteLine ("\t\t\tget {{ return ({0}) {1}; }}", GetImplementedType (p.PropertyType), name);
			output.WriteLine ("\t\t\tset {{ {0} = ({1}) value; }}", name, GetImplementedType (cp.PropertyType));
			output.WriteLine ("\t\t}");
		}
	}
	
	IEnumerable<Type> GetInheritanceChain (Type type)
	{
		yield return type;
		var btn = GetBaseTypeFromAttribute (type);
		if (btn != null) {
			var bt = LookupType (type, btn);
			if (bt != null)
				foreach (var t in GetInheritanceChain (bt))
					yield return t;
		}
	}
	
	void ImplementInterfaceMethods (Type classType, Type type)
	{
		foreach (var m in EnumerateMethods (type)) {
			var mParams = m.GetParameters ();
			var cm = GetInheritanceChain (classType).SelectMany (t => t.GetMethods ()).FirstOrDefault (c => c.Name == m.Name && c.GetParameters ().Length == mParams.Length);
			if (cm == null) {
				output.WriteLine ("// !!!!! warning !!!!! no implementation for " + m);
				continue;
			}
			var cParams = cm.GetParameters ();
			
			string name = ToImplementorName (type, m.Name);
			output.WriteLine ("\t\t{0} {1}.{2}{3} ({4})", GetImplementedType (m.ReturnType), GetImplementedType (type), name, m.GetGenericArgumentsString (), GetArgumentsSignature (m));
			output.WriteLine ("\t\t{");
			// note that here it uses class method, not interface, to bring casts.
			var args = new List<string> ();
			for (int i = 0; i < mParams.Length; i++)
				args.Add ("(" + GetImplementedType (cParams [i].ParameterType) + ") " + EscapeIdentifier (mParams [i].Name));
			bool needsExtraReturn = m.ReturnType != typeof (void) && cm.ReturnType == typeof (void);
			output.WriteLine ("\t\t\t{0} {1} ({2});",
					needsExtraReturn || m.ReturnType == typeof (void) ? "" : "return (" + GetImplementedType (m.ReturnType) + ")",
					name,
					string.Join (", ", args.ToArray ()));
			if (needsExtraReturn)
				output.WriteLine ("\t\t\treturn null; // dummy");
			output.WriteLine ("\t\t}");
		}
	}
	
	string ToImplementorName (Type container, string name)
	{
		if (name == "instance") // special case for AuthorTokenKindMap.instance.
			return "InstanceValue";
		var ret = Char.ToUpper (name [0]) + name.Substring (1);
		if (container.Name == ret)
			return "Invoke" + ret;
		else
			return ret;
	}
	
	bool IsObjectInstance (Type type)
	{
		return !type.IsEnum && GetImplementedType (type).StartsWith ("TypeScriptServiceBridge");
	}
	
	bool IsStronglyTypedArray (Type type)
	{
		return type.IsArray && IsObjectInstance (type.GetElementType ());
	}
	
	string GetImplementedType (Type type, bool addImpl = false)
	{
		if (type == typeof (void))
			return "void";
		if (type == typeof (string))
			return "string";
		if (type == typeof (bool))
			return "bool";
		if (type == typeof (double))
			return "double";
		if (type.IsArray)
			return "TypeScriptArray<" + GetImplementedType (type.GetElementType ()) + ">";
		if (type.IsGenericParameter)
			return type.Name;
		switch (type.GetTypeReferenceName ()) {
		case "any":
			return "object";
		}
		// FIXME: we cannot support callback yet. So, just return object
		if (IsCallType (type))
			return "object";

		string suffix = type.IsInterface && addImpl ? "_Impl" : string.Empty;
		
		bool isSystem = (type.Namespace ?? "").StartsWith ("System");
		if (type.IsGenericType)
			return (isSystem ? "" : "TypeScriptServiceBridge.") + 
				type.Namespace + "." + type.GetTypeReferenceName (suffix);
		
		if (!isSystem)
			return "TypeScriptServiceBridge." + type.FullName + suffix;
		return type.FullName + suffix;
	}
	
	string EscapeIdentifier (string s)
	{
		return "@" + s;
	}
}

static class Extensions
{
	public static string GetPrimaryName (this Type type)
	{
		return type.IsGenericType ? type.Name.Substring (0, type.Name.IndexOf ('`')) : type.Name;
	}
	
	public static string GetTypeReferenceName (this Type type, string suffix = null)
	{
		return type.GetPrimaryName () + suffix + (type.IsGenericType ? type.GetGenericArgumentsString () : string.Empty);
	}
	
	public static string GetGenericArgumentsString (this Type type)
	{
		return GetGenericArgumentsString (type.GenericTypeArguments);
	}
	
	public static string GetGenericArgumentsString (this MethodInfo m)
	{
		return GetGenericArgumentsString (m.GetGenericArguments ());
	}
	
	public static string GetGenericArgumentsString (Type [] types)
	{
		return types == null || types.Length == 0 ? string.Empty : '<' + string.Join (", ", types.Select (t => t.GetTypeReferenceName ()).ToArray ()) + '>';
	}
}
