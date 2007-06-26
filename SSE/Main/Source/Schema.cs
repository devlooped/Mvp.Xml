using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp.Xml.Synchronization
{
	public static class Schema
	{
		/// <summary>
		/// Version of the SSE specification implemented in this library.
		/// </summary>
		public const string SpecificationVersion = "0.93";

		/// <summary>
		/// Namespace of the SSE elements.
		/// </summary>
		public const string Namespace = "http://www.microsoft.com/schemas/sse";

		/// <summary>
		/// Default prefix used for SSE elements.
		/// </summary>
		public const string DefaultPrefix = "sx";

		public static class ElementNames
		{
			public const string Sharing = "sharing";
			public const string Related = "related";
			public const string Sync = "sync";
			public const string History = "history";
			public const string Conflicts = "conflicts";
		}

		public static class AttributeNames
		{
			// sx:sharing
			public const string Since = "since";
			public const string Until = "until";
			public const string Version = "version";
			public const string Expires = "expires";
			// sx:related
			public const string Link = "link";
			public const string Title = "title";
			public const string Type = "type";
			// sx:sync
			public const string Id = "id";
			public const string Updates = "updates";
			public const string Deleted = "deleted";
			public const string NoConflicts = "noconflicts";
			// sx:history
			public const string Sequence = "sequence";
			public const string When = "when";
			public const string By = "by";
		}
	}
}
