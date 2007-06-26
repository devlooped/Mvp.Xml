#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Xml.XPath;
using System.Net;
using System.Threading;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class HttpSyncFixture : TestFixtureBase
	{
		[TestMethod]
		public void Uritest()
		{
			Console.WriteLine(new Uri("http://foo.com/sync.ashx?id=1").GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.SafeUnescaped));
		}

		[TestMethod]
		public void ShouldPostAndGet()
		{
			using (new HttpServer("http://localhost:8081/feed/"))
			{
				MockSyncRepository localSync = new MockSyncRepository();
				MockXmlRepository localXml = new MockXmlRepository();
				SyncEngine localEngine = new SyncEngine(localXml, localSync);
				localXml.AddTwoItems();

				HttpSync sync = new HttpSync(localEngine);
				Feed localFeed = new Feed("Mock", "http://myclient/feed/", "Mock client feed");
				IList<Item> syncConflicts = sync.Synchronize(localFeed, "http://localhost:8081/feed/");
				Assert.AreEqual(0, syncConflicts.Count);
				Assert.AreEqual(3, Count(localXml.GetAll()));
			}
		}

		//[TestMethod]
		//public void ShouldSaveConflictAndReturn()
		//{
		//    using (HttpServer server = new HttpServer("http://localhost:8081/feed/"))
		//    {
		//        MockSyncRepository localSync = new MockSyncRepository();
		//        MockXmlRepository localXml = new MockXmlRepository();
		//        SyncEngine localEngine = new SyncEngine(localXml, localSync);
		//        localXml.AddTwoItems();

		//        HttpSync sync = new HttpSync(localEngine);
		//        Feed localFeed = new Feed("Mock", "http://myclient/feed/", "Mock client feed");
		//        sync.Synchronize(localFeed, "http://localhost:8081/feed/");

		//        // The two stores are in sync
		//    }

		//}

		class HttpServer : IDisposable
		{
			MockSyncRepository remoteSync;
			MockXmlRepository remoteXml;
			public SyncEngine remoteEngine;
			HttpListener listener;
			Thread thread;
			string serverUrl;

			public HttpServer(string serverUrl)
			{
				this.serverUrl = serverUrl;
				remoteSync = new MockSyncRepository();
				remoteXml = new MockXmlRepository();
				remoteEngine = new SyncEngine(remoteXml, remoteSync);
				remoteXml.AddOneItem();

				thread = new Thread(new ThreadStart(Start));
				thread.Start();
			}

			private void Start()
			{
				listener = new HttpListener();
				listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
				listener.Prefixes.Add(serverUrl);
				listener.Start();

				try
				{
					HttpListenerContext context = listener.GetContext();
					// TODO: should "server" return conflicts to the 
					// client? How do we avoid returning duplicates? 
					// (i.e. item exists on server and is therefore 
					// published, but the same item was sent by the 
					// client and has a conflict. which one should 
					// we return? Conflicting one should already 
					// contain the merged information from the 
					// server item).
					using (XmlReader r = XmlReader.Create(context.Request.InputStream))
					{
						Feed feed;
						IEnumerable<Item> i;
						new RssFeedReader(r).Read(out feed, out i);
						List<Item> items = new List<Item>(i);

						IList<Item> conflicts = remoteEngine.Import(feed.Link, items);
						Assert.AreEqual(0, conflicts.Count);
						Assert.AreEqual(3, Count(remoteXml.GetAll()));
					}

					XmlWriterSettings set = new XmlWriterSettings();
					set.CloseOutput = true;

					using (XmlWriter w = XmlWriter.Create(context.Response.OutputStream, set))
					{
						Feed feed = new Feed("Mock", serverUrl, "Mock feed");
						remoteEngine.Publish(feed, new RssFeedWriter(w));
						w.Flush();
					}

					context.Response.Close();
				}
				catch (ThreadAbortException)
				{
				}
			}

			public void Dispose()
			{
				listener.Close();
				thread.Abort();
			}
		}
	}
}
