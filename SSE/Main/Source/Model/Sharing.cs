using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Mvp.Xml.Synchronization
{
	[Serializable]
	public class Sharing
	{
		private string since;
		private string until;
		private DateTime? expires;

		public DateTime? Expires
		{
			get { return expires; }
			set 
			{
				if (value != null)
				{
					expires = Timestamp.Normalize(value.Value);
				}
				else
				{
					expires = value;
				}
			}
		}

		/// <summary>
		/// Typically, a date time in a normalized string form.
		/// </summary>
		public string Since
		{
			get { return since; }
			set { since = value; }
		}

		/// <summary>
		/// Typically, a date time in a normalized string form.
		/// </summary>
		public string Until
		{
			get { return until; }
			set { until = value; }
		}

		private List<Related> related = new List<Related>();

		public List<Related> Related
		{
			get { return related; }
			set { related = value; }
		}
	}
}
