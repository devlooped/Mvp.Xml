using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.Synchronization
{
	// TODO: review the need for this class.
	public class NullXmlItem : IXmlItem
	{
		private string id;
		private DateTime lastUpdated = DateTime.Now;

		public NullXmlItem(string id, DateTime? lastUpdated)
		{
			this.id = id;
			if (lastUpdated.HasValue)
				this.lastUpdated = lastUpdated.Value;
		}

		public string Id
		{
			get { return id; }
			set { id = value; }
		}

		public DateTime Timestamp
		{
			get { return lastUpdated; }
			set { lastUpdated = value; }
		}

		public string Title
		{
			get { return String.Empty; }
			set { }
		}

		public string Description
		{
			get { return String.Empty;  }
			set { }
		}

		public XmlElement Payload
		{
			get { return null; }
			set { }
		}

		#region ICloneable Members

		object ICloneable.Clone()
		{
			return DoClone();
		}

		public IXmlItem Clone()
		{
			return DoClone();
		}

		protected virtual IXmlItem DoClone()
		{
			return new NullXmlItem(id, lastUpdated);
		}

		#endregion

		#region Equality

		public bool Equals(IXmlItem other)
		{
			return NullXmlItem.Equals(this, other as NullXmlItem);
		}

		public override bool Equals(object obj)
		{
			return NullXmlItem.Equals(this, obj as NullXmlItem);
		}

		public static bool Equals(NullXmlItem obj1, NullXmlItem obj2)
		{
			if (Object.ReferenceEquals(obj1, obj2)) return true;
			if (!Object.Equals(null, obj1) && !Object.Equals(null, obj2))
			{
				return obj1.id == obj2.id &&
					obj1.lastUpdated == obj2.lastUpdated;
			}

			return false;
		}

		public override int GetHashCode()
		{
			int hash = id.GetHashCode();
			hash = hash ^ lastUpdated.GetHashCode();

			return hash;
		}

		#endregion
	}
}
