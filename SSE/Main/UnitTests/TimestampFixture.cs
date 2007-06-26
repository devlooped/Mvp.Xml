#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Xml;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class TimestampFixture : TestFixtureBase
	{
		[TestMethod]
		public void ShouldRoundtripFormat()
		{
			DateTime now = DateTime.Now;
			// Timestamp resolution is up to seconds :S.
			now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

			string value = Timestamp.ToString(now);
			DateTime dt = Timestamp.Parse(value);

			Assert.AreEqual(now, dt);
		}

		[TestMethod]
		public void ShouldNormalizePreserveTimezone()
		{
			DateTime now = DateTime.Now;
			DateTime norm = Timestamp.Normalize(now);

			TimeSpan offset1 = TimeZone.CurrentTimeZone.GetUtcOffset(now);
			TimeSpan offset2 = TimeZone.CurrentTimeZone.GetUtcOffset(norm);

			Assert.AreEqual(offset1, offset2);
		}
	}
}
