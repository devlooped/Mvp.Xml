using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using System.Globalization;
using System.IO;

namespace Mvp.Xml.Synchronization
{
	public class RssFeedReader : IFeedReader
	{
		XmlReader reader;
		public event EventHandler ItemRead;

		public RssFeedReader(XmlReader reader)
		{
			this.reader = reader;
		}

		public void Read(out Feed feed, out IEnumerable<Item> items)
		{
			feed = ReadFeed();
			items = ReadItems();
		}

		private Feed ReadFeed()
		{
			string title = null;
			string link = null;
			string description = null;
			Sharing sharing = null;

			while (reader.Read() && !IsItemElement(reader))
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName == "title")
						title = ReadElementValue(reader);
					else if (reader.LocalName == "link")
						link = ReadElementValue(reader);
					else if (reader.LocalName == "description")
						description = ReadElementValue(reader);
					else if (IsSseElement(reader, Schema.ElementNames.Sharing))
						sharing = ReadSharing();
				}
			}

			Feed feed = new Feed(title, link, description);
			if (sharing != null)
				feed.Sharing = sharing;

			return feed;
		}

		private IEnumerable<Item> ReadItems()
		{
			do
			{
				if (IsItemElement(reader) && reader.NodeType == XmlNodeType.Element)
				{
					yield return ReadItem(reader);
				}
			}
			while (reader.Read());
		}

		private Item ReadItem(XmlReader reader)
		{
			if (reader.ReadState == ReadState.Initial)
				reader.MoveToContent();
			if (!IsItemElement(reader))
				throw new InvalidOperationException();

			DateTime lastUpdated = DateTime.MinValue;
			string title = null;
			string description = null;

			MemoryStream mem = new MemoryStream();
			XmlWriter writer = XmlWriter.Create(mem);
			writer.WriteStartElement("payload");

			Sync sync = null;

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName == "title")
						title = ReadElementValue(reader);
					else if (reader.LocalName == "pubDate")
						lastUpdated = RssDateTime.Parse(ReadElementValue(reader)).LocalTime;
					else if (reader.LocalName == "description")
						description = ReadElementValue(reader);
					else if (IsSseElement(reader, Schema.ElementNames.Sync))
						sync = ReadSync();
					// Anything that is unknown is payload.
					else
						writer.WriteNode(reader.ReadSubtree(), false);
				}
				else if (reader.NodeType == XmlNodeType.EndElement &&
					IsItemElement(reader))
					break;
			}

			writer.WriteEndElement();
			writer.Close();
			mem.Position = 0;

			Item item;

			if (!sync.Deleted)
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(mem);
				XmlElement payload = doc.DocumentElement;

				item = new Item(
					new XmlItem(title, description, lastUpdated, payload),
					sync);
				item.XmlItem.Id = sync.Id;
			}
			else
			{
				item = new Item(
					new NullXmlItem(sync.Id, sync.LastUpdate.When),
					sync);
			}

			if (ItemRead != null)
				ItemRead(this, EventArgs.Empty);

			return item;
		}

		private Sharing ReadSharing()
		{
			if (!IsSseElement(reader, Schema.ElementNames.Sharing))
				throw new InvalidOperationException();

			Sharing sharing = new Sharing();
			if (reader.MoveToAttribute(Schema.AttributeNames.Since))
				sharing.Since = reader.Value;
			if (reader.MoveToAttribute(Schema.AttributeNames.Until))
				sharing.Until = reader.Value;
			if (reader.MoveToAttribute(Schema.AttributeNames.Expires))
				sharing.Expires = DateTime.Parse(reader.Value);

			if (reader.NodeType == XmlNodeType.Attribute)
				reader.MoveToElement();

			if (!reader.IsEmptyElement)
			{
				while (reader.Read() &&
					!(IsSseElement(reader, Schema.ElementNames.Sharing) &&
					reader.NodeType == XmlNodeType.EndElement))
				{
					if (IsSseElement(reader, Schema.ElementNames.Related) &&
						reader.NodeType == XmlNodeType.Element)
					{
						sharing.Related.Add(new Related(
							reader.GetAttribute(Schema.AttributeNames.Link),
							(RelatedType)Enum.Parse(
								typeof(RelatedType),
								CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
									reader.GetAttribute(Schema.AttributeNames.Type)), 
									false),
							reader.GetAttribute(Schema.AttributeNames.Title)));
					}
				}
			}

			return sharing;
		}

		internal Sync ReadSync()
		{
			if (!IsSseElement(reader, Schema.ElementNames.Sync))
				throw new InvalidOperationException();

			Sync newSync = null;

			reader.MoveToAttribute(Schema.AttributeNames.Id);
			string id = reader.Value;
			reader.MoveToAttribute(Schema.AttributeNames.Updates);
			int updates = XmlConvert.ToInt32(reader.Value);

			newSync = new Sync(id, updates);

			if (reader.MoveToAttribute(Schema.AttributeNames.Deleted))
			{
				newSync.Deleted = XmlConvert.ToBoolean(reader.Value);
			}
			if (reader.MoveToAttribute(Schema.AttributeNames.NoConflicts))
			{
				newSync.NoConflicts = XmlConvert.ToBoolean(reader.Value);
			}

			reader.MoveToElement();

			List<History> historyUpdates = new List<History>();

			while (reader.Read() && 
				!(reader.NodeType == XmlNodeType.EndElement &&
					IsSseElement(reader, Schema.ElementNames.Sync)))
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (IsSseElement(reader, Schema.ElementNames.History))
					{
						reader.MoveToAttribute(Schema.AttributeNames.Sequence);
						int sequence = XmlConvert.ToInt32(reader.Value);
						string by = null;
						DateTime? when = null;

						if (reader.MoveToAttribute(Schema.AttributeNames.When))
							when = DateTime.Parse(reader.Value);
						if (reader.MoveToAttribute(Schema.AttributeNames.By))
							by = reader.Value;

						historyUpdates.Add(new History(by, when, sequence));
					}
					else if (IsSseElement(reader, Schema.ElementNames.Conflicts))
					{
						while (reader.Read() &&
							!(IsSseElement(reader, Schema.ElementNames.Conflicts)
							&& reader.NodeType == XmlNodeType.EndElement))
						{
							if (IsItemElement(reader))
							{
								newSync.Conflicts.Add(ReadItem(reader.ReadSubtree()));
							}
						}
					}
				}
			}

			if (historyUpdates.Count != 0)
			{
				historyUpdates.Reverse();
				foreach (History history in historyUpdates)
				{
					newSync.AddHistory(history);
				} 
			}

			return newSync;
		}

		private bool IsSseElement(XmlReader reader, string elementName)
		{
			return reader.LocalName == elementName &&
				reader.NamespaceURI == Schema.Namespace;
		}

		private bool IsItemElement(XmlReader reader)
		{
			return reader.LocalName == "item" &&
				reader.NamespaceURI == String.Empty;
		}

		private string ReadElementValue(XmlReader reader)
		{
			if (reader.NodeType == XmlNodeType.Element)
			{
				if (reader.IsEmptyElement)
					return null;
				else
					reader.Read();
			}

			return reader.Value;
		}

	}
}
