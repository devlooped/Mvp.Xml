using System;
using System.Xml;

namespace Mvp.Xml.Synchronization
{
	[Serializable]
	public class XmlItem : IXmlItem
	{
		private string id;
		private string description;
		private string title;
		private DateTime lastUpdated = DateTime.Now;
		private XmlElement payload;

		public XmlItem(string title, string description, XmlElement payload)
			: this(Guid.NewGuid().ToString(), title, description, DateTime.Now, payload)
		{
		}

		public XmlItem(string title, string description, DateTime timestamp, XmlElement payload)
			: this(Guid.NewGuid().ToString(), title, description, timestamp, payload)
		{
		}

		public XmlItem(string id, string title, string description, DateTime timestamp, XmlElement payload)
		{
			Guard.ArgumentNotNullOrEmptyString(id, "id");
			Guard.ArgumentNotNullOrEmptyString(title, "title");
			Guard.ArgumentNotNullOrEmptyString(description, "description");
			Guard.ArgumentNotNull(payload, "payload");

			this.id = id;
			this.title = title;
			this.description = description;
			this.lastUpdated = timestamp;
			this.payload = payload;
		}

		public string Id
		{
			get { return id; }
			set { Guard.ArgumentNotNullOrEmptyString(value, "Id"); id = value; }
		}

		public string Title
		{
			get { return title; }
			set { Guard.ArgumentNotNullOrEmptyString(value, "Title"); title = value; }
		}

		public string Description
		{
			get { return description; }
			set { Guard.ArgumentNotNullOrEmptyString(value, "Description"); description = value; }
		}

		public DateTime Timestamp
		{
			get { return lastUpdated; }
			set { lastUpdated = value; }
		}

		public XmlElement Payload
		{
			get { return payload; }
			set { Guard.ArgumentNotNull(value, "Payload"); payload = value; }
		}

		#region Equality

		public bool Equals(IXmlItem other)
		{
			return XmlItem.Equals(this, other);
		}

		public override bool Equals(object obj)
		{
			return XmlItem.Equals(this, obj as IXmlItem);
		}

		public static bool Equals(IXmlItem obj1, IXmlItem obj2)
		{
			if (Object.ReferenceEquals(obj1, obj2)) return true;
			if (!Object.Equals(null, obj1) && !Object.Equals(null, obj2))
			{
				return obj1.Id == obj2.Id &&
					obj1.Title == obj2.Title &&
					obj1.Description == obj2.Description &&
					obj1.Timestamp == obj2.Timestamp &&
					obj1.Payload.OuterXml == obj2.Payload.OuterXml;
			}

			return false;
		}

		public override int GetHashCode()
		{
			int hash = id.GetHashCode();
			hash = hash ^ title.GetHashCode();
			hash = hash ^ description.GetHashCode();
			hash = hash ^ lastUpdated.GetHashCode();
			hash = hash ^ payload.OuterXml.GetHashCode();

			return hash;
		}

		#endregion

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
			return new XmlItem(id, title, description, lastUpdated, payload);
		}

		#endregion
	}
}
