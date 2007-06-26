#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class ComparableListFixture
	{
		[TestMethod]
		public void ShouldNotEqualNull()
		{
			ComparableList<object> list1 = new ComparableList<object>();

			Assert.AreNotEqual(null, list1);
			Assert.IsFalse(list1 == null);
			Assert.IsTrue(list1 != null);
		}

		[TestMethod]
		public void ShouldEqualEmptyLists()
		{
			ComparableList<object> list1 = new ComparableList<object>();
			ComparableList<object> list2 = new ComparableList<object>();

			Assert.AreEqual(list1, list2);
			Assert.IsTrue(list1 == list2);
			Assert.IsFalse(list1 != list2);
			Assert.IsTrue(list1.Equals(list2));
		}

		[TestMethod]
		public void ShouldNotEqualDifferentCount()
		{
			ComparableList<object> list1 = new ComparableList<object>();
			list1.Add(new object());
			ComparableList<object> list2 = new ComparableList<object>();

			Assert.AreNotEqual(null, list1);
			Assert.IsFalse(list1 == list2);
			Assert.IsTrue(list1 != list2);
			Assert.IsFalse(list1.Equals(list2));
		}

		[TestMethod]
		public void ShouldNotEqualListsWithDifferentItems()
		{
			ComparableList<object> list1 = new ComparableList<object>();
			list1.Add(new object());
			ComparableList<object> list2 = new ComparableList<object>();
			list2.Add(new object());

			Assert.AreNotEqual(list1, list2);
			Assert.IsFalse(list1.Equals(list2));
			Assert.IsFalse(list1 == list2);
			Assert.IsTrue(list1 != list2);
		}

		[TestMethod]
		public void ShouldNotEqualListsWithNullItem()
		{
			ComparableList<object> list1 = new ComparableList<object>();
			list1.Add(new object());
			ComparableList<object> list2 = new ComparableList<object>();
			list2.Add(null);

			Assert.AreEqual(list1.Count, list2.Count);
			Assert.AreNotEqual(list1, list2);
			Assert.IsFalse(list1.Equals(list2));
			Assert.IsFalse(list1 == list2);
			Assert.IsTrue(list1 != list2);
		}

		[TestMethod]
		public void ShouldEqualListsWithSameObjects()
		{
			object obj = new object();
			ComparableList<object> list1 = new ComparableList<object>();
			list1.Add(obj);
			ComparableList<object> list2 = new ComparableList<object>();
			list2.Add(obj);

			Assert.AreEqual(list1, list2);
			Assert.IsTrue(list1.Equals(list2));
			Assert.IsTrue(list1 == list2);
			Assert.IsFalse(list1 != list2);
		}

		[TestMethod]
		public void ShouldHaveSameHashcodeEqualListsWithSameObjects()
		{
			object obj = new object();
			ComparableList<object> list1 = new ComparableList<object>();
			list1.Add(obj);
			ComparableList<object> list2 = new ComparableList<object>();
			list2.Add(obj);

			Assert.AreEqual(list1.GetHashCode(), list2.GetHashCode());
		}

		[TestMethod]
		public void ShouldEqualSameList()
		{
			ComparableList<object> list1 = new ComparableList<object>();
			ComparableList<object> list2 = list1;

			Assert.AreEqual(list1, list2);
			Assert.IsTrue(list1.Equals(list2));
			Assert.IsTrue(list1 == list2);
			Assert.IsFalse(list1 != list2);
		}

		[TestMethod]
		public void ShouldNotEqualListsWithSameObjectsButDifferentOrders()
		{
			object obj1 = new object();
			object obj2 = new object();

			ComparableList<object> list1 = new ComparableList<object>();
			list1.Add(obj1);
			list1.Add(obj2);

			ComparableList<object> list2 = new ComparableList<object>();
			list2.Add(obj2);
			list2.Add(obj1);

			Assert.AreNotEqual(list1, list2);
			Assert.IsFalse(list1.Equals(list2));
			Assert.IsFalse(list1 == list2);
			Assert.IsTrue(list1 != list2);
		}
	}
}
