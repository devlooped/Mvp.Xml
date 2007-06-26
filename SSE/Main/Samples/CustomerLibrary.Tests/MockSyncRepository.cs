using System;
using System.Collections.Generic;
using System.Text;
using Mvp.Xml.Synchronization;

namespace CustomerLibrary.Tests
{
	public class MockSyncRepository : ISyncRepository
	{
		Dictionary<string, Sync> syncs = new Dictionary<string, Sync>();
		Dictionary<string, DateTime> lastSync = new Dictionary<string, DateTime>();

		public Sync Get(string id)
		{
			if (!syncs.ContainsKey(id))
				return null;

			return syncs[id].Clone();
		}

		public void Save(Sync sync)
		{
			syncs[sync.Id] = sync.Clone();
		}

		public DateTime? GetLastSync(string feed)
		{
			if (!lastSync.ContainsKey(feed))
				return null;

			return lastSync[feed];
		}

		public void SetLastSync(string feed, DateTime date)
		{
			lastSync[feed] = date;
		}

		public IEnumerable<Sync> GetAll()
		{
			Sync[] values = new Sync[syncs.Count];
			syncs.Values.CopyTo(values, 0);

			return values;
		}

		public IEnumerable<Sync> GetConflicts()
		{
			foreach (Sync sync in syncs.Values)
			{
				if (sync.Conflicts.Count > 0)
					yield return sync;
			}
		}
	}
}
