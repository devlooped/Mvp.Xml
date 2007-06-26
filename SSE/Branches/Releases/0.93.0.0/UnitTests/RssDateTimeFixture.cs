#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class RssDateTimeFixture : TestFixtureBase
	{
		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ThrowsIfNullString()
		{
			RssDateTime.Parse(null);
		}

		[TestMethod]
		public void ToStringRendersOffset()
		{
			RssDateTime rdt = new RssDateTime(DateTime.Now, TimeSpan.FromHours(-3));

			WriteIfDebugging(rdt.ToString());
			Assert.IsTrue(rdt.ToString().EndsWith("-0300"));
		}

		[TestMethod]
		public void StoresOffset()
		{
			RssDateTime rdt = new RssDateTime(DateTime.Now, TimeSpan.FromHours(-3));

			Assert.AreEqual(TimeSpan.FromHours(-3), rdt.Offset);
		}

		[TestMethod]
		public void CanParseCorrectDateTime()
		{
			RssDateTime actual;
			bool canParse = RssDateTime.TryParse("Wed, 18 Oct 06 16:30:10 -0300", out actual);

			Assert.IsTrue(canParse);
			Assert.AreEqual(DayOfWeek.Wednesday, actual.UniversalTime.DayOfWeek);
			Assert.AreEqual(18, actual.UniversalTime.Day);
			Assert.AreEqual(10, actual.UniversalTime.Month);
			Assert.AreEqual(2006, actual.UniversalTime.Year);
			Assert.AreEqual(19, actual.UniversalTime.Hour);
			Assert.AreEqual(30, actual.UniversalTime.Minute);
			Assert.AreEqual(10, actual.UniversalTime.Second);
			Assert.AreEqual(DateTimeKind.Utc, actual.UniversalTime.Kind);
		}

		[ExpectedException(typeof(FormatException))]
		[TestMethod]
		public void InvalidWeekDayThrows()
		{
			RssDateTime.Parse("Fed, 18 Oct 06 16:30:10 -0300");
		}

		[ExpectedException(typeof(FormatException))]
		[TestMethod]
		public void WrongWeekDayThrows()
		{
			RssDateTime.Parse("Fri, 18 Oct 06 16:30:10 -0300");
		}

		[ExpectedException(typeof(FormatException))]
		[TestMethod]
		public void InvalidMonthThrows()
		{
			RssDateTime.Parse("Wed, 18 Foo 06 16:30:10 -0300");
		}

		[TestMethod]
		public void CanSpecifyYearTwoDigits()
		{
			RssDateTime rdt = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 GMT");

			Assert.AreEqual(2006, rdt.UniversalTime.Year);
		}

		[TestMethod]
		public void CanSpecifyYearFourDigits()
		{
			RssDateTime rdt = RssDateTime.Parse("Wed, 18 Oct 2006 16:30:10 GMT");

			Assert.AreEqual(2006, rdt.UniversalTime.Year);
		}

		[TestMethod]
		public void CanSpecifyGMT()
		{
			RssDateTime rdt = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 GMT");
			
			Assert.AreEqual(16, rdt.UniversalTime.Hour);
			Assert.AreEqual(DateTimeKind.Utc, rdt.UniversalTime.Kind);
			Assert.AreEqual(TimeSpan.Zero, rdt.Offset);

			RssDateTime.Parse(rdt.ToString());
		}

		[TestMethod]
		public void CanSpecifyUT()
		{
			RssDateTime rdt = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 UT");

			Assert.AreEqual(16, rdt.UniversalTime.Hour);
			Assert.AreEqual(DateTimeKind.Utc, rdt.UniversalTime.Kind);
			Assert.AreEqual(TimeSpan.Zero, rdt.Offset);

			RssDateTime.Parse(rdt.ToString());
		}

		[TestMethod]
		public void CanSpecifyMilitaryUT()
		{
			RssDateTime rdt = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 Z");

			Assert.AreEqual(16, rdt.UniversalTime.Hour);
			Assert.AreEqual(DateTimeKind.Utc, rdt.UniversalTime.Kind);
			Assert.AreEqual(TimeSpan.Zero, rdt.Offset);

			RssDateTime.Parse(rdt.ToString());
		}

		[TestMethod]
		public void CanSpecifyMilitaryOffset()
		{
			RssDateTime rdt = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 A");

			Assert.AreEqual(17, rdt.UniversalTime.Hour);
			Assert.AreEqual(DateTimeKind.Utc, rdt.UniversalTime.Kind);
			Assert.AreEqual(TimeSpan.FromHours(-1), rdt.Offset);

			RssDateTime.Parse(rdt.ToString());
		}

		[ExpectedException(typeof(FormatException))]
		[TestMethod]
		public void InvalidMilitaryOffsetThrows()
		{
			RssDateTime.Parse("Wed, 18 Foo 06 16:30:10 J");
		}

		[TestMethod]
		public void CanSpecifyUSOffset()
		{
			RssDateTime rdt = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 EST");

			Assert.AreEqual(21, rdt.UniversalTime.Hour);
			Assert.AreEqual(DateTimeKind.Utc, rdt.UniversalTime.Kind);
			Assert.AreEqual(TimeSpan.FromHours(-5), rdt.Offset);

			RssDateTime.Parse(rdt.ToString());
		}

		[ExpectedException(typeof(FormatException))]
		[TestMethod]
		public void InvalidUSOffsetThrows()
		{
			RssDateTime.Parse("Wed, 18 Foo 06 16:30:10 PSD");
		}

		[TestMethod]
		public void AddTimeSpanOperator()
		{
			RssDateTime original = RssDateTime.Parse("Wed, 18 Oct 2006 16:30:10 -0300");
			RssDateTime actual = original + TimeSpan.FromHours(1);

			Assert.AreEqual("Wed, 18 Oct 2006 17:30:10 -0300", actual.ToString());
		}

		[TestMethod]
		public void EqualOperator()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");
			RssDateTime t2 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");

			Assert.IsTrue(t1 == t2);

			t1 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");
			t2 = RssDateTime.Parse("Wed, 18 Oct 06 19:30:10 GMT");

			Assert.IsTrue(t1 == t2);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ShouldThrowIfCompareNotRssDateTime()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");

			t1.CompareTo(new object());
		}

		[TestMethod]
		public void CanClone()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");
			RssDateTime t2 = t1.Clone();

			Assert.IsFalse(Object.ReferenceEquals(t1, t2));
			Assert.AreEqual(t1, t2);
		}

		[TestMethod]
		public void CompareTo()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");
			RssDateTime t2 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");

			Assert.AreEqual(0, t1.CompareTo(t2));

			t1 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");
			t2 = RssDateTime.Parse("Thu, 19 Oct 06 16:30:10 -0300");

			Assert.AreEqual(-1, t1.CompareTo(t2));
			Assert.AreEqual(1, t2.CompareTo(t1));
			Assert.AreEqual(1, t1.CompareTo(null));
			Assert.AreEqual(1, t2.CompareTo(null));
		}

		[TestMethod]
		public void EqualMethod()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");
			RssDateTime t2 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");

			Assert.IsTrue(t1.Equals(t2));

			t1 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");
			t2 = RssDateTime.Parse("Wed, 18 Oct 06 19:30:10 GMT");

			Assert.IsTrue(t1.Equals(t2));
		}

		[TestMethod]
		public void GreaterThanOperator()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0400");
			RssDateTime t2 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");

			Assert.IsTrue(t1 > t2);
		}

		[TestMethod]
		public void GreaterThanEqualsOperator()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");
			RssDateTime t2 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");

			Assert.IsTrue(t1 >= t2);
		}

		[TestMethod]
		public void NotEqualsOperator()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 13:30:10 -0300");
			RssDateTime t2 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");

			Assert.IsTrue(t1 != t2);
		}

		[TestMethod]
		public void LessThanOperator()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 18:30:10 GMT");
			RssDateTime t2 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0300");

			Assert.IsTrue(t1 < t2);
		}

		[TestMethod]
		public void LessThanEqualsOperator()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 18:30:10 GMT");
			RssDateTime t2 = RssDateTime.Parse("Wed, 18 Oct 06 18:30:10 -0000");

			Assert.IsTrue(t1 <= t2);
		}

		[TestMethod]
		public void SubstractDatesOperator()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 18:30:10 GMT");
			RssDateTime t2 = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 -0000");

			Assert.AreEqual(TimeSpan.FromHours(2), t1 - t2);
		}

		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		[TestMethod]
		public void SubstractTimeSpanOperatorThrowsOutOfRange()
		{
			RssDateTime t1 = RssDateTime.MinValue;

			RssDateTime t2 = t1 - TimeSpan.FromDays(1000000);
		}

		[TestMethod]
		public void CanAccessLocalTime()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 18:30:10 -0300");

			DateTime local = t1.LocalTime;
			Assert.AreEqual(DateTimeKind.Local, local.Kind);

			Assert.AreEqual(18, local.Hour);
		}

		[TestMethod]
		public void CanParsePositiveOffset()
		{
			RssDateTime t1 = RssDateTime.Parse("Wed, 18 Oct 06 18:30:10 +0300");
		}

		// This is the preferred format for RSS date time, which 
		// is contraty to the RFC which is 2 digits.
		[TestMethod]
		public void YearIsRenderedInFourDigits()
		{
			RssDateTime rdt = RssDateTime.Parse("Wed, 18 Oct 06 16:30:10 EST");

			string date = rdt.ToString();

			Assert.IsTrue(date.IndexOf("18 Oct 2006") != -1);
		}

		[TestMethod]
		public void NoOffsetAssumesLocalOffset()
		{
			DateTime now = DateTime.Now;
			RssDateTime rdt = new RssDateTime(now);

			TimeSpan offset = now - now.ToUniversalTime();

			Assert.AreEqual(offset, rdt.Offset);

			RssDateTime.Parse(rdt.ToString());
		}
	}
}
