using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Synchronization
{
	public sealed class Behaviors
	{
		public static Sync Create(string id, string by, DateTime? when, bool deleteItem)
		{
			Guard.ArgumentNotNullOrEmptyString(id, "id");
			if (by == null && when == null)
				throw new ArgumentException(Properties.Resources.MustProvideWhenOrBy);

			return Behaviors.Update(new Sync(id), by, when, deleteItem);
		}

		public static Sync Delete(Sync sync, string by, DateTime? when)
		{
			Guard.ArgumentNotNull(sync, "sync");
			Guard.ArgumentNotNullOrEmptyString(by, "by");
			Guard.ArgumentNotNull(when, "when");
			
			//Deleted attribute set to true because it is a deletion (3.2.4 from spec)
			return Update(sync, by, when, true);
		}

		public static ItemMergeResult Merge(IXmlRepository xmlRepository, ISyncRepository syncRepository, Item incomingItem)
		{
			Guard.ArgumentNotNull(xmlRepository, "xmlRepository");
			Guard.ArgumentNotNull(syncRepository, "syncRepository");
			Guard.ArgumentNotNull(incomingItem, "incomingItem");

			return new MergeBehavior(xmlRepository, syncRepository).Execute(incomingItem);
		}

		// 3.2
		public static Sync Update(Sync sync, string by, DateTime? when, bool deleteItem)
		{
			Sync updated = sync.Clone();

			// 3.2.1
			updated.Updates++;

			// 3.2.2 & 3.2.2.a.i
			History history = new History(by, when, updated.Updates);

			// 3.2.3
			updated.AddHistory(history);

			// 3.2.4
			updated.Deleted = deleteItem;

			// TODO: optional truncation behavior 
			// for deletions, and prune of old history for 
			// same By.

			return updated;
		}

		// 3.4
		public static Item ResolveConflicts(Item resolvedItem, string by, DateTime? when, bool deleteItem)
		{
			//3.4	Conflict Resolution Behavior
			//Merging Conflict Items 
			//1.	Set R as a reference the resolved item
			//2.	Set Ry as a reference to the sx:sync sub-element for R
			//3.	For each item sub-element C of the sx:conflicts element that has been resolved:
			//a.	Set Sc as a reference to the sx:sync sub-element for C
			//b.	Remove C from the sx:conflicts element.
			//b.	For each sx:history sub-element Hc of Sc:
			//i.	For each sx:history sub-element Hr of Sr:
			//aa.	Compare Hc with Hr to see if Hc can be subsumed2 by Hr – if so then process the next item sub-element
			//ii.	Add Hr as a sub-element of Sr, immediately after the topmost sx:history sub-element of Sr.
			//3. If the sx:conflicts element contains no sub-elements, the sx:conflicts element SHOULD be removed.

			Item R = resolvedItem.Clone();
			Sync Sr = R.Sync;
			foreach (Item C in Sr.Conflicts.ToArray())
			{
				Sync Sc = C.Sync;
				Sr.Conflicts.Remove(C);
				foreach (History Hc in Sc.UpdatesHistory)
				{
					bool isSubsumed = false;
					foreach (History Hr in Sr.UpdatesHistory)
					{
						if (Hc.IsSubsumedBy(Hr))
						{
							isSubsumed = true;
							break;
						}
					}
					if (isSubsumed)
					{
						break;
					}
					else
					{
						Sr.AddConflictHistory(Hc);
					}
				}
			}

			Sync updatedSync = Update(Sr, by, when, deleteItem);

			return new Item(R.XmlItem, updatedSync);
		}
	}
}
