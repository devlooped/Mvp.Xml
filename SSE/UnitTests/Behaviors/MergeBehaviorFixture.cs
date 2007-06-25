#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Text;
using System.Collections.Generic;
using System.Xml.XPath;
using System.IO;
using System.Threading;

namespace Mvp.Xml.Synchronization.Tests
{
    [TestClass]
    public class MergeBehaviorFixture : TestFixtureBase
    {
		 [TestMethod]
		 public void ShouldWinLatestUpdateWithoutConflicts()
		 {
			 MockXmlRepository xmlRepo = new MockXmlRepository();
			 MockSyncRepository syncRepo = new MockSyncRepository();

			 Sync sa = Behaviors.Create(Guid.NewGuid().ToString(), "kzu", DateTime.Now, false);

			 Sync sb = sa.Clone();

			 sb = Behaviors.Update(sb, "vga", DateTime.Now.AddSeconds(5), false);

			 Item a = new Item(new XmlItem("a", "a", GetNavigator("<payload/>")), sa);
			 a.XmlItem.Id = sb.Id;
			 xmlRepo.Add(a.XmlItem);
			 syncRepo.Save(a.Sync);

			 Item b = new Item(new XmlItem("b", "b", GetNavigator("<payload/>")), sb);
			 b.XmlItem.Id = sb.Id;

			 ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, b);

			 Assert.AreEqual(MergeOperation.Updated, result.Operation);
			 Assert.AreEqual("b", result.Proposed.XmlItem.Title);
			 Assert.AreEqual("vga", result.Proposed.Sync.LastUpdate.By);
		 }

		 [TestMethod]
		 public void ShouldNoOpForEqualItem()
		 {
			 MockXmlRepository xmlRepo = new MockXmlRepository();
			 MockSyncRepository syncRepo = new MockSyncRepository();

			 Sync sa = Behaviors.Create(Guid.NewGuid().ToString(), "kzu", DateTime.Now, false);

			 Sync sb = sa.Clone();

			 Item a = new Item(new XmlItem("a", "a", GetNavigator("<payload/>")), sa);
			 a.XmlItem.Id = sb.Id;
			 xmlRepo.Add(a.XmlItem);
			 syncRepo.Save(a.Sync);

			 Item b = new Item(new XmlItem("a", "a", GetNavigator("<payload/>")), sb);
			 b.XmlItem.Id = sb.Id;

			 ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, b);

