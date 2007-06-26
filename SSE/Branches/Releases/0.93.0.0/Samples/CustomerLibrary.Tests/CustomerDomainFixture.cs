#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Data.SqlServerCe;

namespace CustomerLibrary.Tests
{
	[TestClass]
	public class CustomerDomainFixture
	{
		const string ConnectionString = "Data Source=CustomerDb.sdf";

		[TestInitialize]
		public void Initialize()
		{
			if (File.Exists("CustomerDb.sdf"))
				File.Delete("CustomerDb.sdf");

			SqlCeEngine engine = new SqlCeEngine(ConnectionString);
			engine.CreateDatabase();
		}

		[TestMethod]
		public void CanCreateCustomers()
		{
			Customer c = new Customer();
			c.FirstName = "Daniel";
			c.LastName = "Cazzulino";
			c.Birthday = new DateTime(1974, 4, 9);

			CustomerDataAccess dac = new CustomerDataAccess(new SqlCeProviderFactory(), ConnectionString);
			dac.Add(c);
			dac.Add(c);
			dac.Add(c);

			IEnumerable<Customer> all = dac.GetAll();
			Assert.AreEqual(3, new List<Customer>(all).Count);
		}

		[TestMethod]
		public void InsertUpdatesCustomerWithGeneratedData()
		{
			Customer c = new Customer();
			c.FirstName = "Daniel";
			c.LastName = "Cazzulino";
			c.Birthday = new DateTime(1974, 4, 9);
			c.Id = 100;

			CustomerDataAccess dac = new CustomerDataAccess(new SqlCeProviderFactory(), ConnectionString);
			dac.Add(c);

			Assert.AreNotEqual(100, c.Id);
			Assert.IsNotNull(c.Timestamp);
		}
		
		[TestMethod]
		public void CanCreateAndReadCustomer()
		{
			Customer c = new Customer();
			c.FirstName = "Daniel";
			c.LastName = "Cazzulino";
			c.Birthday = new DateTime(1974, 4, 9);

			CustomerDataAccess dac = new CustomerDataAccess(new SqlCeProviderFactory(), ConnectionString);
			int id = dac.Add(c);

			Assert.AreNotEqual(0, id);

			Customer c2 = dac.GetById(id);

			Assert.AreEqual(c.FirstName, c2.FirstName);
			Assert.AreEqual(c.LastName, c2.LastName);
		}

		[TestMethod]
		public void CanCreateAndDelete()
		{
			Customer c = new Customer();
			c.FirstName = "Daniel";
			c.LastName = "Cazzulino";
			c.Birthday = new DateTime(1974, 4, 9);

			CustomerDataAccess dac = new CustomerDataAccess(new SqlCeProviderFactory(), ConnectionString);
			int id = dac.Add(c);

			Assert.AreNotEqual(0, id);

			bool deleted = dac.Delete(id);

			Assert.IsTrue(deleted);
			Assert.AreEqual(0, new List<Customer>(dac.GetAll()).Count);
		}

		[TestMethod]
		public void CanUpdate()
		{
			Customer c = new Customer();
			c.FirstName = "Daniel";
			c.LastName = "Cazzulino";
			c.Birthday = new DateTime(1974, 4, 9);

			CustomerDataAccess dac = new CustomerDataAccess(new SqlCeProviderFactory(), ConnectionString);
			int id = dac.Add(c);

			DateTime? insertUpdate = c.Timestamp;

			c.FirstName = "kzu";
			dac.Update(c);

			Customer c2 = dac.GetById(id);

			Assert.AreEqual("kzu", c2.FirstName);
			//Assert.AreNotEqual(insertUpdate, c2.LastUpdated);
		}

		[TestMethod]
		public void GetByNonExistentIdReturnsNull()
		{
			CustomerDataAccess dac = new CustomerDataAccess(new SqlCeProviderFactory(), ConnectionString);
			Customer c = dac.GetById(5);
			
			Assert.IsNull(c);
		}
	}
}
