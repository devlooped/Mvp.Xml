using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Synchronization
{
	[Serializable]
	public class History : ICloneable<History>, IEquatable<History>
	{
		private DateTime? when;
		private string by;
		private int sequence;

		public History(string by)
			: this(by, null, 1)
		{
		}

		public History(DateTime? when)
			: this(null, when, 1)
		{
		}

		public History(string by, DateTime? when)
			: this(by, when, 1)
		{
		}

		public History(string by, DateTime? when, int sequence)
		{
			if (String.IsNullOrEmpty(by) && when == null)
				throw new ArgumentException(Properties.Resources.Arg_EitherWhenOrByMustBeSpecified);
			if (sequence <= 0)
				throw new ArgumentException(Properties.Resources.Arg_SequenceMustBeGreaterThanZero);

			this.by = by;
			if (when != null)
			{
				this.when = Timestamp.Normalize(when.Value);
			}
			this.sequence = sequence;
		}

		public string By
		{
			get { return by; }
		}

		public DateTime? When
		{
			get { return when; }
		}

		public int Sequence
		{
			get { return sequence; }
		}

		#region Equality

		public static bool operator ==(History h1, History h2)
		{
			return History.Equals(h1, h2);
		}

		public static bool operator !=(History h1, History h2)
		{
			return !History.Equals(h1, h2);
		}

		public bool Equals(History other)
		{
			return History.Equals(this, other);
		}

		public override bool Equals(object obj)
		{
			return History.Equals(this, obj as History);
		}

		public static bool Equals(History h1, History h2)
		{
			if (Object.ReferenceEquals(h1, h2)) return true;
			if (!Object.Equals(null, h1) && !Object.Equals(null, h2))
			{
				return h1.by == h2.by && 
					h1.when == h2.when &&
					h1.sequence == h2.sequence;
			}

			return false;
		}

		public override int GetHashCode()
		{
			int hash = ((String.IsNullOrEmpty(by)) ? 0 : this.by.GetHashCode());
			hash = hash ^ ((this.when == null) ? 0 : this.when.GetHashCode());
			hash = hash ^ sequence.GetHashCode();

			return hash;
		}

		#endregion

		#region ICloneable<History> Members

		public History Clone()
		{
			return (History)DoClone();
		}

		object ICloneable.Clone()
		{
			return DoClone();
		}

		protected virtual object DoClone()
		{
			return new History(by, when, sequence);
		}

		#endregion

		public bool IsSubsumedBy(History history)
		{
			History Hx = this;
			History Hy = history;

			if (!String.IsNullOrEmpty(Hx.By))
			{
				return Hx.By == Hy.By &&
					Hy.Sequence >= Hx.Sequence;
			}
			else if (String.IsNullOrEmpty(Hy.By))
			{
				return Hx.When == Hy.When &&
					Hx.Sequence == Hy.Sequence;
			}

			return false;
		}
	}
}
