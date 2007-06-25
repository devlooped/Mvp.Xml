#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class ComparableStackFixture
	{
		[TestMethod]
		public void ShouldNotEqualNull()
		{
			ComparableStack<object> list1 = new ComparableStack<object>();

			Assert.AreNotEqual(null, list1);
			Assert.IsFalse(list1 == null);
			Assert.IsTrue(list1 != null);
		}

		[TestMethod]
		public void ShouldEqualEmptyLists()
		{
			ComparableStack<object> list1 = new ComparableStack<object>();
			ComparableStack<object> list2 = new ComparableStack<object>();

			Assert.AreEqual(list1, list2);
			Assert.IsTrue(list1 == list2);
			Assert.IsFalse(list1 != list2);
			Assert.IsTrue(list1.Equals(list2));
		}

		[TestMethod]
		public void ShouldNotEqualDifferentCount()
		{
			ComparableStack<object> list1 = new ComparableStack<object>();
			list1.Push(new object());
			ComparableStack<object> list2 = new ComparableStack<object>();

			Assert.AreNotEqual(null, list1);
			Assert.IsFalse(list1 == list2);
			Assert.IsTrue(list1 != list2);
			Assert.IsFalse(list1.Equals(list2));
		}

		[TestMethod]
		public void ShouldNotEqualListsWithDifferentItems()
		{
			ComparableStack<object> list1 = new ComparableStack<object>();
			list1.Push(new object());
			ComparableStack<object> list2 = new ComparableStack<object>();
			list2.Push(new object());

			Assert.AreNotEqual(list1, list2);
			Assert.IsFalse(list1.Equals(list2));
			Assert.IsFalse(list1 == list2);
			Assert.IsTrue(list1 != list2);
		}

		[TestMethod]
		public void ShouldNotEqualListsWithNullItem()
		{
			ComparableStack<object> list1 = new ComparableStack<object>();
			list1.Push(new object());
			ComparableStack<object> list2 = new ComparableStack<object>();
			list2.Push(null);

			Assert.AreNotEqual(list1, list2);
			Assert.IsFalse(list1.Equals(list2));
			Assert.IsFalse(list1 == list2);
			Assert.IsTrue(list1 != list2);
		}

		[TestMethod]
		public void ShouldEqualListsWithSameObjects()
		{
			object obj = new object();
			ComparableStack<object> list1 = new ComparableStack<object>();
			list1.Push(obj);
			ComparableStack<object> list2 = new ComparableStack<object>();
			list2.Push(obj);

			Assert.AreEqual(list1, list2);
			Assert.IsTrue(list1.Equals(list2));
			Assert.IsTrue(list1 == list2);
			Assert.IsFalse(list1 != list2);
		}

		[TestMethod]
		public void ShouldHaveSameHashcodeEqualListsWithSameObjects()
		{
			object obj = new object();
			ComparableStack<object> list1 = new ComparableStack<object>();
			list1.Push(obj);
			ComparableStack<object> list2 = new ComparableStack<object>();
			list2.Push(obj);

			Assert.AreEqual(list1.GetHashCode(), list2.GetHashCode());
		}

		[TestMethod]
		public void ShouldEqualSameList()
		{
			ComparableStack<object> list1 = new ComparableStack<object>();
			ComparableStack<object> list2 = list1;

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

			ComparableStack<object> list1 = new ComparableStack<object>();
			list1.Push(obj1);
			list1.Push(obj2);

			ComparableStack<object> list2 = new ComparableStack<object>();
			list2.Push(obj2);
			list2.Push(obj1);

			Assert.AreNotEqual(list1, list2);
			Assert.IsFalse(list1.Equals(list2));
			Assert.IsFalse(list1 == list2);
			Assert.IsTrue(list1 != list2);
		}
	}
}
