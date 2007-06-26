#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Mvp.Xml.Synchronization;
using System.Data.SqlServerCe;

namespace CustomerLibrary.Tests
{
	[TestClass]
	public class CustomerSyncFixture : TestFixtureBase
	{
		const string ConnectionString = "Data Source=CustomerDb.sdf";

		[TestInitialize]
		public void Initialize()
		{
			if (File.Exists("CustomerDb.sdf"))
				File.Delete("CustomerDb.sdf");

			SqlCeEngine engine = new SqlCeEngine(ConnectionString);
			engine.CreateDatabase();

			CustomerDataAccess dac = new CustomerDataAccess(new SqlCeProviderFactory(), ConnectionString);
			dac.Add(new Customer("Daniel", "Cazzulino", new DateTime(1974, 4, 9)));
			dac.Add(new Customer("Victor", "Garcia Aprea", new DateTime(1975, 2, 21)));
		}

		[TestMethod]
		public void CanExport()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new CustomerRepository(new SqlCeProviderFactory(), ConnectionString);
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			IEnumerable<Item> items = engine.Export();

			Assert.AreEqual(2, Count(items));
		}

		[TestMethod]
		public void CanExportImportToAnotherRepository()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new CustomerRepository(new SqlCeProviderFactory(), ConnectionString);
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			IEnumerable<Item> items = engine.Export();

			ISyncRepository syncRepo2 = new MockSyncRepository();
			IXmlRepository xmlRepo2 = new MockXmlRepository();
			SyncEngine engine2 = new SyncEngine(xmlRepo2, syncRepo2);

			engine2.Import("customers", items);

			Assert.AreEqual(2, Count(xmlRepo2.GetAll()));
		}

		[TestMethod]
		public void CanImportFromAnotherCustomerRepository()
		{
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new CustomerRepository(new SqlCeProviderFactory(), ConnectionString);
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			IEnumerable<Item> items = engine.Export();

			ISyncRepository syncRepo2 = new MockSyncRepository();
			if (File.Exists("CustomerDb2.sdf"))
				File.Delete("CustomerDb2.sdf");

			SqlCeEngine ceEngine = new SqlCeEngine("Data Source=CustomerDb2.sdf");
			ceEngine.CreateDatabase();

			IXmlRepository xmlRepo2 = new CustomerRepository(new SqlCeProviderFactory(), "Data Source=CustomerDb2.sdf");
			SyncEngine engine2 = new SyncEngine(xmlRepo2, syncRepo2);

			engine2.Import("customers", items);

			Assert.AreEqual(2, Count(xmlRepo2.GetAll()));
		}

		[TestMethod]
		public void CanEditCustomerAndPropagateUpdate()
		{
			new CustomerDataAccess(new SqlCeProviderFactory(), ConnectionString).Delete(2);
			ISyncRepository syncRepo = new MockSyncRepository();
			IXmlRepository xmlRepo = new CustomerRepository(new SqlCeProviderFactory(), ConnectionString);
			SyncEngine engine = new SyncEngine(xmlRepo, syncRepo);

			ISyncRepository syncRepo2 = new MockSyncRepository();
			if (File.Exists("Temp.sdf"))
				File.Delete("Temp.sdf");

			SqlCeEngine ceEngine = new SqlCeEngine("Data Source=Temp.sdf");
			ceEngine.CreateDatabase();

			IXmlRepository xmlRepo2 = new CustomerRepository(new SqlCeProviderFactory(), "Data Source=Temp.sdf");
			SyncEngine engine2 = new SyncEngine(xmlRepo2, syncRepo2);

			engine2.Import("customers", engine.Export());

			// both repositories are in sync now.

			// update customer on one repository.
			CustomerDataAccess dac = new CustomerDataAccess(new SqlCeProviderFactory(), ConnectionString);
			int id = 0;
			foreach (Customer c in dac.GetAll())
			{
				id = c.Id;
				c.FirstName = "kzu";
				dac.Update(c);
				break;
			}

			IEnumerable<Item> exported = engine.Export();
			IEnumerable<ItemMergeResult> merge = engine2.PreviewImport(exported);

			IList<Item> conflicts = engine2.Import("customers", exported);
			Assert.AreEqual(0, conflicts.Count);

			CustomerDataAccess dac2 = new CustomerDataAccess(new SqlCeProviderFactory(), "Data Source=Temp.sdf");
			Customer c2 = dac2.GetById(1);

			Assert.IsNotNull(c2);
			Assert.AreEqual("kzu", c2.FirstName);
		}
	}
}
