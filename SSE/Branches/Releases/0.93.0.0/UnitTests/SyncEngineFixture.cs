#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock2;
#endif

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Threading;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class SyncEngineFixture : TestFixtureBase
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowIfNullXmlRepo()
		{
			new SyncEngine(null, new MockSyncRepository());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowIfNullSyncRepo()
		{
			new SyncEngine(new MockXmlRepository(), null);
		}

		[TestMethod]
		public void ShouldExportAllItems()
		{
			SyncEngine engine = new SyncEngine(
				new MockXmlRepository().AddTwoItems(),
				new MockSyncRepository());

			IEnumerable<Item> items = engine.Export();

			Assert.AreEqual(2, new List<Item>(items).Count);
		}

		[TestMethod]
		public void ShouldExportDeleted()
		{
			IXmlRepository xmlRepo = new MockXmlRepository().AddOneItem();
			SyncEngine engine = new SyncEngine(
				xmlRepo,
				new MockSyncRepository());

			Item item = GetFirst<Item>(engine.Export());
			xmlRepo.Remove(item.XmlItem.Id);

			IEnumerable<Item> items = engine.Export();
			Assert.AreEqual(1, Count(items));
		}

		[TestMethod]
		public void ShouldCreateSyncIfExportingItemsWithNoMapping()
		{
			MockXmlRepository xmlrepo = new MockXmlRepository();
			xmlrepo.AddTwoItems();
			SyncEngine engine = new SyncEngine(
				xmlrepo, new MockSyncRepository());

			IEnumerable<Item> items = engine.Export();

			foreach (Item item in items)
			{
				Assert.IsNotNull(item.Sync);
			}

			Assert.AreEqual(2, new List<Item>(items).Count);
		}

		[TestMethod]
		public void ShouldExportByDays()
		{
			MockXmlRepository xmlrepo = new MockXmlRepository();
			xmlrepo.AddThreeItemsByDays();

			SyncEngine engine = new SyncEngine(
				xmlrepo, new MockSyncRepository());

			IEnumerable<Item> items = engine.Export(2);

			Assert.AreEqual(2, new List<Item>(items).Count);
		}

		[TestMethod]
		public void ShouldUpdateSyncOnExport()
		{
			MockXmlRepository xmlrepo = new MockXmlRepository();
			xmlrepo.AddOneItem();

			SyncEngine engine = new SyncEngine(
				xmlrepo, new MockSyncRepository());

			Item item = new List<Item>(engine.Export())[0];

			DateTime originalUpdate = item.Sync.LastUpdate.When.Value;

			Thread.Sleep(1000);

			item.XmlItem.Title = "updated";
			xmlrepo.Update(item.XmlItem);

			Item item2 = GetFirst<Item>(engine.Export());

			Assert.AreEqual("updated", item2.XmlItem.Title);
			Assert.AreNotEqual(originalUpdate, item2.Sync.LastUpdate.When);
			Assert.AreEqual(2, item2.Sync.Updates);
		}

		[TestMethod]
		public void ShouldDefaultNullForLastSyncUnkownFeed()
		{
			Feed f = new Feed("Foo", "http://foo", "Foo Feed");

			SyncEngine engine = new SyncEngine(
				new MockXmlRepository(),
				new MockSyncRepository());

			DateTime? lastSync = engine.GetLastSync(f.Link);

			Assert.IsNull(lastSync);
		}

		[TestMethod]
		public void ShouldPreviewImport()
		{
			SyncEngine engine = new SyncEngine(
				new MockXmlRepository(),
				new MockSyncRepository());

			IEnumerable<ItemMergeResult> results = engine.PreviewImport(GetMockItems());
			int count = 0;

			foreach (ItemMergeResult result in results)
			{
				count++;
				Assert.AreEqual(MergeOperation.Added, result.Operation);
			}

			Assert.AreEqual(2, count);
		}

		[TestMethod]
		public void ShouldImportWithNoConflicts()
		{
			IXmlRepository xmlRepo = new MockXmlRepository();
			ISyncRepository syncRepo = new MockSyncRepository();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			IList<Item> conflicts = engine.Import("http://kzu", GetMockItems());

			Assert.AreEqual(0, conflicts.Count);
			Assert.AreEqual(2, new List<IXmlItem>(xmlRepo.GetAll()).Count);
		}

		[TestMethod]
		public void ShouldImportReturnConflicts()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			string id = Guid.NewGuid().ToString();
			Sync sync = Behaviors.Create(id, DeviceAuthor.Current, DateTime.Now, false);
			Item item = new Item(
				new XmlItem(id, "foo", "bar",
					DateTime.Now, GetElement("<foo id='bar'/>")),
				sync);

			// Save original item.
			item.Sync.ItemTimestamp = xmlRepo.Add(item.XmlItem);
			syncRepo.Save(item.Sync);

			// Remote editing.
			Item incoming = item.Clone();
			incoming = new Item(new XmlItem(id, "remote", item.XmlItem.Description,
				item.Sync.LastUpdate.When.Value, item.XmlItem.Payload),
				Behaviors.Update(incoming.Sync, "REMOTE\\kzu", DateTime.Now, false));

			// Local editing.
			item = new Item(new XmlItem(id, "changed", item.XmlItem.Description,
				item.Sync.LastUpdate.When.Value, item.XmlItem.Payload),
				Behaviors.Update(item.Sync, DeviceAuthor.Current, DateTime.Now, false));
			xmlRepo.Update(item.XmlItem);
			syncRepo.Save(item.Sync);

			IList<Item> conflicts = engine.Import("http://kzu", incoming);

			Assert.AreEqual(1, conflicts.Count);
		}

		[TestMethod]
		public void ShouldImportUpdateWithConflict()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			string id = Guid.NewGuid().ToString();
			Sync sync = Behaviors.Create(id, DeviceAuthor.Current, DateTime.Now, false);
			Item item = new Item(
				new XmlItem(id, "foo", "bar",
					DateTime.Now, GetElement("<foo id='bar'/>")),
				sync);

			// Save original item.
			xmlRepo.Add(item.XmlItem);
			syncRepo.Save(item.Sync);

			Item incoming = item.Clone();

			Thread.Sleep(1000);
			
			// Local editing.
			item = new Item(new XmlItem(id, "changed", item.XmlItem.Description,
				item.Sync.LastUpdate.When.Value, item.XmlItem.Payload),
				Behaviors.Update(item.Sync, DeviceAuthor.Current, DateTime.Now, false));
			xmlRepo.Update(item.XmlItem);
			syncRepo.Save(item.Sync);

			Thread.Sleep(1000);

			// Conflicting remote editing.
			incoming = new Item(new XmlItem(id, "remote", item.XmlItem.Description,
				item.Sync.LastUpdate.When.Value, item.XmlItem.Payload),
				Behaviors.Update(incoming.Sync, "REMOTE\\kzu", DateTime.Now, false));

			IList<Item> conflicts = engine.Import("http://kzu", incoming);

			Assert.AreEqual(1, conflicts.Count);
			Assert.AreEqual(1, new List<IXmlItem>(xmlRepo.GetAll()).Count);
			Assert.AreEqual("remote", xmlRepo.Get(id).Title);
			Assert.AreEqual("REMOTE\\kzu", syncRepo.Get(id).LastUpdate.By);
		}

		[TestMethod]
		public void ShouldUpdateLastSyncOnImportIfUpdatesSaved()
		{
			IXmlRepository xmlRepo = new MockXmlRepository();
			ISyncRepository syncRepo = new MockSyncRepository();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			engine.Import("http://kzu", GetMockItems());

			DateTime? lastSync = engine.GetLastSync("http://kzu");

			Assert.IsNotNull(lastSync);
		}

		[TestMethod]
		public void ShouldAlwaysUpdateLastSyncEvenIfOnlyConflicts()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			string id = Guid.NewGuid().ToString();
			Sync sync = Behaviors.Create(id, DeviceAuthor.Current, DateTime.Now, false);
			Item item = new Item(
				new XmlItem(id, "foo", "bar",
					DateTime.Now, GetElement("<foo id='bar'/>")),
				sync);

			// Will save the only item and update LastSync
			engine.Import("http://kzu", item);
			DateTime? lastSync = engine.GetLastSync("http://kzu");

			Thread.Sleep(100);

			Assert.AreEqual(1, new List<IXmlItem>(xmlRepo.GetAll()).Count);

			// Remote editing.
			Item incoming = new Item(new XmlItem(id, "remote", item.XmlItem.Description,
				item.Sync.LastUpdate.When.Value, item.XmlItem.Payload),
				Behaviors.Update(item.Sync.Clone(), "REMOTE\\kzu", DateTime.Now, false));

			// Local editing.
			item = new Item(new XmlItem(id, "changed", item.XmlItem.Description,
				item.Sync.LastUpdate.When.Value, item.XmlItem.Payload),
				Behaviors.Update(item.Sync.Clone(), DeviceAuthor.Current, DateTime.Now, false));
			xmlRepo.Update(item.XmlItem);
			syncRepo.Save(item.Sync);

			IList<Item> conflicts = engine.Import("http://kzu", incoming);

			Assert.AreEqual(1, conflicts.Count);
			Assert.AreNotEqual(lastSync, engine.GetLastSync("http://kzu"));
		}

		[TestMethod]
		public void ShouldUpdateSyncLocalItemsOnImport()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			string id = Guid.NewGuid().ToString();
			Sync sync = Behaviors.Create(id, DeviceAuthor.Current, DateTime.Now, false);
			sync.ItemTimestamp = DateTime.Now;
			Item item = new Item(
				new XmlItem(id, "foo", "bar",
					sync.ItemTimestamp.Value, GetElement("<foo id='bar'/>")),
				sync);

			// Save original item.
			xmlRepo.Add(item.XmlItem);
			syncRepo.Save(item.Sync);
			DateTime? lastUpdated = item.Sync.LastUpdate.When;

			Thread.Sleep(1000);

			// Local editing outside of SSE by the local app.
			DateTime now = xmlRepo.Update(new XmlItem(id, "changed", item.XmlItem.Description,
				DateTime.Now, item.XmlItem.Payload));
			now = Timestamp.Normalize(now);

			// Import of same item should cause it to 
			// be updated with new Sync info and a no-op on sync.
			engine.Import("http://kzu", item);

			Sync updated = syncRepo.Get(id);
			Assert.AreEqual(2, updated.Updates);
			Assert.AreNotEqual(lastUpdated, updated.LastUpdate.When);
			// New update When should match the actual XML Repository reported 
			// LastUpdated time.
			Assert.AreEqual(now, updated.LastUpdate.When);
		}

		[TestMethod]
		public void ShouldPublishCompleteFeed()
		{
			MockSyncRepository syncRepo = new MockSyncRepository();
			MockXmlRepository xmlRepo = new MockXmlRepository();
			xmlRepo.AddThreeItemsByDays();

			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);
			Feed feed = new Feed("Hello World", "http://kzu", "this is my feed");
			feed.Sharing.Related.Add(new Related("http://kzu/full", RelatedType.Complete));

			MockWriter writer = new MockWriter();
			engine.Publish(feed, writer);

			Assert.AreEqual(3, writer.Items);
		}

		[TestMethod]
		public void ShouldPublishLast1Day()
		{
			MockSyncRepository syncRepo = new MockSyncRepository();
			MockXmlRepository xmlRepo = new MockXmlRepository();
			xmlRepo.AddThreeItemsByDays();

			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);
			Feed feed = new Feed("Hello World", "http://kzu", "this is my feed");
			feed.Sharing.Related.Add(new Related("http://kzu/full", RelatedType.Complete));

			MockWriter writer = new MockWriter();
			engine.Publish(feed, writer, 1);

			Assert.AreEqual(2, writer.Items);
		}

		[TestMethod]
		public void ShouldImportFeedAndUpdateLastSync()
		{
			MockSyncRepository syncRepo = new MockSyncRepository();
			MockXmlRepository xmlRepo = new MockXmlRepository();

			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			Assert.IsNull(engine.GetLastSync("Feed.xml"));

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
   </sx:sync>
  </item>
 </channel>
</rss>";

			RssFeedReader rss = new RssFeedReader(GetReader(xml));
			Feed feed;
			IEnumerable<Item> items;
			rss.Read(out feed, out items);

			IList<Item> conflicts = engine.Import("Feed.xml", items);

			Assert.AreEqual(0, conflicts.Count);

			Assert.IsNotNull(engine.GetLastSync("Feed.xml"));
			Assert.AreEqual(1, new List<IXmlItem>(xmlRepo.GetAll()).Count);
		}

		[TestMethod]
		public void ShouldImportFeedAndUpdateLastSyncFromFeedInfo()
		{
			MockSyncRepository syncRepo = new MockSyncRepository();
			MockXmlRepository xmlRepo = new MockXmlRepository();

			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			Assert.IsNull(engine.GetLastSync("http://somefakeurl.com/partial.xml"));

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
   </sx:sync>
  </item>
 </channel>
</rss>";

			RssFeedReader rss = new RssFeedReader(GetReader(xml));
			IList<Item> conflicts = engine.Subscribe(rss);

			Assert.AreEqual(0, conflicts.Count);

			Assert.IsNotNull(engine.GetLastSync("http://somefakeurl.com/partial.xml"));
			Assert.AreEqual(1, new List<IXmlItem>(xmlRepo.GetAll()).Count);
		}

		[TestMethod]
		public void ShouldImportItemsWithConflict()
		{
			MockSyncRepository syncRepo = new MockSyncRepository();
			MockXmlRepository xmlRepo = new MockXmlRepository();

			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			Assert.IsNull(engine.GetLastSync("http://somefakeurl.com/partial.xml"));

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

			RssFeedReader rss = new RssFeedReader(GetReader(xml));
			IList<Item> conflicts = engine.Subscribe(rss);

			Assert.AreEqual(1, conflicts.Count);

			Assert.IsNotNull(engine.GetLastSync("http://somefakeurl.com/partial.xml"));
			Assert.AreEqual(1, Count(xmlRepo.GetAll()));
		}

		[TestMethod]
		public void ShouldPropagateUpdate()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository().AddOneItem();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			IEnumerable<Item> items = engine.Export();

			ISyncRepository syncRepo2 = new MockSyncRepository();
			IXmlRepository xmlRepo2 = new MockXmlRepository();
			SyncEngine engine2 = new SyncEngine(xmlRepo2, syncRepo2);

			engine2.Import("mock", items);

			// both repositories are in sync now.

			Thread.Sleep(100);

			// update item on one repository.
			IXmlItem item = GetFirst<IXmlItem>(xmlRepo.GetAll());
			item.Title = "updated";
			item.Timestamp = DateTime.Now;
			xmlRepo.Update(item);

			IList<Item> conflicts = engine2.Import("mock", engine.Export());
			Assert.AreEqual(0, conflicts.Count);

			IXmlItem item2 = GetFirst<IXmlItem>(xmlRepo2.GetAll());

			Assert.AreEqual("updated", item2.Title);
		}

		[TestMethod]
		public void ShouldSetSyncItemTimestampOnImport()
		{
			MockSyncRepository syncRepo = new MockSyncRepository();
			MockXmlRepository xmlRepo = new MockXmlRepository();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

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
   </sx:sync>
  </item>
 </channel>
</rss>";

			engine.Subscribe(new RssFeedReader(GetReader(xml)));

			Item item = GetFirst<Item>(engine.Export());
			Assert.IsNotNull(item.Sync.ItemTimestamp);
			Assert.AreEqual(item.XmlItem.Timestamp, item.Sync.ItemTimestamp);
		}


		[TestMethod]
		public void ShouldRetrieveItemsWithConflicts()
		{
			MockSyncRepository syncRepo = new MockSyncRepository();
			MockXmlRepository xmlRepo = new MockXmlRepository();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

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

			engine.Subscribe(new RssFeedReader(GetReader(xml)));

			Assert.AreEqual(1, Count(engine.ExportConflicts()));
		}

		[TestMethod]
		public void ShouldUpdateSyncDeletedOnExportConflictsIfItemIsDeleted()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository().AddOneItem();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			// Cause the item to be Sync'ed.
			Item item = GetFirst<Item>(engine.Export());

			// Introduce a conflict.
			Item clone = item.Clone();
			clone.XmlItem.Title = "Conflict";
			Sync updatedSync = Behaviors.Update(clone.Sync, "Conflict", DateTime.Now, false);
			item.Sync.Conflicts.Add(new Item(clone.XmlItem, updatedSync));
			syncRepo.Save(item.Sync);

			// Delete xml item.
			xmlRepo.Remove(item.XmlItem.Id);

			Item conflict = GetFirst<Item>(engine.ExportConflicts());
			Assert.IsNotNull(conflict);
		}

		[TestMethod]
		public void ShouldSaveUpdatedItemOnResolveConflicts()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository().AddOneItem();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			// Cause the item to be Sync'ed.
			Item item = GetFirst<Item>(engine.Export());

			// Introduce a conflict.
			IXmlItem xml = item.XmlItem.Clone();
			xml.Title = "Conflict";
			Sync updatedSync = Behaviors.Update(item.Sync, "Conflict", DateTime.Now, false);
			item.Sync.Conflicts.Add(new Item(xml, updatedSync));

			item.XmlItem.Title = "Resolved";

			Item resolved = engine.Save(item, true);

			IXmlItem storedXml = xmlRepo.Get(item.XmlItem.Id);

			Assert.AreEqual("Resolved", storedXml.Title);
		}

		[TestMethod]
		public void ShouldSaveUpdatedItemOnResolveConflicts2()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository().AddOneItem();
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			// Cause the item to be Sync'ed.
			Item item = GetFirst<Item>(engine.Export());

			// Introduce a conflict.
			IXmlItem xml = item.XmlItem.Clone();
			xml.Title = "Conflict";
			Sync updatedSync = Behaviors.Update(item.Sync, "Conflict", DateTime.Now, false);
			item.Sync.Conflicts.Add(new Item(xml, updatedSync));

			item.XmlItem.Title = "Resolved";

			Item resolved = engine.Save(item, true);

			IXmlItem storedXml = xmlRepo.Get(item.XmlItem.Id);

			Assert.AreEqual("Resolved", storedXml.Title);
		}

