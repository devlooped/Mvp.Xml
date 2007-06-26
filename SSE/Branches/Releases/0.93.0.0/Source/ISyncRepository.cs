using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Synchronization
{
	public interface ISyncRepository
	{
		Sync Get(string id);
		void Save(Sync sync);

		DateTime? GetLastSync(string feed);
		void SetLastSync(string feed, DateTime date);

		IEnumerable<Sync> GetAll();
		IEnumerable<Sync> GetConflicts();
	}
}
