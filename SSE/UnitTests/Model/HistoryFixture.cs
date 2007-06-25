#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class HistoryFixture : TestFixtureBase
	{
		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ShouldThrowIfSequenceNotGreaterThanZero()
		{
			new History("foo", DateTime.Now, 0);
		}

		[TestMethod]
		public void ShouldBeValidNullBy()
		{
			History h = new History(null, DateTime.Now, 1);

			Assert.IsNull(h.By);
		}

		[TestMethod]
		public void ShouldBeValidNullWhen()
		{
			History h = new History("foo", null, 1);

			Assert.IsNull(h.When);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ShouldThrowIfBothByAndWhenNull()
		{
			new History(null, null, 1);
		}

		[TestMethod]
		public void ShouldSubsumeWithByEqualSequence()
		{
			History Hx = new History("kzu", null, 2);
			History Hy = new History("kzu", null, 2);

			Assert.IsTrue(Hx.IsSubsumedBy(Hy));
		}

		[TestMethod]
		public void ShouldSubsumeWithByGreaterSequence()
		{
			History Hx = new History("kzu", null, 2);
			History Hy = new History("kzu", null, 3);

			Assert.IsTrue(Hx.IsSubsumedBy(Hy));
		}

		[TestMethod]
		public void ShouldSubsumeWithoutByEqualWhenAndSequence()
		{
			DateTime when = DateTime.Now;
			History Hx = new History(null, when, 2);
			History Hy = new History(null, when, 2);

			Assert.IsTrue(Hx.IsSubsumedBy(Hy));
		}

		[TestMethod]
		public void ShouldNotSubsumeWithoutByOnXAndByOnYWithEqualWhenAndSequence()
		{
			DateTime when = DateTime.Now;

			History Hx = new History(null, when, 2);
			History Hy = new History("kzu", when, 2);

			Assert.IsFalse(Hx.IsSubsumedBy(Hy));
		}

		[TestMethod]
		public void ShouldNotSubsumeWithoutByAndDifferentWhen()
		{
			History Hx = new History(null, DateTime.Now, 2);
			History Hy = new History(null, DateTime.Now.AddSeconds(10), 2);

			Assert.IsFalse(Hx.IsSubsumedBy(Hy));
		}

		[TestMethod]
		public void ShouldNotEqualNull()
		{
			History h1 = new History("foo");
			History h2 = null;

			AssertNotEquals(h1, h2);
		}

		[TestMethod]
		public void ShouldEqualWithSameBy()
		{
			History h1 = new History("foo");
			History h2 = new History("foo");

			AssertEquals(h1, h2);
		}

		[TestMethod]
		public void ShouldNotEqualWithDifferentBy()
		{
			History h1 = new History("foo");
			History h2 = new History("bar");

			AssertNotEquals(h1, h2);
		}

		[TestMethod]
		public void ShouldEqualWithSameWhen()
		{
			DateTime now = DateTime.Now;
			History h1 = new History(now);
			History h2 = new History(now);

			AssertEquals(h1, h2);
		}

		[TestMethod]
		public void ShouldNotEqualWithDifferentWhen()
		{
			History h1 = new History(DateTime.Now);
			History h2 = new History(DateTime.Now.AddSeconds(50));

			AssertNotEquals(h1, h2);
		}

		[TestMethod]
		public void ShouldNotEqualWithSameNowButNullBy()
		{
			DateTime now = DateTime.Now;
			History h1 = new History("kzu", now);
			History h2 = new History(null, now);

			AssertNotEquals(h1, h2);
		}

		[TestMethod]
		public void ShouldEqualWithSameSequenceAndBy()
		{
			DateTime now = DateTime.Now;
			History h1 = new History("kzu", null, 5);
			History h2 = new History("kzu", null, 5);

			AssertEquals(h1, h2);
		}

		[TestMethod]
		public void ShouldEqualWithSameSequenceAndWhen()
		{
			DateTime now = DateTime.Now;
			History h1 = new History(null, now, 5);
			History h2 = new History(null, now, 5);

			AssertEquals(h1, h2);
		}

		[TestMethod]
		public void ShouldHaveSameHashcodeWithSameBy()
		{
			History h1 = new History("kzu");
			History h2 = new History("kzu");

			Assert.AreEqual(h1.GetHashCode(), h2.GetHashCode());
		}

		[TestMethod]
		public void ShouldHaveSameHashcodeWithSameWhen()
		{
			DateTime now = DateTime.Now;
			History h1 = new History(now);
			History h2 = new History(now);

			Assert.AreEqual(h1.GetHashCode(), h2.GetHashCode());
		}

		[TestMethod]
		public void ShouldHaveSameHashcodeWithSameByWhenSequence()
		{
			DateTime now = DateTime.Now;
			History h1 = new History("kzu", now, 5);
			History h2 = new History("kzu", now, 5);

			Assert.AreEqual(h1.GetHashCode(), h2.GetHashCode());
		}

		[TestMethod]
		public void ShouldHaveDifferentHashcodeWithNullBy()
		{
			DateTime now = DateTime.Now;
			History h1 = new History("kzu", now);
			History h2 = new History(null, now);

			Assert.AreNotEqual(h1.GetHashCode(), h2.GetHashCode());
		}

		[TestMethod]
		public void ShouldDefaultNullWhenAndSequenceOne()
		{
			History h = new History("Foo");
			Assert.AreEqual("Foo", h.By);
			Assert.AreEqual(null, h.When);
			Assert.AreEqual(1, h.Sequence);
		}

		[TestMethod]
		public void ShouldDefaultNullByAndSequenceOne()
		{
			DateTime now = Timestamp.Normalize(DateTime.Now);
			History h = new History(now);
			Assert.AreEqual(now, h.When);
			Assert.AreEqual(null, h.By);
			Assert.AreEqual(1, h.Sequence);
		}

		[TestMethod]
		public void ShouldEqualClone()
		{
			History h1 = new History("foo", DateTime.Now, 4);
			History h2 = h1.Clone();

			AssertEquals(h1, h2);
		}

		[TestMethod]
		public void ShouldEqualCloneable()
		{
			History h1 = new History("foo", DateTime.Now, 4);
			History h2 = (History)((ICloneable)h1).Clone();

			AssertEquals(h1, h2);
		}

		[TestMethod]
		public void ShouldTestProperties()
		{
			TestProperties(new History("foo"));
		}

		[TestMethod]
		public void ShouldNormalizeWhenWithoutMilliseconds()
		{
			DateTime when = new DateTime(2007, 6, 6, 6, 6, 6, 100);
			DateTime expected = new DateTime(2007, 6, 6, 6, 6, 6);

			History history = new History(when);

			Assert.AreNotEqual(when, history.When);	
			Assert.AreEqual(expected, history.When);			
		}

		private static void AssertEquals(History h1, History h2)
		{
			Assert.AreEqual(h1, h2);
			Assert.IsTrue(h1.Equals(h2));
			Assert.IsTrue(h1 == h2);
			Assert.IsFalse(h1 != h2);
		}

		private static void AssertNotEquals(History h1, History h2)
		{
			Assert.AreNotEqual(h1, h2);
			Assert.IsFalse(h1 == h2);
			Assert.IsFalse(h1.Equals(h2));
			Assert.IsTrue(h1 != h2);
		}
	}
}
