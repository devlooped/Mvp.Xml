using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace Mvp.Xml.Synchronization
{
	public class SyncEngine
	{
		IXmlRepository xmlRepo;
		ISyncRepository syncRepo;

		public SyncEngine(
			IXmlRepository xmlRepository,
			ISyncRepository syncRepository)
		{
			Guard.ArgumentNotNull(xmlRepository, "xmlRepository");
			Guard.ArgumentNotNull(syncRepository, "syncRepository");

			this.xmlRepo = xmlRepository;
			this.syncRepo = syncRepository;
		}

		public IEnumerable<Item> Export()
		{
			return BuildItems(xmlRepo.GetAll());
		}

		public IEnumerable<Item> Export(int days)
		{
			return BuildItems(xmlRepo.GetAllSince(
				DateTime.Today.Subtract(TimeSpan.FromDays(days))));
		}

		private IEnumerable<Item> BuildItems(IEnumerable<IXmlItem> xmlItems)
		{
			foreach (IXmlItem xml in xmlItems)
			{
				Sync sync = syncRepo.Get(xml.Id);

				if (sync == null)
				{
					// Add sync on-the-fly.
					sync = Behaviors.Create(xml.Id, DeviceAuthor.Current, xml.Timestamp, false);
					sync.ItemTimestamp = xml.Timestamp;
					syncRepo.Save(sync);
				}
				else
				{
					sync = SynchronizeSyncFromItem(xml, sync);
				}

				yield return new Item(xml, sync);
			}

			// Search deleted items.
			// TODO: Is there a better way than iterating every sync?
			foreach (Sync sync in syncRepo.GetAll())
			{
				if (!xmlRepo.Contains(sync.Id) && !sync.Deleted)
				{
					Sync updatedSync = Behaviors.Update(sync, DeviceAuthor.Current, DateTime.Now, true);
					syncRepo.Save(updatedSync);

					yield return new Item(null, updatedSync);
				}
			}
		}

		public IEnumerable<Item> ExportConflicts()
		{
			foreach (Sync sync in syncRepo.GetConflicts())
			{
				IXmlItem item = xmlRepo.Get(sync.Id);
				Sync itemSync = sync;
				if (item == null)
				{
					// Update deletion if necessary.
					if (!sync.Deleted)
					{
						itemSync = Behaviors.Update(sync, DeviceAuthor.Current, DateTime.Now, true);
						syncRepo.Save(itemSync);
					}
				}
				else
				{
					itemSync = SynchronizeSyncFromItem(item, sync);
				}
				
				yield return new Item(item, itemSync);
			}
		}

		public IEnumerable<ItemMergeResult> PreviewImport(IEnumerable<Item> items)
		{
			foreach (Item incoming in items)
			{
				yield return Behaviors.Merge(xmlRepo, syncRepo, incoming);
			}
		}

		public IList<Item> Import(string feed, IEnumerable<ItemMergeResult> items)
		{
			// Straight import of data in merged results. 
			// Conflicting items are saved and also 
			// are returned for conflict resolution by the user or 
			// a custom component. MergeBehavior determines 
			// the winner element that is saved.
			// Conflicts are returned in a list because we need 
			// the full iteration over the merged items to be 
			// processed. If we returned an IEnumerable, we would 
			// depend on the client iterating it in order to 
			// actually import items, which is undesirable.

			List<Item> conflicts = new List<Item>();
			bool updateSync = false;

			foreach (ItemMergeResult result in items)
			{
				SynchronizeItemFromSync(result.Proposed);

				if (result.Operation != MergeOperation.None &&
					result.Proposed != null &&
					result.Proposed.Sync.Conflicts.Count > 0)
				{
					conflicts.Add(result.Proposed);
				}

				switch (result.Operation)
				{
					case MergeOperation.Added:
						if (!result.Proposed.Sync.Deleted)
						{
							result.Proposed.Sync.ItemTimestamp = xmlRepo.Add(result.Proposed.XmlItem);
							syncRepo.Save(result.Proposed.Sync);
							updateSync = true;
						}
						break;
					case MergeOperation.Deleted:
						xmlRepo.Remove(result.Proposed.XmlItem.Id);
						syncRepo.Save(result.Proposed.Sync);
						updateSync = true;
						break;
					case MergeOperation.Updated:
					case MergeOperation.Conflict:
						result.Proposed.Sync.ItemTimestamp = xmlRepo.Update(result.Proposed.XmlItem);
						syncRepo.Save(result.Proposed.Sync);
						updateSync = true;
						break;
					case MergeOperation.None:
						break;
					default:
						throw new InvalidOperationException();
				}
			}

			if (updateSync)
				syncRepo.SetLastSync(feed, DateTime.Now);

			return conflicts;
		}

		public IList<Item> Import(string feed, IEnumerable<Item> items)
		{
			return Import(feed, PreviewImport(items));
		}

		public IList<Item> Import(string feed, params Item[] items)
		{
			return Import(feed, PreviewImport(items));
		}

		public Item SaveClearConflicts(Item resolvedItem)
		{
			Item item = Behaviors.ResolveConflicts(resolvedItem, DeviceAuthor.Current, DateTime.Now, false);

			resolvedItem.Sync.ItemTimestamp = xmlRepo.Update(resolvedItem.XmlItem);
			syncRepo.Save(resolvedItem.Sync);

			return item;
		}

		public void Publish(Feed feed, FeedWriter writer)
		{
			// Update Feed.Sharing
			feed.Sharing.Since = Timestamp.ToString(xmlRepo.GetFirstUpdated());
			feed.Sharing.Until = Timestamp.ToString(xmlRepo.GetLastUpdated());

			IEnumerable<Item> items = Export();
			writer.Write(feed, items);
		}

		// Partial feed publishing
		public void Publish(Feed feed, FeedWriter writer, int lastDays)
		{
			DateTime since = DateTime.Today.Subtract(TimeSpan.FromDays(lastDays));

			// Update Feed.Sharing
			feed.Sharing.Since = Timestamp.ToString(xmlRepo.GetFirstUpdated(since));
			feed.Sharing.Until = Timestamp.ToString(xmlRepo.GetLastUpdated());

			IEnumerable<Item> items = Export(lastDays);
			writer.Write(feed, items);
		}

		// TODO: Optimize subscribe when caller doesn't care about 
		// retrieving the conflicts.
		public IList<Item> Subscribe(IFeedReader reader)
		{
			Feed feed;
			IEnumerable<Item> items;

			// TODO: 5.1
			reader.Read(out feed, out items);

			return Import(feed.Link, items);
		}

		public DateTime? GetLastSync(string feed)
		{
			return syncRepo.GetLastSync(feed);
		}

		/// <summary>
		/// Ensures the Sync information is current WRT the 
		/// item actual LastUpdated date. If it's not, a new 
		/// update will be added. Used when exporting/retrieving 
		/// items from the local stores.
		/// </summary>
		private Sync SynchronizeSyncFromItem(IXmlItem item, Sync sync)
		{
			if (item.Timestamp > sync.ItemTimestamp)
			{
				Sync updated = Behaviors.Update(sync,
					DeviceAuthor.Current,
					item.Timestamp, sync.Deleted);
				sync.ItemTimestamp = item.Timestamp;
				syncRepo.Save(sync);
				return updated;
			}

			return sync;
		}

		/// <summary>
		/// Ensures the LastUpdate property on the <see cref="IXmlItem"/> 
		/// matches the Sync last update. This is the opposite of 
		/// SynchronizeSyncFromItem, and is used for incoming items
		/// being imported.
		/// </summary>
		private void SynchronizeItemFromSync(Item item)
		{
			if (item != null &&
				item.XmlItem != null && 
				item.Sync.LastUpdate != null && 
				item.Sync.LastUpdate.When != null)
			{
				item.XmlItem.Timestamp = item.Sync.LastUpdate.When.Value;
			}
		}
	}
}
