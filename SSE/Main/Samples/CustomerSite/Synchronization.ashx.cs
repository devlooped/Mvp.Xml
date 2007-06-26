using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using Mvp.Xml.Synchronization;
using System.Xml;
using System.Collections.Generic;
using CustomerLibrary;
using System.IO;
using System.Data.Common;
using System.Configuration;

namespace CustomerSite
{
	[WebService(Namespace = Schema.Namespace)]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class Synchronization : IHttpHandler
	{
		IXmlRepository xmlRepo;
		ISyncRepository syncRepo;

		public Synchronization()
		{
			ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings["CustomerDB"];
			DbProviderFactory factory = DbProviderFactories.GetFactory(cn.ProviderName);

			xmlRepo = new CustomerRepository(factory, cn.ConnectionString);

			cn = ConfigurationManager.ConnectionStrings["SyncDB"];
			factory = DbProviderFactories.GetFactory(cn.ProviderName);

			syncRepo = new DbSyncRepository(factory, "Customer", cn.ConnectionString);
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/xml";

			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			using (XmlReader r = XmlReader.Create(context.Request.InputStream))
			{
				IList<Item> conflicts = engine.Subscribe(new RssFeedReader(r));
			}

			XmlWriterSettings set = new XmlWriterSettings();
			set.CloseOutput = true;

			using (XmlWriter w = XmlWriter.Create(context.Response.OutputStream, set))
			{
				Feed feed = new Feed(
					Properties.Settings.Default.FeedTitle,
					context.Request.Url.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.SafeUnescaped),
					Properties.Settings.Default.FeedDescription);

				engine.Publish(feed, new RssFeedWriter(w));
			}

			context.Response.End();
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
