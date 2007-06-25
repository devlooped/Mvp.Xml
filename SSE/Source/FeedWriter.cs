using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.Synchronization
{
	public abstract class FeedWriter
	{
		XmlWriter writer;
		public event EventHandler ItemWritten;

		public FeedWriter(XmlWriter writer)
		{
			Guard.ArgumentNotNull(writer, "writer");

			this.writer = new XmlSharingWriter(writer);
		}

		public void Write(Feed feed, IEnumerable<Item> items)
		{
			// write feed root element: rss | atom
			WriteFeed(feed, writer);
			WriteSharing(feed.Sharing);

			foreach (Item item in items)
			{
				Write(item);
				if (ItemWritten != null)
					ItemWritten(this, EventArgs.Empty);
			}

			// close feed root
			writer.WriteEndElement();
		}

		internal void Write(Item item)
		{
			// <item>
			WriteItem(item, writer);
			Write(item.Sync);
			// </item>
			writer.WriteEndElement();
		}

		internal void Write(Sync sync)
		{
			// <sx:sync>
			writer.WriteStartElement(Schema.DefaultPrefix, Schema.ElementNames.Sync, Schema.Namespace);
			writer.WriteAttributeString(Schema.AttributeNames.Id, sync.Id);
			writer.WriteAttributeString(Schema.AttributeNames.Updates, XmlConvert.ToString(sync.Updates));
			writer.WriteAttributeString(Schema.AttributeNames.Deleted, XmlConvert.ToString(sync.Deleted));
			writer.WriteAttributeString(Schema.AttributeNames.NoConflicts, XmlConvert.ToString(sync.NoConflicts));

			WriteHistory(sync.UpdatesHistory);

			if (sync.Conflicts.Count > 0)
			{
				// <sx:conflicts>
				writer.WriteStartElement(Schema.DefaultPrefix, Schema.ElementNames.Conflicts, Schema.Namespace);

				foreach (Item conflict in sync.Conflicts)
				{
					Write(conflict);
				}

				// </sx:conflicts>
				writer.WriteEndElement();
			}

			// </sx:sync>
			writer.WriteEndElement();
		}

		private void WriteSharing(Sharing sharing)
		{
			// <sx:sharing>
			writer.WriteStartElement(Schema.DefaultPrefix, Schema.ElementNames.Sharing, Schema.Namespace);
			if (sharing.Since != null)
				writer.WriteAttributeString(Schema.AttributeNames.Since, sharing.Since);
			if (sharing.Until != null)
				writer.WriteAttributeString(Schema.AttributeNames.Until, sharing.Until);
			if (sharing.Expires != null)
				writer.WriteAttributeString(Schema.AttributeNames.Expires, Timestamp.ToString(sharing.Expires.Value));

			WriteRelated(sharing.Related);

			// </sx:sharing>
			writer.WriteEndElement();
		}

		private void WriteRelated(List<Related> related)
		{
			if (related.Count > 0)
			{
				foreach (Related rel in related)
				{
					// <sx:related>
					writer.WriteStartElement(Schema.DefaultPrefix, Schema.ElementNames.Related, Schema.Namespace);
					writer.WriteAttributeString(Schema.AttributeNames.Link, rel.Link);
					if (rel.Title != null)
						writer.WriteAttributeString(Schema.AttributeNames.Title, rel.Title);
					writer.WriteAttributeString(Schema.AttributeNames.Type, rel.Type.ToString().ToLower());
					writer.WriteEndElement();
				}
			}
		}

		private void WriteHistory(IEnumerable<History> updatesHistory)
		{
			foreach (History history in updatesHistory)
			{
				// <sx:history>
				writer.WriteStartElement(Schema.DefaultPrefix, Schema.ElementNames.History, Schema.Namespace);
				writer.WriteAttributeString(Schema.AttributeNames.Sequence, XmlConvert.ToString(history.Sequence));
				writer.WriteAttributeString(Schema.AttributeNames.When, Timestamp.ToString(history.When.Value));
				writer.WriteAttributeString(Schema.AttributeNames.By, history.By);
				// </sx:history>
				writer.WriteEndElement();
			}
		}

		protected abstract void WriteFeed(Feed feed, XmlWriter writer);
		protected abstract void WriteItem(Item item, XmlWriter writer);
	}
}
