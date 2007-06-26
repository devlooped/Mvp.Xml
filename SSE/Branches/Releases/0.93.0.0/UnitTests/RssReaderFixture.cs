#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class RssReaderFixture : TestFixtureBase
	{
		[TestMethod]
		public void ShouldReadItems()
		{
			string xml = @"
<rss version='2.0' xmlns:sx='http://www.microsoft.com/schemas/sse'>
 <channel>
  <title>To Do List</title>
  <description>A list of items to do</description>
  <link>http://somefakeurl.com/partial.xml</link>
  <sx:sharing version='0.93' since='2005-02-13T18:30:02Z'
    until='2005-05-23T18:30:02Z' >
   <sx:related link='http://x.com/all.xml' type='complete' />
   <sx:related link='http://y.net/B.xml' type='aggregated' 
    title='To Do List (Jacks Copy)' />
  </sx:sharing>
  <item>
   <title>Buy groceries</title>
   <description>Get milk, eggs, butter and bread</description>
   <pubDate>Sun, 19 May 02 15:21:36 GMT</pubDate>
   <customer id='1' />
   <sx:sync id='0a7903db47fb0fff' updates='3'>
    <sx:history sequence='3' by='JEO2000'/>
    <sx:history sequence='2' by='REO1750'/>
    <sx:history sequence='1' by='REO1750'/>
	<sx:conflicts>
	  <item>
	   <title>Buy icecream</title>
	   <description>Get hagen daaz</description>
	   <pubDate>Sun, 19 May 02 12:21:36 GMT</pubDate>
	   <customer id='1' />
	   <sx:sync id='0a7903db47fb0fff' updates='1'>
		<sx:history sequence='1' by='REO1750'/>
	   </sx:sync>
	  </item>
	</sx:conflicts>
   </sx:sync>
  </item>
 </channel>
</rss>";

			IFeedReader reader = new RssFeedReader(GetReader(xml));

			Feed feed;
			IEnumerable<Item> i;

			reader.Read(out feed, out i);

			Assert.AreEqual("To Do List", feed.Title);
			Assert.AreEqual("A list of items to do", feed.Description);
			Assert.AreEqual("http://somefakeurl.com/partial.xml", feed.Link);
			Assert.AreEqual(2, feed.Sharing.Related.Count);
			List<Item> items = new List<Item>(i);
			Assert.AreEqual(1, items.Count);
			Assert.AreEqual("Buy groceries", items[0].XmlItem.Title);
			Assert.AreEqual("Get milk, eggs, butter and bread", items[0].XmlItem.Description);
			Assert.AreEqual(RssDateTime.Parse("Sun, 19 May 02 15:21:36 GMT").LocalTime, items[0].XmlItem.Timestamp);
			Assert.AreEqual(3, items[0].Sync.Updates);
			Assert.AreEqual("JEO2000", items[0].Sync.LastUpdate.By);
			Assert.AreEqual("0a7903db47fb0fff", items[0].Sync.Id);
			Assert.AreEqual("0a7903db47fb0fff", items[0].XmlItem.Id);
			Assert.AreEqual(1, items[0].Sync.Conflicts.Count);
			Assert.AreEqual("Buy icecream", items[0].Sync.Conflicts[0].XmlItem.Title);
			Assert.AreEqual(@"<payload>
  <customer id=""1"" />
</payload>", ReadToEnd(new XmlNodeReader(items[0].XmlItem.Payload)));
		}

		[TestMethod]
		public void ShouldReadItemsWithEmptySharing()
		{
			string xml = @"
<rss version='2.0' xmlns:sx='http://www.microsoft.com/schemas/sse'>
 <channel>
  <title>To Do List</title>
  <description>A list of items to do</description>
  <link>http://somefakeurl.com/partial.xml</link>
  <sx:sharing version='0.93' since='2005-02-13T18:30:02Z'
    until='2005-05-23T18:30:02Z' />
  <item>
   <title>Buy groceries</title>
   <description>Get milk, eggs, butter and bread</description>
   <pubDate>Sun, 19 May 02 15:21:36 GMT</pubDate>
   <customer id='1' />
   <sx:sync id='0a7903db47fb0fff' updates='3'>
    <sx:history sequence='3' by='JEO2000'/>
    <sx:history sequence='2' by='REO1750'/>
    <sx:history sequence='1' by='REO1750'/>
	<sx:conflicts>
	  <item>
	   <title>Buy icecream</title>
	   <description>Get hagen daaz</description>
	   <pubDate>Sun, 19 May 02 12:21:36 GMT</pubDate>
	   <customer id='1' />
	   <sx:sync id='0a7903db47fb0fff' updates='1'>
		<sx:history sequence='1' by='REO1750'/>
	   </sx:sync>
	  </item>
	</sx:conflicts>
   </sx:sync>
  </item>
 </channel>
</rss>";

			IFeedReader reader = new RssFeedReader(GetReader(xml));

			Feed feed;
			IEnumerable<Item> i;

			reader.Read(out feed, out i);

			Assert.AreEqual(1, Count(i));
		}

		[TestMethod]
		public void ShouldSetPayloadToUnknownElements()
		{
			string xml = @"
<rss version='2.0' xmlns:sx='http://www.microsoft.com/schemas/sse'>
 <channel>
  <title>To Do List</title>
  <description>A list of items to do</description>
  <link>http://somefakeurl.com/partial.xml</link>
  <sx:sharing version='0.93'/>
  <item>
   <title>Buy groceries</title>
   <payload>unknown</payload>
   <description>Get milk, eggs, butter and bread</description>
   <pubDate>Sun, 19 May 02 15:21:36 GMT</pubDate>
   <someData id='1' xmlns='foo' />
   <sx:sync id='0a7903db47fb0fff' updates='3'>
    <sx:history sequence='3' by='JEO2000'/>
    <sx:history sequence='2' by='REO1750'/>
    <sx:history sequence='1' by='REO1750'/>
   </sx:sync>
   <someOtherData xmlns='bar' />
  </item>
 </channel>
</rss>";

			IFeedReader reader = new RssFeedReader(GetReader(xml));

			Feed feed;
			IEnumerable<Item> i;

			reader.Read(out feed, out i);

			List<Item> items = new List<Item>(i);
			Assert.AreEqual(1, items.Count);
			Assert.AreEqual(@"<payload>
  <payload>unknown</payload>
  <someData id=""1"" xmlns=""foo"" />
  <someOtherData xmlns=""bar"" />
</payload>", ReadToEnd(new XmlNodeReader(items[0].XmlItem.Payload)));
		}

		[TestMethod]
		public void ShouldReadTwoItems()
		{
			MockSyncRepository localSync = new MockSyncRepository();
			MockXmlRepository localXml = new MockXmlRepository();
			SyncEngine localEngine = new SyncEngine(localXml, localSync);
			localXml.AddTwoItems();

			MemoryStream mem = new MemoryStream();
			XmlWriter w2 = XmlWriter.Create(mem);
			Feed feed1 = new Feed("Mock", "http://myclient/feed/", "Mock client feed");
			localEngine.Publish(feed1, new RssFeedWriter(w2));
			w2.Flush();

			mem.Position = 0;
			//Console.WriteLine(ReadToEnd(GetReader(new StreamReader(mem).ReadToEnd())));
			//mem.Position = 0;
			Feed feed2;
			IEnumerable<Item> i2;
			new RssFeedReader(XmlReader.Create(mem)).Read(out feed2, out i2);
			List<Item> items2 = new List<Item>(i2);

			Assert.AreEqual(2, items2.Count);
		}
	}
}
