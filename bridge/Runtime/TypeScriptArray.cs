using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jurassic;
using Jurassic.Library;
using TypeScriptServiceBridge.Hosting;

namespace TypeScriptServiceBridge
{
	public class TypeScriptArray<T> : TypeScriptObject, ICollection<T>
	{
		public static TypeScriptArray<T> Create (object obj)
		{
			return obj == null || obj is Null ? null : new TypeScriptArray<T> ((ObjectInstance) obj);
		}

		public TypeScriptArray (ObjectInstance instance)
			: base (instance)
		{
		}

		public int Length {
			get { return ConvertTo<int> (GetPropertyValue ("length")); }
		}

		public T this [int index] {
			get { return ConvertTo<T> (LanguageServiceHost.Instance.GetArrayItem (this, index)); }
			set { LanguageServiceHost.Instance.SetArrayItem (this, index, ToNative (value)); }
		}

		#region ICollection implementation

		int ICollection<T>.Count {
			get { return (int) Length; }
		}

		bool ICollection<T>.IsReadOnly {
			get { return false; }
		}

		void ICollection<T>.Add (T item)
		{
			throw new NotImplementedException ();
			//ArrayInstance.Concat (Instance, item);
		}

		void ICollection<T>.Clear ()
		{
			throw new NotImplementedException ();
			//ArrayInstance.Splice (Instance, 0, (int) array.Length);
		}

		public bool Contains (T item)
		{
			return this.Any (v => v.Equals (item));
		}

		public void CopyTo (T [] array, int arrayIndex)
		{
			foreach (var t in array.Select (item => ConvertTo<T> (item)))
				array [arrayIndex++] = t;
		}

		bool ICollection<T>.Remove (T item)
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<T> GetEnumerator ()
		{
			for (int i = 0; i < Length; i++)
				yield return ConvertTo<T> (this [i]);
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion
	}
}

