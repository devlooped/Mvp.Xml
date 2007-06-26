using System;
using System.Collections.Generic;

namespace Mvp.Xml.Synchronization
{
	[Serializable]
	public class Sync : ICloneable<Sync>, IEquatable<Sync>
	{
		private string id;
		private bool deleted = false;
		private int updates = 0;
		private bool noConflicts = false;
		private ComparableStack<History> updatesHistory = new ComparableStack<History>();
		private ComparableList<Item> conflicts = new ComparableList<Item>();
		private DateTime? itemTimestamp;

		public Sync(string id, int updates)
		{
			Guard.ArgumentNotNullOrEmptyString(id, "id");
			this.id = id;
			this.updates = updates;
		}

		public Sync(string id) : this(id, 0)
		{
		}

		public string Id
		{
			get { return id; }
		}

		public int Updates
		{
			get { return updates; }
			set { updates = value; }
		}

		public bool Deleted
		{
			get { return deleted; }
			set { deleted = value; }
		}

		public bool NoConflicts
		{
			get { return noConflicts; }
			set { noConflicts = value; }
		}

		public History LastUpdate
		{
			get { return updatesHistory.Count > 0 ? updatesHistory.Peek() : null; }
		}

		public IEnumerable<History> UpdatesHistory
		{
			get { return updatesHistory; }
		}

		public List<Item> Conflicts
		{
			get { return conflicts; }
		}

		/// <summary>
		/// Timestamp of the item when the last update was performed.
		/// </summary>
		public DateTime? ItemTimestamp
		{
			get { return itemTimestamp; }
			set { itemTimestamp = value; }
		}

		public void AddHistory(History history)
		{
			updatesHistory.Push(history);
		}

		/// <summary>
		/// Adds the conflict history immediately after the topmost history.
		/// </summary>
		/// <remarks>Used for conflict resolution only.</remarks>
		public void AddConflictHistory(History history)
		{
			History topmost = updatesHistory.Pop();
			updatesHistory.Push(history);
			updatesHistory.Push(topmost);
		}

		#region ICloneable<Sync> Members

		public Sync Clone()
		{
			Sync newSync = new Sync(this.id, this.updates);
			newSync.deleted = this.deleted;

			List<History> newHistory = new List<History>(updatesHistory);
			newHistory.Reverse();
			foreach (History history in newHistory)
			{
				newSync.updatesHistory.Push(history.Clone());
			}

			foreach (Item conflict in this.conflicts)
			{
				newSync.Conflicts.Add(conflict.Clone());
			}

			newSync.itemTimestamp = this.itemTimestamp;

			return newSync;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion

		#region Equality

		public static bool Equals(Sync s1, Sync s2)
		{
			if (Object.ReferenceEquals(s1, s2)) return true;
			if (!Object.Equals(null, s1) && !Object.Equals(null, s2))
			{
				return s1.id == s2.id &&
					s1.updates == s2.updates &&
					s1.deleted == s2.deleted &&
					s1.noConflicts == s2.noConflicts &&
					s1.updatesHistory == s2.updatesHistory;				
			}

			return false;
		}

		public bool Equals(Sync sync)
		{
			return Sync.Equals(this, sync);
		}

		public override bool Equals(object obj)
		{
			return Sync.Equals(this, obj as Sync);
		}

		public override int GetHashCode()
		{
			int hash = 0;
			hash = hash ^ this.id.GetHashCode();
			hash = hash ^ this.updates.GetHashCode();
			hash = hash ^ this.deleted.GetHashCode();
			hash = hash ^ this.noConflicts.GetHashCode();
			hash = hash ^ this.updatesHistory.GetHashCode();
			//hash = hash ^ this.conflicts.GetHashCode();

			return hash;
		}

		/// <summary>Determines whether two specified instances of <see cref="Sync"></see> are equal.</summary>
		/// <returns><see langword="true"/> if s1 and s2 represent the same sync information; <see langword="false"/> otherwise.</returns>
		/// <param name="s2">A <see cref="Sync"></see>.</param>
		/// <param name="s1">A <see cref="Sync"></see>.</param>
		public static bool operator ==(Sync s1, Sync s2)
		{
			return Sync.Equals(s1, s2);
		}

		/// <summary>Determines whether two specified instances of <see cref="Sync"></see> are not equal.</summary>
		/// <returns><see langword="false"/> if s1 and s2 represent the same sync information; <see langword="true"/> otherwise.</returns>
		/// <param name="s2">A <see cref="Sync"></see>.</param>
		/// <param name="s1">A <see cref="Sync"></see>.</param>
		public static bool operator !=(Sync s1, Sync s2)
		{
			return !Sync.Equals(s1, s2);
		}

		#endregion
	}
}