#if !PocketPC

		[TestMethod]
		public void ShouldSaveAddNewItemAndSync()
		{
			Mockery mocks = new Mockery();
			ISyncRepository syncRepo = mocks.NewMock<ISyncRepository>();
			IXmlRepository xmlRepo = mocks.NewMock<IXmlRepository>();

			Expect.Once.On(xmlRepo).Method("Contains").Will(Return.Value(false));
			Expect.Once.On(xmlRepo).Method("Add").Will(Return.Value(DateTime.Now));
			Expect.Once.On(syncRepo).Method("Save");

			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			Sync sync = Behaviors.Create(Guid.NewGuid().ToString(), "kzu", null, false);
			XmlItem item = new XmlItem("foo", "bar", GetElement("<payload/>"));

			engine.Save(new Item(item, sync));

			mocks.VerifyAllExpectationsHaveBeenMet();
		}

		[TestMethod]
		public void ShouldSaveUpdateExistingItemAndSync()
		{
			Mockery mocks = new Mockery();
			ISyncRepository syncRepo = mocks.NewMock<ISyncRepository>();
			IXmlRepository xmlRepo = mocks.NewMock<IXmlRepository>();

			Expect.Once.On(xmlRepo).Method("Contains").Will(Return.Value(true));
			Expect.Once.On(xmlRepo).Method("Update").Will(Return.Value(DateTime.Now));
			Expect.Once.On(syncRepo).Method("Save");

			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);
			
			Sync sync = Behaviors.Create(Guid.NewGuid().ToString(), "kzu", null, false);
			XmlItem item = new XmlItem("foo", "bar", GetElement("<payload/>"));

			engine.Save(new Item(item, sync));

			mocks.VerifyAllExpectationsHaveBeenMet();
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfSaveNullItem()
		{
			Mockery mocks = new Mockery();
			ISyncRepository syncRepo = mocks.NewMock<ISyncRepository>();
			IXmlRepository xmlRepo = mocks.NewMock<IXmlRepository>();

			Expect.Never.On(xmlRepo);
			Expect.Never.On(syncRepo);

			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			engine.Save(null);

			mocks.VerifyAllExpectationsHaveBeenMet();
		}

