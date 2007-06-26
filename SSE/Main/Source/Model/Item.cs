using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace Mvp.Xml.Synchronization
{
	[Serializable]
	public class Item : ICloneable<Item>
	{
		private IXmlItem xmlItem;
		private Sync sync;

		public Item(IXmlItem xmlItem, Sync sync)
		{
			Guard.ArgumentNotNull(sync, "sync");

			this.xmlItem = xmlItem;
			this.sync = sync;
		}

		public Sync Sync
		{
			get { return sync; }
		}

		public IXmlItem XmlItem
		{
			get { return this.xmlItem; }
		}

		// TODO: test this method!
		public bool IsSubsumedBy(Item item)
		{
			History Hx = this.sync.LastUpdate;
			foreach (History Hy in item.sync.UpdatesHistory)
			{
				if (Hx.IsSubsumedBy(Hy))
				{
					return true;
				}
			}
			return false;
		}

		#region Equality

		public static bool operator ==(Item h1, Item h2)
		{
			return Item.Equals(h1, h2);
		}

		public static bool operator !=(Item h1, Item h2)
		{
			return !Item.Equals(h1, h2);
		}

		public bool Equals(Item other)
		{
			return Item.Equals(this, other);
		}

		public override bool Equals(object obj)
		{
			return Item.Equals(this, obj as Item);
		}

		public static bool Equals(Item obj1, Item obj2)
		{
			if (Object.ReferenceEquals(obj1, obj2)) return true;
			if (!Object.Equals(null, obj1) && !Object.Equals(null, obj2))
			{
				// If both xmlitems are null, we check for sync equality.
				if (Object.Equals(null, obj1.xmlItem) &&
					Object.Equals(null, obj2.xmlItem))
				{
					return obj1.sync.Equals(obj2.sync);
				}
				else if (!(Object.Equals(null, obj1.xmlItem) ||
					Object.Equals(null, obj2.xmlItem)))
				{
					return obj1.xmlItem.Equals(obj2.xmlItem) &&
						obj1.sync.Equals(obj2.sync);
				}
			}

			return false;
		}

		public override int GetHashCode()
		{
			int hash = ((xmlItem == null) ? 0 : xmlItem.GetHashCode());
			hash = ((sync == null) ? hash : hash ^ sync.GetHashCode());

			return hash;
		}

		#endregion

		#region ICloneable Members

		object ICloneable.Clone()
		{
			return DoClone();
		}

		public Item Clone()
		{
			return (Item)DoClone();
		}

		private object DoClone()
		{
			IXmlItem cloneXml = null;
			Sync cloneSync = null;
			if (xmlItem != null) cloneXml = xmlItem.Clone();
			if (sync != null) cloneSync = sync.Clone();

			return new Item(cloneXml, cloneSync);
		}

		#endregion
	}
}
