using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;

namespace Mvp.Xml.Synchronization
{
	public class HttpSync
	{
		SyncEngine engine;

		public HttpSync(SyncEngine engine)
		{
			this.engine = engine;
		}

		public IList<Item> Synchronize(Feed localFeed, string remoteUrl)
		{
			WebRequest req = WebRequest.Create(remoteUrl);
#if DEBUG
			req.Timeout = -1;
#endif
			
			req.Method = "POST";
			XmlWriterSettings set = new XmlWriterSettings();
			set.CloseOutput = true;
			using (XmlWriter w = XmlWriter.Create(req.GetRequestStream(), set))
			{
				engine.Publish(localFeed, new RssFeedWriter(w));
			}

			WebResponse resp = req.GetResponse();

			using (XmlReader r = XmlReader.Create(resp.GetResponseStream()))
			{
				return engine.Subscribe(new RssFeedReader(r));
			}
		}
	}
}
