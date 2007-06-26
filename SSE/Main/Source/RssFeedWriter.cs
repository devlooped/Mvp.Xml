using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.Synchronization
{
	public class RssFeedWriter : FeedWriter
	{
		public RssFeedWriter(XmlWriter writer) : base(writer) { }

		protected override void WriteFeed(Feed feed, XmlWriter writer)
		{
			writer.WriteStartElement("feed");
			writer.WriteElementString("title", feed.Title);
			writer.WriteElementString("link", feed.Link);
			writer.WriteElementString("description", feed.Description);
		}

		protected override void WriteItem(Item item, XmlWriter writer)
		{
			writer.WriteStartElement("item");
			if (!item.Sync.Deleted)
			{
				writer.WriteElementString("title", item.XmlItem.Title);
				writer.WriteElementString("description", item.XmlItem.Description);
				writer.WriteNode(new XmlNodeReader(item.XmlItem.Payload), false);
			}
		}
	}
}
