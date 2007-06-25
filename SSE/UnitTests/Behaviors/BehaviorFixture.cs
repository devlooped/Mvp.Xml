#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class BehaviorFixture : TestFixtureBase
	{
		#region Merge

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void MergeShouldThrowIfXmlRepoNull()
		{
			Behaviors.Merge(null, new MockSyncRepository(), new Item(
				new XmlItem("a", "b", GetNavigator("<c/>")),
				new Sync(Guid.NewGuid().ToString())));
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void MergeShouldThrowIfSyncRepoNull()
		{
			Behaviors.Merge(new MockXmlRepository(), null, new Item(
				new XmlItem("a", "b", GetNavigator("<c/>")),
				new Sync(Guid.NewGuid().ToString())));
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void MergeShouldThrowIfItemNull()
		{
			Behaviors.Merge(new MockXmlRepository(), new MockSyncRepository(), null);
		}

		[TestMethod]
		public void MergeShouldUpdateSyncBeforeMerge()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();

			string id = Guid.NewGuid().ToString();
			XmlItem xmlItem = new XmlItem(id, "foo", "bar",
					DateTime.Now, GetNavigator("<foo id='bar'/>"));
			// Save to xml repo only, as a regular app would do.
			xmlRepo.Add(xmlItem);
			Thread.Sleep(1000);

			Sync sync = Behaviors.Create(id, DeviceAuthor.Current, DateTime.Now, false);
			Item remoteItem = new Item(
				xmlItem, sync);

			Thread.Sleep(1000);

			ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, remoteItem);

			Assert.AreEqual(MergeOperation.Updated, result.Operation);
			Assert.IsNotNull(result.Proposed);
		}

		[TestMethod]
		public void MergeShouldAddWithoutConflict()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();

			Sync sync = Behaviors.Create(Guid.NewGuid().ToString(), DeviceAuthor.Current, DateTime.Now, false);

			Item remoteItem = new Item(
				new XmlItem(sync.Id, "foo", "bar",
					DateTime.Now, GetNavigator("<foo id='bar'/>")),
				sync);

			ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, remoteItem);

			Assert.AreEqual(MergeOperation.Added, result.Operation);
			Assert.IsNotNull(result.Proposed);
		}

		[TestMethod]
		public void MergeShouldUpdateWithoutConflict()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();

			Sync sync = Behaviors.Create(Guid.NewGuid().ToString(), DeviceAuthor.Current, DateTime.Now, false);
			Item item = new Item(
				new XmlItem(sync.Id, "foo", "bar",
					DateTime.Now, GetNavigator("<foo id='bar'/>")),
				sync);

			// Save original item.
			xmlRepo.Add(item.XmlItem);
			syncRepo.Save(item.Sync);

			Thread.Sleep(50);

			// Simulate editing.
			sync = Behaviors.Update(item.Sync, "REMOTE\\kzu", DateTime.Now, false);
			item = new Item(new XmlItem(sync.Id, "changed", item.XmlItem.Description,
				sync.LastUpdate.When.Value, item.XmlItem.Payload),
				sync);

			ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, item);

			Assert.AreEqual(MergeOperation.Updated, result.Operation);
			Assert.IsNotNull(result.Proposed);
			Assert.AreEqual("changed", result.Proposed.XmlItem.Title);
			Assert.AreEqual("REMOTE\\kzu", result.Proposed.Sync.LastUpdate.By);
		}

		[TestMethod]
		public void MergeShouldDeleteWithoutConflict()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();

			Sync sync = Behaviors.Create(Guid.NewGuid().ToString(), DeviceAuthor.Current, DateTime.Now, false);
			string id = sync.Id;
			Item item = new Item(
				new XmlItem(sync.Id, "foo", "bar",
					DateTime.Now, GetNavigator("<foo id='bar'/>")),
				sync);

			// Save original item.
			xmlRepo.Add(item.XmlItem);
			syncRepo.Save(item.Sync);

			Thread.Sleep(50);

			// Simulate editing.
			sync = Behaviors.Update(item.Sync, "REMOTE\\kzu", DateTime.Now, true);
			item = new Item(item.XmlItem, sync);

			ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, item);

			Assert.AreEqual(MergeOperation.Deleted, result.Operation);
			Assert.IsNotNull(result.Proposed);
			Assert.AreEqual(true, result.Proposed.Sync.Deleted);
			Assert.AreEqual("REMOTE\\kzu", result.Proposed.Sync.LastUpdate.By);
		}

		[TestMethod]
		public void MergeShouldConflictOnDeleteWithConflict()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();

			Sync localSync = Behaviors.Create(Guid.NewGuid().ToString(), DeviceAuthor.Current, DateTime.Now, false);
			Item localItem = new Item(
				new XmlItem(localSync.Id, "foo", "bar",
					DateTime.Now, GetNavigator("<foo id='bar'/>")),
				localSync);

			// Save original item.
			xmlRepo.Add(localItem.XmlItem);
			syncRepo.Save(localItem.Sync);

			Item remoteItem = localItem.Clone();

			Thread.Sleep(50);

			// Local editing.
			localSync = Behaviors.Update(localItem.Sync, DeviceAuthor.Current, DateTime.Now, false);
			localItem = new Item(new XmlItem(localSync.Id, "changed", localItem.XmlItem.Description,
				localSync.LastUpdate.When.Value, localItem.XmlItem.Payload),
				localSync);
			xmlRepo.Update(localItem.XmlItem);
			syncRepo.Save(localItem.Sync);

			Thread.Sleep(50);

			// Remote editing.
			Sync remoteSync = Behaviors.Update(remoteItem.Sync, "REMOTE\\kzu", DateTime.Now, false);
			remoteSync.Deleted = true;
			remoteItem = new Item(remoteItem.XmlItem, remoteSync);

			// Merge conflicting changed incoming item.
			ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, remoteItem);

			Assert.AreEqual(MergeOperation.Conflict, result.Operation);
			Assert.IsNotNull(result.Proposed);
			Assert.AreEqual(true, result.Proposed.Sync.Deleted);
			Assert.AreEqual("REMOTE\\kzu", result.Proposed.Sync.LastUpdate.By);
		}

		[TestMethod]
		public void MergeShouldNoOpWithNoChanges()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();

			Sync sync = Behaviors.Create(Guid.NewGuid().ToString(), DeviceAuthor.Current, DateTime.Now, false);
			Item item = new Item(
				new XmlItem(sync.Id, "foo", "bar",
					DateTime.Now, GetNavigator("<foo id='bar'/>")),
				sync);

			// Save original item.
			xmlRepo.Add(item.XmlItem);
			syncRepo.Save(item.Sync);

			// Do a merge with the same item.
			ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, item);

			Assert.AreEqual(MergeOperation.None, result.Operation);
			Assert.IsNull(result.Proposed);
		}

		[TestMethod]
		public void MergeShouldNoOpOnUpdatedLocalItemWithUnchangedIncoming()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();

			Sync sync = Behaviors.Create(Guid.NewGuid().ToString(), DeviceAuthor.Current, DateTime.Now, false);
			Item item = new Item(
				new XmlItem(sync.Id, "foo", "bar",
					DateTime.Now, GetNavigator("<foo id='bar'/>")),
				sync);

			// Save original item.
			xmlRepo.Add(item.XmlItem);
			syncRepo.Save(item.Sync);

			Item original = item.Clone();

			Thread.Sleep(50);

			// Simulate editing.
			sync = Behaviors.Update(item.Sync, DeviceAuthor.Current, DateTime.Now, false);
			item = new Item(new XmlItem(sync.Id, "changed", item.XmlItem.Description,
				sync.LastUpdate.When.Value, item.XmlItem.Payload),
				sync);

			// Save the updated local item.
			item.Sync.ItemTimestamp = xmlRepo.Update(item.XmlItem);
			syncRepo.Save(item.Sync);

			// Merge with the older incoming item.
			ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, item);

			Assert.AreEqual(MergeOperation.None, result.Operation);
			Assert.IsNull(result.Proposed);
		}

		[TestMethod]
		public void MergeShouldIncomingWinWithConflict()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();

			Sync localSync = Behaviors.Create(Guid.NewGuid().ToString(), DeviceAuthor.Current, DateTime.Now, false);
			Item localItem = new Item(
				new XmlItem(localSync.Id, "foo", "bar",
					DateTime.Now, GetNavigator("<foo id='bar'/>")),
				localSync);

			// Save original item.
			xmlRepo.Add(localItem.XmlItem);
			syncRepo.Save(localItem.Sync);

			Item remoteItem = localItem.Clone();

			Thread.Sleep(50);

			// Local editing.
			localSync = Behaviors.Update(localItem.Sync, DeviceAuthor.Current, DateTime.Now, false);
			localItem = new Item(new XmlItem(localSync.Id, "changed", localItem.XmlItem.Description,
				localSync.LastUpdate.When.Value, localItem.XmlItem.Payload),
				localSync);
			localSync.ItemTimestamp = xmlRepo.Update(localItem.XmlItem);
			syncRepo.Save(localItem.Sync);

			Thread.Sleep(50);

			// Remote editing.
			Sync remoteSync = Behaviors.Update(remoteItem.Sync, "REMOTE\\kzu", DateTime.Now, false);
			remoteItem = new Item(new XmlItem(localSync.Id, "changed2", localItem.XmlItem.Description,
				remoteSync.LastUpdate.When.Value, localItem.XmlItem.Payload),
				remoteSync);

			// Merge conflicting changed incoming item.
			ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, remoteItem);

			Assert.AreEqual(MergeOperation.Conflict, result.Operation);
			Assert.IsNotNull(result.Proposed);
			// Remote item won
			Assert.AreEqual("REMOTE\\kzu", result.Proposed.Sync.LastUpdate.By);
			Assert.AreEqual(1, result.Proposed.Sync.Conflicts.Count);
			Assert.AreEqual(DeviceAuthor.Current, result.Proposed.Sync.Conflicts[0].Sync.LastUpdate.By);
		}

		[TestMethod]
		public void MergeShouldLocalWinWithConflict()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();

			DateTime now = DateTime.Now.Subtract(TimeSpan.FromMinutes(1));

			Sync localSync = Behaviors.Create(Guid.NewGuid().ToString(), DeviceAuthor.Current, DateTime.Now, false);
			Item localItem = new Item(
				new XmlItem(localSync.Id, "foo", "bar",
					now, GetNavigator("<foo id='bar'/>")),
				localSync);

			// Save original item.
			xmlRepo.Add(localItem.XmlItem);
			syncRepo.Save(localItem.Sync);

			// Remote editing.
			Sync remoteSync = Behaviors.Update(localSync, "REMOTE\\kzu", now.AddSeconds(20), false);
			Item remoteItem = new Item(new XmlItem(localSync.Id, "changed2", localItem.XmlItem.Description,
				remoteSync.LastUpdate.When.Value, localItem.XmlItem.Payload),
				remoteSync);

			// Local editing.
			localSync = Behaviors.Update(localItem.Sync, DeviceAuthor.Current, DateTime.Now, false);
			localItem = new Item(new XmlItem(localSync.Id, "changed", localItem.XmlItem.Description,
				localSync.LastUpdate.When.Value, localItem.XmlItem.Payload),
				localSync);
			localSync.ItemTimestamp = xmlRepo.Update(localItem.XmlItem);
			syncRepo.Save(localItem.Sync);

			// Merge conflicting changed incoming item.
			ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, remoteItem);

			Assert.AreEqual(MergeOperation.Conflict, result.Operation);
			Assert.IsNotNull(result.Proposed);
			// Local item won
			Assert.AreEqual(DeviceAuthor.Current, result.Proposed.Sync.LastUpdate.By);
			Assert.AreEqual(1, result.Proposed.Sync.Conflicts.Count);
			Assert.AreEqual("REMOTE\\kzu", result.Proposed.Sync.Conflicts[0].Sync.LastUpdate.By);
		}

		[TestMethod]
		public void MergeShouldConflictWithDeletedLocalItem()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new MockXmlRepository();

			DateTime now = DateTime.Now.Subtract(TimeSpan.FromMinutes(1));

			Sync localSync = Behaviors.Create(Guid.NewGuid().ToString(), DeviceAuthor.Current, DateTime.Now, false);
			string id = localSync.Id;
			Item localItem = new Item(
				new XmlItem(id, "foo", "bar",
					now, GetNavigator("<foo id='bar'/>")),
				localSync);

			// Save original item.
			localItem.Sync.ItemTimestamp = xmlRepo.Add(localItem.XmlItem);
			syncRepo.Save(localItem.Sync);

			// Delete item from repository from outside of SSE.
			xmlRepo.Remove(id);

			// Remote editing.
			Sync remoteSync = Behaviors.Update(localItem.Sync, "REMOTE\\kzu", now.AddSeconds(1), false);
			Item remoteItem = new Item(
				new XmlItem(id, "changed2", localItem.XmlItem.Description, remoteSync.LastUpdate.When.Value, localItem.XmlItem.Payload),
				remoteSync);

			// Merge conflicting changed incoming item.
			ItemMergeResult result = Behaviors.Merge(xmlRepo, syncRepo, remoteItem);

			Assert.AreEqual(MergeOperation.Conflict, result.Operation);
			Assert.IsNotNull(result.Proposed);
			// Local item won
			Assert.AreEqual(DeviceAuthor.Current, result.Proposed.Sync.LastUpdate.By);
			Assert.AreEqual(1, result.Proposed.Sync.Conflicts.Count);
			Assert.AreEqual("REMOTE\\kzu", result.Proposed.Sync.Conflicts[0].Sync.LastUpdate.By);
			Assert.IsTrue(result.Proposed.Sync.Deleted);
		}

		// TODO:
		// ShouldUpdateItemBeforeMergeIfDifferentUnderlyingTimestamp
		// WinnerPicking missing tests: FirstWinsWithBy and comparison with updates.
		// FirstWinsWithWhen when lastupdate.when is null

		#endregion

		#region Update

		[TestMethod]
		public void UpdateShouldNotModifyArgument()
		{
			Sync expected = Behaviors.Update(new Sync(Guid.NewGuid().ToString()), "foo", null, false);

			Sync updated = Behaviors.Update(expected, "bar", null, false);

			Assert.AreEqual("foo", expected.LastUpdate.By);
			Assert.AreNotEqual(expected, updated);
			Assert.AreEqual("bar", updated.LastUpdate.By);
		}

		[TestMethod]
		public void UpdateShouldIncrementUpdatesByOne()
		{
			Sync sync = new Sync(Guid.NewGuid().ToString());

			int original = sync.Updates;

			Sync updated = Behaviors.Update(sync, "foo", DateTime.Now, false);

			Assert.AreEqual(original + 1, updated.Updates);
		}

		[TestMethod]
		public void UpdateShouldAddTopmostHistory()
		{
			Sync sync = new Sync(Guid.NewGuid().ToString());

			int original = sync.Updates;

			sync = Behaviors.Update(sync, "foo", DateTime.Now, false);
			sync = Behaviors.Update(sync, "bar", DateTime.Now, false);

			Assert.AreEqual("bar", GetFirst<History>(sync.UpdatesHistory).By);
		}

		#endregion

		#region Create

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void CreateShouldThrowExceptionIfIdNull()
		{
			Behaviors.Create(null, DeviceAuthor.Current, DateTime.Now, true);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void CreateShouldThrowExceptionIfIdEmpty()
		{
			Behaviors.Create("", DeviceAuthor.Current, DateTime.Now, true);
		}

		[TestMethod]
		public void CreateShouldNotThrowIfNullByWithWhen()
		{
			Behaviors.Create(Guid.NewGuid().ToString(), null, DateTime.Now, true);
		}

		[TestMethod]
		public void CreateShouldNotThrowIfNullWhenWithBy()
		{
			Behaviors.Create(Guid.NewGuid().ToString(), "foo", null, true);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void CreateShouldThrowIfNullWhenAndBy()
		{
			Behaviors.Create(Guid.NewGuid().ToString(), null, null, true);
		}

		[TestMethod]
		public void CreateShouldReturnSyncWithId()
		{
			Guid id = Guid.NewGuid();
			Sync sync = Behaviors.Create(id.ToString(), DeviceAuthor.Current, DateTime.Now, true);
			Assert.AreEqual(id.ToString(), sync.Id);
		}

		[TestMethod]
		public void CreateShouldReturnSyncWithUpdatesEqualsToOne()
		{
			Guid id = Guid.NewGuid();
			Sync sync = Behaviors.Create(id.ToString(), DeviceAuthor.Current, DateTime.Now, true);
			Assert.AreEqual(1, sync.Updates);
		}

		[TestMethod]
		public void CreateShouldHaveAHistory()
		{
			Guid id = Guid.NewGuid();
			Sync sync = Behaviors.Create(id.ToString(), DeviceAuthor.Current, DateTime.Now, true);
			List<History> histories = new List<History>(sync.UpdatesHistory);
			Assert.AreEqual(1, histories.Count);
		}

		[TestMethod]
		public void CreateShouldHaveHistorySequenceSameAsUpdateCount()
		{
			Guid id = Guid.NewGuid();
			Sync sync = Behaviors.Create(id.ToString(), DeviceAuthor.Current, DateTime.Now, true);
			History history = new List<History>(sync.UpdatesHistory)[0];
			Assert.AreEqual(sync.Updates, history.Sequence);
		}

		[TestMethod]
		public void CreateShouldHaveHistoryWhenEqualsToNow()
		{
			Guid id = Guid.NewGuid();
			DateTime time = DateTime.Now;
			Sync sync = Behaviors.Create(id.ToString(), DeviceAuthor.Current, DateTime.Now, true);
			History history = new List<History>(sync.UpdatesHistory)[0];
			DatesEqualWithoutMillisecond(time, history.When.Value);
		}

		# endregion

		#region Delete

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfSyncNull()
		{
			Behaviors.Delete(null, DeviceAuthor.Current, DateTime.Now);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfByNull()
		{
			Behaviors.Delete(new Sync(Guid.NewGuid().ToString()), null, DateTime.Now);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfWhenParameterNull()
		{
			Behaviors.Delete(new Sync(Guid.NewGuid().ToString()), DeviceAuthor.Current, null);
		}

		[TestMethod]
		public void ShouldIncrementUpdatesByOneOnDeletion()
		{
			Sync sync = new Sync(new Guid().ToString());
			int updates = sync.Updates;
			sync = Behaviors.Delete(sync, DeviceAuthor.Current, DateTime.Now);
			Assert.AreEqual(updates + 1, sync.Updates);
		}

		[TestMethod]
		public void ShouldDeletionAttributeBeTrue()
		{
			Sync sync = new Sync(new Guid().ToString());
			sync = Behaviors.Delete(sync, DeviceAuthor.Current, DateTime.Now);
			Assert.AreEqual(true, sync.Deleted);
		}

		#endregion Delete

		#region ResolveConflicts

		[TestMethod]
		public void ResolveShouldNotUpdateArgument()
		{
			Item item = new Item(
				new XmlItem("foo", "bar", GetNavigator("<payload/>")),
				Behaviors.Create(Guid.NewGuid().ToString(), "one", DateTime.Now, false));

			Item resolved = Behaviors.ResolveConflicts(item, "two", DateTime.Now, false);

			Assert.AreNotSame(item, resolved);
		}

		[TestMethod]
		public void ResolveShouldUpdateEvenIfNoConflicts()
		{
			Item item = new Item(
				new XmlItem("foo", "bar", GetNavigator("<payload/>")),
				Behaviors.Create(Guid.NewGuid().ToString(), "one", DateTime.Now, false));

			Item resolved = Behaviors.ResolveConflicts(item, "two", DateTime.Now, false);

			Assert.AreNotEqual(item, resolved);
			Assert.AreEqual(2, resolved.Sync.Updates);
			Assert.AreEqual("two", resolved.Sync.LastUpdate.By);
		}

		[TestMethod]
		public void ResolveShouldAddConflictItemHistoryWithoutIncrementingUpdates()
		{
			XmlItem xml = new XmlItem("foo", "bar", GetNavigator("<payload/>"));
			Sync sync = Behaviors.Create(Guid.NewGuid().ToString(), "one",
				DateTime.Now.Subtract(TimeSpan.FromMinutes(10)), false);
			Sync conflictSync = Behaviors.Create(sync.Id, "two",
				DateTime.Now.Subtract(TimeSpan.FromHours(1)), false);
			sync.Conflicts.Add(new Item(xml.Clone(), conflictSync));

			Item conflicItem = new Item(xml, sync);
			Item resolvedItem = Behaviors.ResolveConflicts(conflicItem, "one", DateTime.Now, false);

			Assert.AreEqual(2, resolvedItem.Sync.Updates);
			Assert.AreEqual(3, Count(resolvedItem.Sync.UpdatesHistory));
		}

		[TestMethod]
		public void ResolveShouldRemoveConflicts()
		{
			XmlItem xml = new XmlItem("foo", "bar", GetNavigator("<payload/>"));
			Sync sync = Behaviors.Create(Guid.NewGuid().ToString(), "one",
				DateTime.Now.Subtract(TimeSpan.FromMinutes(10)), false);
			Sync conflictSync = Behaviors.Create(sync.Id, "two",
				DateTime.Now.Subtract(TimeSpan.FromHours(1)), false);
			sync.Conflicts.Add(new Item(xml.Clone(), conflictSync));

			Item conflicItem = new Item(xml, sync);
			Item resolvedItem = Behaviors.ResolveConflicts(conflicItem, "one", DateTime.Now, false);

			Assert.AreEqual(0, resolvedItem.Sync.Conflicts.Count);
		}

		[TestMethod]
		public void ResolveShouldNotAddConflictItemHistoryIfSubsumed()
		{
			XmlItem xml = new XmlItem("foo", "bar", GetNavigator("<payload/>"));
			Sync sync = Behaviors.Create(Guid.NewGuid().ToString(), "one",
				DateTime.Now, false);
			Sync conflictSync = sync.Clone();
			// Add subsuming update
			sync = Behaviors.Update(sync, "one", DateTime.Now.AddDays(1), false);

			conflictSync = Behaviors.Update(conflictSync, "two", DateTime.Now.AddMinutes(5), false);

			sync.Conflicts.Add(new Item(xml.Clone(), conflictSync));

			Item conflicItem = new Item(xml, sync);
			Item resolvedItem = Behaviors.ResolveConflicts(conflicItem, "one", DateTime.Now, false);

			Assert.AreEqual(3, resolvedItem.Sync.Updates);
			// there would otherwise be 3 updates to the original item + 2 on the conflict.
			Assert.AreEqual(4, Count(resolvedItem.Sync.UpdatesHistory));
		}

		#endregion

		private static void DatesEqualWithoutMillisecond(DateTime d1, DateTime d2)
		{
			Assert.AreEqual(d1.Year, d2.Year);
			Assert.AreEqual(d1.Month, d2.Month);
			Assert.AreEqual(d1.Date, d2.Date);
			Assert.AreEqual(d1.Hour, d2.Hour);
			Assert.AreEqual(d1.Minute, d2.Minute);
			Assert.AreEqual(d1.Second, d2.Second);
		}
	}
}