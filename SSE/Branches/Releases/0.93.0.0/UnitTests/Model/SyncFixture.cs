#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class SyncFixture
	{
		[TestMethod]
		public void ShouldEqualNullSyncToNull()
		{
			Assert.IsTrue((Sync)null == null);
		}

		[TestMethod]
		public void ShouldNotEqualNullOperator()
		{
			Assert.IsFalse(null == new Sync("foo"));
			Assert.IsFalse(new Sync("foo") == null);
		}

		[TestMethod]
		public void ShouldNotEqualNull()
		{
			Assert.IsFalse(new Sync("foo").Equals((object)null));
			Assert.IsFalse(new Sync("foo").Equals((Sync)null));
		}

		[TestMethod]
		public void ShouldEqualIfSameId()
		{
			Sync s1 = new Sync(Guid.NewGuid().ToString());
			Sync s2 = new Sync(s1.Id);

			Assert.AreEqual(s1, s2);
			Assert.IsTrue(s1 == s2);
		}

		[TestMethod]
		public void ShouldNotEqualIfDifferentId()
		{
			Sync s1 = new Sync(Guid.NewGuid().ToString());
			Sync s2 = new Sync(Guid.NewGuid().ToString());

			Assert.AreNotEqual(s1, s2);
			Assert.IsFalse(s1 == s2);
			Assert.IsTrue(s1 != s2);
		}

		[TestMethod]
		public void ShouldNotEqualIfDifferentUpdates()
		{
			Sync s1 = new Sync(Guid.NewGuid().ToString());
			Sync s2 = new Sync(s1.Id, 2);

			Assert.AreNotEqual(s1, s2);
			Assert.IsFalse(s1 == s2);
			Assert.IsTrue(s1 != s2);
		}

		[TestMethod]
		public void ShouldNotEqualIfDifferentDeleted()
		{
			Sync s1 = new Sync(Guid.NewGuid().ToString());
			Sync s2 = new Sync(s1.Id);
			s2.Deleted = true;

			Assert.AreNotEqual(s1, s2);
			Assert.IsFalse(s1 == s2);
			Assert.IsTrue(s1 != s2);
		}

		[TestMethod]
		public void ShouldNotEqualIfDifferentNoConflicts()
		{
			Sync s1 = new Sync(Guid.NewGuid().ToString());
			s1.NoConflicts = false;
			Sync s2 = new Sync(s1.Id);
			s2.NoConflicts = true;

			Assert.AreNotEqual(s1, s2);
			Assert.IsFalse(s1 == s2);
			Assert.IsTrue(s1 != s2);
		}

		[TestMethod]
		public void ShouldEqualIfEqualHistory()
		{
			Sync s1 = new Sync(Guid.NewGuid().ToString());
			Sync s2 = s1.Clone();

			History history = new History("foo");
			s1.AddHistory(history);
			s2.AddHistory(history);

			Assert.AreEqual(s1, s2);
			Assert.IsTrue(s1 == s2);
		}

		[TestMethod]
		public void ShouldNotEqualIfDifferentHistory()
		{
			Sync s1 = new Sync(Guid.NewGuid().ToString());
			Sync s2 = s1.Clone();

			s1.AddHistory(new History("kzu"));
			s2.AddHistory(new History("vga"));

			Assert.AreNotEqual(s1, s2);
			Assert.IsFalse(s1 == s2);
			Assert.IsTrue(s1 != s2);
		}



		// TODO: missing tests
		// Validate Id. 2.3
		// Validate updates
		// Test equality
		// Test AddUpdate
		// See if AddHistory is needed as public
		// Test IsConflictWith (see if it belongs here)
		// Sync.Id must be == to Item.XmlItem.Id

		// Validate required properties
	}
}