#endif

		//[ExpectedException(typeof(InvalidOperationException))]
		//[TestMethod]
		//public void ShouldThrowIfRepositoryDoesntUpdateTimestamp()
		//{
		//    ISyncRepository syncRepo = new MockSyncRepository();
		//    IXmlRepository xmlRepo = new MockXmlRepository().AddOneItem();
		//    SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

		//    IEnumerable<Item> items = engine.Export();

		//    ISyncRepository syncRepo2 = new MockSyncRepository();
		//    IXmlRepository xmlRepo2 = new NotUpdatingRepository();
		//    SyncEngine engine2 = new SyncEngine(xmlRepo2, syncRepo2);

		//    engine2.Import("mock", items);
		//}

		private IEnumerable<Item> GetMockItems()
		{
			string id = null;
			DateTime? when;
			return new List<Item>(
				new Item[] {
					new Item(
						new XmlItem(
							(id = Guid.NewGuid().ToString()), "Foo", "Description",
							(when = DateTime.Now).Value, 
							GetElement("<foo id='1'/>")), 
						Behaviors.Update(new Sync(id), DeviceAuthor.Current, when, false)), 
					new Item(
						new XmlItem(
							(id = Guid.NewGuid().ToString()), "Foo", "Description", 
							(when = DateTime.Now).Value, 
							GetElement("<foo id='2'/>")), 
						Behaviors.Update(new Sync(id), DeviceAuthor.Current, when, false))
				});
		}

		class MockWriter : FeedWriter
		{
			public int Items;

			public MockWriter()
				: base(XmlWriter.Create(new MemoryStream()))
			{
				base.ItemWritten += delegate { Items++; };
			}

			protected override void WriteFeed(Feed feed, XmlWriter writer)
			{
				writer.WriteStartElement("feed");
			}

			protected override void WriteItem(Item item, XmlWriter writer)
			{
				writer.WriteStartElement("item");
			}

			
		}

		//class NotUpdatingRepository : IXmlRepository
		//{
		//    #region IXmlRepository Members

		//    public void Add(IXmlItem item)
		//    {
		//        throw new Exception("The method or operation is not implemented.");
		//    }

		//    public IXmlItem Get(string id)
		//    {
		//        throw new Exception("The method or operation is not implemented.");
		//    }

		//    public bool Remove(string id)
		//    {
		//        throw new Exception("The method or operation is not implemented.");
		//    }

		//    public bool Update(IXmlItem item)
		//    {
		//        return true;
		//    }

		//    public IEnumerable<IXmlItem> GetAll()
		//    {
		//        throw new Exception("The method or operation is not implemented.");
		//    }

		//    public IEnumerable<IXmlItem> GetAllSince(DateTime date)
		//    {
		//        throw new Exception("The method or operation is not implemented.");
		//    }

		//    public DateTime GetFirstUpdated()
		//    {
		//        throw new Exception("The method or operation is not implemented.");
		//    }

		//    public DateTime GetFirstUpdated(DateTime since)
		//    {
		//        throw new Exception("The method or operation is not implemented.");
		//    }

		//    public DateTime GetLastUpdated()
		//    {
		//        throw new Exception("The method or operation is not implemented.");
		//    }

		//    #endregion
		//}
	}
}
