#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Data.SqlServerCe;
using System.IO;
using System.Data.Common;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class DbSyncRepositoryFixture : TestFixtureBase
	{
		const string ConnectionString = "Data Source=SyncDb.sdf";

		[TestInitialize]
		public void Initialize()
		{
			if (File.Exists("SyncDb.sdf"))
				File.Delete("SyncDb.sdf");

			new SqlCeEngine(ConnectionString).CreateDatabase();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowIfNullRepositoryId()
		{
			new DbSyncRepository(new SqlCeProviderFactory(), null, ConnectionString);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfNullConnectionString()
		{
			new DbSyncRepository(new SqlCeProviderFactory(), "Foo", null);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfNullFactory()
		{
			new DbSyncRepository(null, "Foo", ConnectionString);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfNullItemTimestamp()
		{
			ISyncRepository repo = new DbSyncRepository(new SqlCeProviderFactory(), "Foo", ConnectionString);
			Sync s = new Sync(Guid.NewGuid().ToString());

			repo.Save(s);
		}

		[TestMethod]
		public void ShouldAddSync()
		{
			ISyncRepository repo = new DbSyncRepository(new SqlCeProviderFactory(), "Foo", ConnectionString);
			Sync s = new Sync(Guid.NewGuid().ToString(), 50);
			s.ItemTimestamp = DateTime.Now;

			repo.Save(s);

			Sync s2 = repo.Get(s.Id);
			Assert.AreEqual(s.Updates, s2.Updates);
		}

		[TestMethod]
		public void ShouldGetAllSync()
		{
			ISyncRepository repo = new DbSyncRepository(new SqlCeProviderFactory(), "Foo", ConnectionString);
			Sync s = new Sync(Guid.NewGuid().ToString(), 50);
			s.ItemTimestamp = DateTime.Now;

			repo.Save(s);
			s = new Sync(Guid.NewGuid().ToString());
			s.ItemTimestamp = DateTime.Now;
			repo.Save(s);

			Assert.AreEqual(2, Count(repo.GetAll()));
		}

		[TestMethod]
		public void ShouldGetConflictSync()
		{
			ISyncRepository repo = new DbSyncRepository(new SqlCeProviderFactory(), "Foo", ConnectionString);
			Sync s = new Sync(Guid.NewGuid().ToString(), 50);
			s.ItemTimestamp = DateTime.Now;

			repo.Save(s);
			s = new Sync(Guid.NewGuid().ToString());
			s.Conflicts.Add(new Item(new XmlItem("title", "desc", GetNavigator("<payload/>")),
				Behaviors.Update(s.Clone(), "foo", DateTime.Now, false)));
			s.ItemTimestamp = DateTime.Now;
			repo.Save(s);

			Assert.AreEqual(1, Count(repo.GetConflicts()));
		}

		[TestMethod]
		public void ShouldGetNullItemIfMissing()
		{
			ISyncRepository repo = new DbSyncRepository(new SqlCeProviderFactory(), "Foo", ConnectionString);
			Sync s = repo.Get(Guid.NewGuid().ToString());

			Assert.IsNull(s);
		}

		[TestMethod]
		public void ShouldGetNullLastSync()
		{
			ISyncRepository repo = new DbSyncRepository(new SqlCeProviderFactory(), "Foo", ConnectionString);

			Assert.IsNull(repo.GetLastSync("foo"));
		}

		[TestMethod]
		public void ShouldSaveLastSync()
		{
			DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 
				DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
			ISyncRepository repo = new DbSyncRepository(new SqlCeProviderFactory(), "Foo", ConnectionString);

			repo.SetLastSync("foo", now);

			Assert.AreEqual(now, repo.GetLastSync("foo"));
		}
	}
}
