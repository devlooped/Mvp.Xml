using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Synchronization
{
	[Serializable]
	public class ComparableList<T> : List<T>, IEquatable<ComparableList<T>>
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
			return ComparableList<T>.Equals(this, obj as ComparableList<T>);
		}

		public bool Equals(ComparableList<T> other)
		{
			return ComparableList<T>.Equals(this, other);
		}

		public static bool Equals(ComparableList<T> obj1, ComparableList<T> obj2)
		{
			if (Object.ReferenceEquals(obj1, obj2)) return true;
			if (!Object.Equals(null, obj1) && !Object.Equals(null, obj2))
			{
				if (obj1.Count != obj2.Count) return false;
				int count = obj1.Count;
				for (int i = 0; i < count; i++)
				{
					if (Object.Equals(obj1[i], null)) return false;
					if (!obj1[i].Equals(obj2[i])) return false;
				}

				return true;
			}

			return false;
		}

		public static bool operator ==(ComparableList<T> obj1, ComparableList<T> obj2)
		{
			return ComparableList<T>.Equals(obj1, obj2);
		}

		public static bool operator !=(ComparableList<T> obj1, ComparableList<T> obj2)
		{
			return !ComparableList<T>.Equals(obj1, obj2);
		}
	}
}
