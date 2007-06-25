#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Xml.XPath;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class RssReaderWriterFixture : TestFixtureBase
	{
		[TestMethod]
		public void ShouldRoundtripItemWithConflicts()
		{
			string id = Guid.NewGuid().ToString();
			Sync localSync = Behaviors.Create(id, "local", DateTime.Now, false);
			Item localItem = new Item(
				new XmlItem(id, "title", "description", DateTime.Now, GetNavigator("<payload><foo/></payload>")),
				localSync);
			
			// Branch item for conflicting edits
			DateTime conflicTime = DateTime.Now;
			IXmlItem remoteXml = localItem.XmlItem.Clone();
			Sync remoteSync = localItem.Sync.Clone();

			// local edit
			localSync = Behaviors.Update(localItem.Sync, "local", DateTime.Now, false);
			localItem = new Item(localItem.XmlItem, localSync);
			localItem.XmlItem.Timestamp = localSync.LastUpdate.When.Value;

			// remote edit
			remoteXml.Title = "remote";
			remoteSync = Behaviors.Update(remoteSync, "remote", conflicTime, false);
			localItem.Sync.Conflicts.Add(new Item(remoteXml, remoteSync));

			Feed f = new Feed("Conflicts", "asdasdf", "asdfasdf");

			MemoryStream mem = new MemoryStream();
			using (XmlWriter xw = XmlWriter.Create(mem))
			{
				new RssFeedWriter(xw).Write(f, new Item[] { localItem });
			}

			mem.Position = 0;

			using (XmlReader xr = XmlReader.Create(mem))
			{
				using (XmlWriter xw = XmlWriter.Create(Console.Out))
				{
					xw.WriteNode(xr, false);
				}
			}

			mem.Position = 0;
			Item item2 = null;

			using (XmlReader xr = XmlReader.Create(mem))
			{
				Feed f2;
				IEnumerable<Item> items;
				new RssFeedReader(xr).Read(out f2, out items);
				item2 = GetFirst<Item>(items);
			}

			Assert.AreEqual(localItem.Sync.Conflicts.Count, item2.Sync.Conflicts.Count);
		}


		[TestMethod]
		public void ShouldWriteFeedWithDeletedItem()
		{
			IXmlRepository xmlRepo = new MockXmlRepository().AddTwoItems();
			SyncEngine engine = new SyncEngine(
				xmlRepo,
				new MockSyncRepository());

			Item item = GetFirst<Item>(engine.Export());
			xmlRepo.Remove(item.XmlItem.Id);

			StringWriter sw = new StringWriter();
			XmlWriterSettings set = new XmlWriterSettings();
			set.Indent = true;
			XmlWriter xw = XmlWriter.Create(sw, set);

			Feed feed = new Feed("Hello World", "http://kzu", "this is my feed");
			feed.Sharing.Related.Add(new Related("http://kzu/full", RelatedType.Complete));

			FeedWriter writer = new RssFeedWriter(xw);
			engine.Publish(feed, writer);

			xw.Flush();

			Feed feed2;
			IEnumerable<Item> i;
			new RssFeedReader(XmlReader.Create(new StringReader(sw.ToString()))).Read(out feed2, out i);

			Assert.AreEqual(feed.Description, feed2.Description);
			Assert.AreEqual(feed.Link, feed2.Link);
			Assert.AreEqual(feed.Title, feed2.Title);

			List<Item> items = new List<Item>(i);

			Assert.AreEqual(2, items.Count);
		}
	}
}
