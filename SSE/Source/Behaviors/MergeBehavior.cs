using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Synchronization
{
	/// <summary>
	///3.3	Merge Behavior
	///When a subscribing endpoint incorporates items from a publishing endpoint’s feed, these items must be merged with the existing local items. The act of merging items from an incoming feed detects new items, item updates and item conflicts and produces a merged result feed.  The merging of two items with the same id attribute value will result in a ‘winning’ item that MAY have conflict items.  In order to merge items, implementations MUST follow this algorithm for the two items:
	///1.	If no local item exists with the same id attribute value as the incoming item, add the incoming item to the merge result feed; we are done processing the incoming item.
	///2.	Create a collection L and populate it with the local item and the local item’s conflicts (if any exist) by using the following steps:
	///a.	For each item sub-element of the sx:conflicts element for the local item:
	///i.	Add the item sub-element to L
	///b.	If the local item has a sx:conflicts sub-element, remove it
	///c.	Add the local item to L
	///3.	Create a collection I and populate it with the incoming item and the incoming item’s conflicts (if any exist) by using the following steps:
	///a.	For each item sub-element of the sx:conflicts element for the incoming item:
	///i.	Add the item sub-element to I
	///b.	If the incoming item has a sx:conflicts sub-element, remove it
	///c.	Add the incoming item to I
	///4.	Create a collection M that will be used to contain items that will appear in the merged result feed
	///5.	Create a reference W for the current ‘winning’ item and set it to an unassigned value
	///6.	Using L as the outer collection and I as the inner collection, perform the following step
	///7.	For each item X in outer collection:
	///a.	For each item Y in inner collection:
	///i.	Determine if X is subsumed1 by Y – if so then remove X from the outer collection; process the next item in the outer collection
	///b.	Add X to M
	///c.	If W has not been assigned a value, set W to X; process the next item in the outer collection 
	///d.	Determine if X should be declared as the new ‘winning’ item3 – if so set W to X.
	///8.	Using I as the outer collection and L as the inner collection, perform step 7 again
	///9.	Add W to the merge result feed
	///10.	If the noconflicts attribute is set to true, then we are done processing
	///11.	If M contains more than one item:
	///a.	Create a sx:conflicts element and add it as a sub-element of the sx:sync element for W
	///b.	For each item Z in M:
	///i.	If Z equals W (i.e. they are the same item), then process the next item in M
	///ii.	Add Z as a sub-element of the sx:conflicts element created in step 11a.
	/// </summary>
	internal class MergeBehavior
	{
		ISyncRepository syncRepository;
		IXmlRepository xmlRepository;

		public MergeBehavior(IXmlRepository xmlRepository, ISyncRepository syncRepository)
		{
			Guard.ArgumentNotNull(syncRepository, "syncRepository");
			Guard.ArgumentNotNull(xmlRepository, "xmlRepository");

			this.syncRepository = syncRepository;
			this.xmlRepository = xmlRepository;
		}

		public ItemMergeResult Execute(Item incomingItem)
		{
			Guard.ArgumentNotNull(incomingItem, "incomingItem");
			Guard.ArgumentNotNull(incomingItem.Sync, "incomingItem.Sync");
			Guard.ArgumentNotNull(incomingItem.XmlItem, "incomingItem.XmlItem");

			// Item IDs match and are globally unique. They 
			// are the same as the Sync.Id, and if other ID 
			// mechanism is used by the local store, it 
			// can provide its own mapper inside its 
			// IXmlRepository implementation.

			string id = incomingItem.XmlItem.Id;
			IXmlItem localItem = xmlRepository.Get(id);
			Sync sync = null;

			if (localItem == null)
			{
				// If it was a deletion already synchronized.
				sync = syncRepository.Get(id);
				if (sync != null)
				{
					if (!sync.Deleted)
					{
						sync = Behaviors.Update(sync, DeviceAuthor.Current, DateTime.Now, true);
						syncRepository.Save(sync);
					}
				}
				else
				{
					//3.3.1
					return new ItemMergeResult(null, incomingItem, incomingItem, MergeOperation.Added);
				}
			}
			else
			{
				sync = syncRepository.Get(id);
			}

			// See if Sync info should be sync'ed for local edits.
			if (sync == null)
			{
				sync = Behaviors.Create(id, DeviceAuthor.Current, DateTime.Now, false);
				sync.ItemTimestamp = localItem.Timestamp;

				syncRepository.Save(sync);
			}
			else if (localItem != null &&
				sync.ItemTimestamp < localItem.Timestamp)
			{
				sync = Behaviors.Update(sync, DeviceAuthor.Current, localItem.Timestamp, false);
				sync.ItemTimestamp = localItem.Timestamp;
				syncRepository.Save(sync);
			}

			Item original = new Item(localItem, sync);
			Item incoming = incomingItem.Clone();

			// History on both elements must have at least one entry
			if (original.Sync.LastUpdate == null ||
				incoming.Sync.LastUpdate == null)
			{
				throw new ArgumentException(Properties.Resources.SyncHistoryRequired);
			}

			// Ensure LastUpdated on IXmlItem matches LastUpdate on Sync.
			// XmlItem will be null if item is deleted. Only Sync info will be available.
			if (original.XmlItem != null)
				original.XmlItem.Timestamp = original.Sync.LastUpdate.When.Value;
			if (incoming.XmlItem != null)
				incoming.XmlItem.Timestamp = incoming.Sync.LastUpdate.When.Value;

			Item proposed;
			MergeOperation operation = MergeItems(original, incoming, out proposed);

			if (proposed != null && proposed.Equals(original) && operation != MergeOperation.Conflict)
			{
				return new ItemMergeResult(original, incoming, null, MergeOperation.None);
			}
			else
			{
				return new ItemMergeResult(original, incoming, proposed, operation);
			}
		}


		private MergeOperation MergeItems(Item localItem, Item incomingItem, out Item proposedItem)
		{
			proposedItem = null;

			//3.3.2
			List<Item> L = new List<Item>();
			L.AddRange(localItem.Sync.Conflicts);
			localItem.Sync.Conflicts.Clear();
			L.Add(localItem);
			

			//3.3.3
			List<Item> I = new List<Item>();
			I.AddRange(incomingItem.Sync.Conflicts);
			incomingItem.Sync.Conflicts.Clear();
			I.Add(incomingItem);

			//3.3.4
			List<Item> M = new List<Item>();

			//3.3.5
			Item W = null;

			//3.3.6 and 3.3.7
			PerformStep7(ref L, ref I, M, ref W);
			//3.3.8
			PerformStep7(ref I, ref L, M, ref W);

			if (W == null)
			{
				//There is no need to update the local item
				return MergeOperation.None;
			}

			proposedItem = W;

			//3.3.10
			if (!W.Sync.NoConflicts)
			{
				//3.3.11
				foreach (Item z in M)
				{
					if (!z.Equals(W) && !W.Sync.Conflicts.Contains(z))
					{
						W.Sync.Conflicts.Add(z);
					}
				}
			}

			if (W.Sync.Conflicts.Count > 0)
			{
				return MergeOperation.Conflict;
			}
			else if (W.Sync.Deleted)
			{
				return MergeOperation.Deleted;
			}
			else
			{
				return MergeOperation.Updated;
			}
		}

		/// <summary>
		/// 3.3.7 implementation
		/// </summary>
		/// <param name="outerCollection"></param>
		/// <param name="innerCollection"></param>
		/// <param name="M"></param>
		/// <param name="W"></param>
		private void PerformStep7(ref List<Item> outerCollection, ref List<Item> innerCollection, List<Item> M, ref Item W)
		{
			List<Item> resOuter = new List<Item>(outerCollection);
			List<Item> resInner = new List<Item>(innerCollection);
			
			// Collections must be modified from this method.
			foreach (Item x in outerCollection)
			{
				bool isSubsumed = false;
				foreach (Item y in innerCollection)
				{
					if (x.IsSubsumedBy(y))
					{
						isSubsumed = true;
						resOuter.Remove(x);
						break;
					}
				}

				if (!isSubsumed)
				{
					M.Add(x);
					if (W == null)
					{
						W = x;
					}
					else
					{
						W = WinnerPicking(W, x);
					}
				}
			}

			outerCollection = resOuter;
			innerCollection = resInner;
		}

		/// <summary>
		///Winner Picking
		///The ‘winning’ item between an item X and an item Y is the item with most recent update , where X and Y are items with the same id attribute value.  In order to determine the ‘winning’ item, implementations MUST perform the following comparisons, in order, for X and Y:
		///1.	If X has a greater updates attribute value for the sx:sync sub-element than Y’s, then X is the ‘winning’ item
		///2.	If X has the same updates attribute value for the sx:sync sub-element as Y:
		///a.	If X has a when attribute for the topmost sx:history sub-element and Y does not, then X is the ‘winning’ item
		///b.	If X has a when attribute value for the topmost sx:history sub-element and that is chronologically later than Y’s, then X is the ‘winning’ item
		///c.	If X has the same when attribute for the topmost sx:history sub-element as Y:
		///i.	If X has a by attribute for the topmost sx:history sub-element and Y does not, then X is the ‘winning’ item 
		///ii.	If X has a by attribute value for the topmost sx:history sub-element that is collates greater (see Section 2.4 for collation rules) than Y’s, then X is the ‘winning’ item
		///3.	Y is the ‘winning’ item
		/// </summary>
		/// <param name="item"></param>
		/// <param name="anotherItem"></param>
		/// <returns></returns>
		private Item WinnerPicking(Item item, Item anotherItem)
		{
			Item winner = null;

			if (item.Sync.Updates == anotherItem.Sync.Updates)
			{
				if (!FirstWinsWithWhen(item, anotherItem, out winner))
				{
					FirstWinsWithWhen(anotherItem, item, out winner);
				}

				if (winner == null && !FirstWinsWithBy(item, anotherItem, out winner))
				{
					FirstWinsWithBy(anotherItem, item, out winner);
				}
			}
			else
			{
				winner = item.Sync.Updates > anotherItem.Sync.Updates ? item : anotherItem;
			}

			return winner;
		}

		private bool FirstWinsWithWhen(Item first, Item second, out Item winner)
		{
			winner = null;

			if (first.Sync.LastUpdate.When == null) return false;

			bool firstWins = second.Sync.LastUpdate.When == null ||
				   (first.Sync.LastUpdate.When > second.Sync.LastUpdate.When);

			if (firstWins) winner = first;

			return firstWins;
		}

		private bool FirstWinsWithBy(Item first, Item second, out Item winner)
		{
			winner = null;

			if (first.Sync.LastUpdate.By == null) return false;

			bool firstWins = second.Sync.LastUpdate.By == null ||
				   (second.Sync.LastUpdate.By != null &&
					!first.Sync.LastUpdate.By.Equals(second.Sync.LastUpdate.By) &&
					first.Sync.LastUpdate.By.Length > second.Sync.LastUpdate.By.Length
					);

			if (firstWins) winner = first;

			return firstWins;
		}
	}
}
