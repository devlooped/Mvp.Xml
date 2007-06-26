using System;
using System.Collections.Generic;

namespace Mvp.Xml.Synchronization
{
	public interface IFeedReader
	{
		event EventHandler ItemRead;
		void Read(out Feed feed, out IEnumerable<Item> items);
	}
}
