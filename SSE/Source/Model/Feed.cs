using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Synchronization
{
	[Serializable]
	public class Feed
	{
		private Sharing sharing = new Sharing();

		private string title;
		private string description;
		private string link;

		// TODO: unpublished. 2.6 and 4.1. Add Unpublish(Item) ?

		public Feed(string title, string linkUrl, string description)
		{
			Guard.ArgumentNotNullOrEmptyString(title, "title");
			Guard.ArgumentNotNullOrEmptyString(linkUrl, "linkUrl");
			Guard.ArgumentNotNullOrEmptyString(description, "description");

			this.title = title;
			this.link = linkUrl;
			this.description = description;
		}

		public Sharing Sharing
		{
			get { return sharing; }
			set { sharing = value; }
		}

		public string Description
		{
			get { return description; }
		}

		public string Link
		{
			get { return link; }
		}

		public string Title
		{
			get { return title; }
		}
	}
}