			 Assert.AreEqual(MergeOperation.None, result.Operation);
		 }

		 [TestMethod]
		 public void ShouldAddWithoutConflicts()
		 {
			 MockXmlRepository xmlRepo = new MockXmlRepository();
			 MockSyncRepository syncRepo = new MockSyncRepository();

			 Sync sa = new Sync(Guid.NewGuid().ToString());
			 Behaviors.Update(sa, "kzu", DateTime.Now, false);

			 Item a = new Item(new XmlItem("a", "a", GetNavigator("<payload/>")), sa);
			 ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, a);

			 Assert.AreEqual(MergeOperation.Added, result.Operation);
		 }

		 [ExpectedException(typeof(ArgumentException))]
		 [TestMethod]
		 public void ShouldThrowIfSyncNoHistory()
		 {
			 MockXmlRepository xmlRepo = new MockXmlRepository();
			 MockSyncRepository syncRepo = new MockSyncRepository();

			 Sync sa = new Sync(Guid.NewGuid().ToString());

			 Sync sb = sa.Clone();

			 Item a = new Item(new XmlItem("a", "a", GetNavigator("<payload/>")), sa);
			 a.XmlItem.Id = sb.Id;
			 a.Sync.ItemTimestamp = xmlRepo.Add(a.XmlItem);
			 syncRepo.Save(a.Sync);

			 Item b = new Item(new XmlItem("b", "b", GetNavigator("<payload/>")), sb);
			 b.XmlItem.Id = sb.Id;

			 ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, b);
		 }

		 [TestMethod]
		 public void ShouldWinLatestUpdateWithConflicts()
		 {
			 MockXmlRepository xmlRepo = new MockXmlRepository();
			 MockSyncRepository syncRepo = new MockSyncRepository();

			 Sync sa = Behaviors.Create(Guid.NewGuid().ToString(), 
				 "kzu", DateTime.Now.Subtract(TimeSpan.FromSeconds(10)), false);

			 Sync sb = sa.Clone();
			 sb = Behaviors.Update(sb, "vga", DateTime.Now.AddSeconds(50), false);
			 sa = Behaviors.Update(sa, "kzu", DateTime.Now.AddSeconds(100), false);

			 Item a = new Item(new XmlItem("a", "a", GetNavigator("<payload/>")), sa);
			 a.XmlItem.Id = sb.Id;
			 a.Sync.ItemTimestamp = xmlRepo.Add(a.XmlItem);
			 syncRepo.Save(a.Sync);

			 Item b = new Item(new XmlItem("b", "b", GetNavigator("<payload/>")), sb);
			 b.XmlItem.Id = sb.Id;

			 ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, b);

			 Assert.AreEqual(MergeOperation.Conflict, result.Operation);
			 Assert.AreEqual("a", result.Proposed.XmlItem.Title);
			 Assert.AreEqual("kzu", result.Proposed.Sync.LastUpdate.By);
			 Assert.AreEqual(1, result.Proposed.Sync.Conflicts.Count);
		 }

		 [TestMethod]
		 public void ShouldWinLatestUpdateWithConflictsPreserved()
		 {
			 MockXmlRepository xmlRepo = new MockXmlRepository();
			 MockSyncRepository syncRepo = new MockSyncRepository();

			 Sync sa = new Sync(Guid.NewGuid().ToString());
			 sa = Behaviors.Update(sa, "kzu", DateTime.Now, false);

			 Sync sb = sa.Clone();

			 DateTime now;

			 sb = Behaviors.Update(sb, "vga", DateTime.Now.AddSeconds(50), false);
			 sa = Behaviors.Update(sa, "kzu", DateTime.Now.AddSeconds(100), false);

			 Item a = new Item(new XmlItem("a", "a", GetNavigator("<payload/>")), sa);
			 a.XmlItem.Id = sb.Id;
			 xmlRepo.Add(a.XmlItem);
			 syncRepo.Save(a.Sync);

			 Item b = new Item(new XmlItem("b", "b", GetNavigator("<payload/>")), sb);
			 b.XmlItem.Id = sb.Id;

			 ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, b);
			 Assert.AreEqual(MergeOperation.Conflict, result.Operation);
			 Assert.AreEqual("a", result.Proposed.XmlItem.Title);
			 Assert.AreEqual("kzu", result.Proposed.Sync.LastUpdate.By);
			 Assert.AreEqual(1, result.Proposed.Sync.Conflicts.Count);

			 // Merge the winner with conflict with the local no-conflict one.
			 // Should be an update.
			 result = Behaviors.Merge(xmlRepo, syncRepo, result.Proposed);

			 Assert.AreEqual(MergeOperation.Conflict, result.Operation);
			 Assert.AreEqual("a", result.Proposed.XmlItem.Title);
			 Assert.AreEqual("kzu", result.Proposed.Sync.LastUpdate.By);
			 Assert.AreEqual(1, result.Proposed.Sync.Conflicts.Count);
		 }

		 [TestMethod]
		 public void ShouldMergeNoneIfEqualItem()
		 {
			 MockXmlRepository xmlRepo = new MockXmlRepository();
			 MockSyncRepository syncRepo = new MockSyncRepository();

			 DateTime now = DateTime.Now;
			 Sync sa = Behaviors.Create(Guid.NewGuid().ToString(), "kzu", now, false);
			 Sync sb = Behaviors.Update(sa, "kzu", now, false);

			 Item a = new Item(new XmlItem("a", "a", GetNavigator("<payload/>")), sa);
			 a.XmlItem.Id = sb.Id;
			 xmlRepo.Add(a.XmlItem);
			 syncRepo.Save(a.Sync);
			 
			 Item b = new Item(new XmlItem("b", "b", GetNavigator("<payload/>")), sb);
			 b.XmlItem.Id = sb.Id;

			 ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, a);

			 //Assert.AreEqual(MergeOperation.Conflict, result.Operation);
			 //Assert.AreEqual("a", result.Proposed.XmlItem.Title);
			 //Assert.AreEqual("kzu", result.Proposed.Sync.LastUpdate.By);
			 //Assert.AreEqual(1, result.Proposed.Sync.Conflicts.Count);
			 
			 //// Merge the winner with conflict with the local no-conflict one.
			 //// Should be an update.
			 //result = Behaviors.Merge(xmlRepo, syncRepo, result.Proposed);

			 //Assert.AreEqual(MergeOperation.Conflict, result.Operation);
			 //Assert.AreEqual("a", result.Proposed.XmlItem.Title);
			 //Assert.AreEqual("kzu", result.Proposed.Sync.LastUpdate.By);
			 //Assert.AreEqual(1, result.Proposed.Sync.Conflicts.Count);
		 }
	}
}