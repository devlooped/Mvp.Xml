using System;

namespace Mvp.Xml.Synchronization
{
	[Serializable]
	public class Related
	{
		private string link;
		private string title;
		private RelatedType type;

		public Related(string linkUrl, RelatedType type)
			: this(linkUrl, type, null)
		{
		}

		public Related(string linkUrl, RelatedType type, string title)
		{
			Guard.ArgumentNotNullOrEmptyString(linkUrl, "linkUrl");

			this.link = linkUrl;
			this.type = type;
			this.title = title;
		}

		public string Link
		{
			get { return link; }
		}

		public string Title
		{
			get { return title; }
		}

		public RelatedType Type
		{
			get { return type; }
		}
	}
}
