using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Synchronization
{
	[Serializable]
	public class ComparableStack<T> : Stack<T>, IEquatable<ComparableStack<T>>
	{
		public override int GetHashCode()
		{
			int hash = 0;
			foreach (T item in this)
			{
				hash = hash ^ item.GetHashCode();
			}

			return hash;
		}

		public override bool Equals(object obj)
		{
			return ComparableStack<T>.Equals(this, obj as ComparableStack<T>);
		}

		public bool Equals(ComparableStack<T> other)
		{
			return ComparableStack<T>.Equals(this, other);
		}

		public static bool Equals(ComparableStack<T> obj1, ComparableStack<T> obj2)
		{
			if (Object.ReferenceEquals(obj1, obj2)) return true;
			if (!Object.Equals(null, obj1) && !Object.Equals(null, obj2))
			{
				if (obj1.Count != obj2.Count) return false;

				T[] first = obj1.ToArray();
				T[] second = obj2.ToArray();

				for (int i = 0; i < first.Length; i++)
				{
					if (Object.Equals(null, first[i])) return false;
					if (!first[i].Equals(second[i])) return false;
				}

				return true;
			}

			return false;
		}

		public static bool operator ==(ComparableStack<T> obj1, ComparableStack<T> obj2)
		{
			return ComparableStack<T>.Equals(obj1, obj2);
		}

		public static bool operator !=(ComparableStack<T> obj1, ComparableStack<T> obj2)
		{
			return !ComparableStack<T>.Equals(obj1, obj2);
		}
	}
}
