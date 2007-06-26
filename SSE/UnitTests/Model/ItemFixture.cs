#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;

namespace Mvp.Xml.Synchronization.Tests.Model
{
	[TestClass]
	public class ItemFixture : TestFixtureBase
	{
		// Test item subsumption.

		[TestMethod]
		public void ShouldAllowNullXmlItem()
		{
			// A null XML item is one with sync info and 
			// no payload, typically a deleted item.
			new Item(null, new Sync(Guid.NewGuid().ToString()));
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowNullSync()
		{
			new Item(new XmlItem("a", "b", GetElement("<c/>")), null);
		}

		[TestMethod]
		public void ShouldEqualSameObject()
		{
			Item obj1 = new Item(new XmlItem("a", "b", GetElement("<c/>")), new Sync(Guid.NewGuid().ToString()));
			Item obj2 = obj1;

			AssertEquals(obj1, obj2);
		}

		[TestMethod]
		public void ShouldNotEqualNull()
		{
			Item obj1 = new Item(new XmlItem("a", "b", GetElement("<c/>")), new Sync(Guid.NewGuid().ToString()));
			Item obj2 = null;

			AssertNotEquals(obj1, obj2);
		}

		[TestMethod]
		public void ShouldNotEqualDifferentSync()
		{
			Item obj1 = new Item(new XmlItem("foo", "bar", GetElement("<payload/>")), new Sync(Guid.NewGuid().ToString()));
			Item obj2 = new Item(obj1.XmlItem, new Sync(Guid.NewGuid().ToString()));

			AssertNotEquals(obj1, obj2);			
		}

		[TestMethod]
		public void ShouldNotEqualDifferentItem()
		{
			Item obj1 = new Item(new XmlItem("foo", "bar", GetElement("<payload/>")), new Sync(Guid.NewGuid().ToString()));
			Item obj2 = new Item(new XmlItem("foo", "bar", GetElement("<payload id='2'/>")), obj1.Sync);

			AssertNotEquals(obj1, obj2);
		}

		[TestMethod]
		public void ShouldNotEqualWithOneNullXmlItem()
		{
			Item obj1 = new Item(new XmlItem("foo", "bar", GetElement("<payload/>")), new Sync(Guid.NewGuid().ToString()));
			Item obj2 = new Item(null, obj1.Sync);

			AssertNotEquals(obj1, obj2);
		}

		[TestMethod]
		public void ShouldEqualWithBothNullXmlItemAndSameSync()
		{
			Item obj1 = new Item(null, new Sync(Guid.NewGuid().ToString()));
			Item obj2 = new Item(null, obj1.Sync);

			AssertEquals(obj1, obj2);
		}

		[TestMethod]
		public void ShouldEqualWithEqualItemAndSync()
		{
			Sync s1 = new Sync(Guid.NewGuid().ToString());
			Sync s2 = new Sync(s1.Id);
			DateTime now = DateTime.Now;

			Item obj1 = new Item(new XmlItem(s1.Id, "foo", "bar", now, GetElement("<payload/>")), s1);
			Item obj2 = new Item(new XmlItem(s1.Id, "foo", "bar", now, GetElement("<payload/>")), s2);

			AssertEquals(obj1, obj2);
		}

		[TestMethod]
		public void ShouldGetSameHashcodeWithEqualItemAndSync()
		{
			Sync s1 = new Sync(Guid.NewGuid().ToString());
			Sync s2 = new Sync(s1.Id);
			DateTime now = DateTime.Now;

			Item obj1 = new Item(new XmlItem(s1.Id, "foo", "bar", now, GetElement("<payload/>")), s1);
			Item obj2 = new Item(new XmlItem(s1.Id, "foo", "bar", now, GetElement("<payload/>")), s2);

			Assert.AreEqual(obj1.GetHashCode(), obj2.GetHashCode());
		}

		[TestMethod]
		public void ShouldEqualClonedItem()
		{
			Item obj1 = new Item(new XmlItem("a", "b", GetElement("<c/>")), new Sync(Guid.NewGuid().ToString()));
			Item obj2 = obj1.Clone();

			AssertEquals(obj1, obj2);
		}

		[TestMethod]
		public void ShouldEqualClonedCloneableItem()
		{
			Item obj1 = new Item(new XmlItem("a", "b", GetElement("<c/>")), new Sync(Guid.NewGuid().ToString()));
			Item obj2 = (Item)((ICloneable)obj1).Clone();

			AssertEquals(obj1, obj2);
		}
		
		private static void AssertEquals(Item obj1, Item obj2)
		{
			Assert.AreEqual(obj1, obj2);
			Assert.IsTrue(obj1.Equals(obj2));
			Assert.IsTrue(obj1 == obj2);
			Assert.IsFalse(obj1 != obj2);
		}

		private static void AssertNotEquals(Item obj1, Item obj2)
		{
			Assert.AreNotEqual(obj1, obj2);
			Assert.IsFalse(obj1 == obj2);
			Assert.IsFalse(obj1.Equals(obj2));
			Assert.IsTrue(obj1 != obj2);
		}
	}
}