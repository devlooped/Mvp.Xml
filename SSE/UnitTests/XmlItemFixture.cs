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
	public class XmlItemFixture : TestFixtureBase
	{
		[TestMethod]
		public void ShouldEqualWithSameValues()
		{
			XmlItem i1 = new XmlItem(Guid.NewGuid().ToString(), "title", "description", DateTime.Now, GetElement("<payload/>"));
			XmlItem i2 = new XmlItem(i1.Id, "title", "description", i1.Timestamp, GetElement("<payload/>"));

			Assert.AreEqual(i1, i2);
		}

		[TestMethod]
		public void ShouldNotEqualWithDifferentPayload()
		{
			XmlItem i1 = new XmlItem(Guid.NewGuid().ToString(), "title", "description", DateTime.Now, GetElement("<payload/>"));
			XmlItem i2 = new XmlItem(i1.Id, "title", "description", i1.Timestamp, GetElement("<payload id='foo'/>"));

			Assert.AreNotEqual(i1, i2);
		}

		[TestMethod]
		public void ShouldNotEqualWithDifferentTimespan()
		{
			XmlItem i1 = new XmlItem(Guid.NewGuid().ToString(), "title", "description", DateTime.Now, GetElement("<payload/>"));
			XmlItem i2 = new XmlItem(i1.Id, "title", "description", DateTime.Now.AddMilliseconds(50), GetElement("<payload/>"));

			Assert.AreNotEqual(i1, i2);
		}

		[TestMethod]
		public void ShouldNotEqualWithDifferentTitle()
		{
			XmlItem i1 = new XmlItem(Guid.NewGuid().ToString(), "title1", "description", DateTime.Now, GetElement("<payload/>"));
			XmlItem i2 = new XmlItem(i1.Id, "title2", "description", i1.Timestamp, GetElement("<payload/>"));

			Assert.AreNotEqual(i1, i2);
		}

		[TestMethod]
		public void ShouldNotEqualWithDifferentDescription()
		{
			XmlItem i1 = new XmlItem(Guid.NewGuid().ToString(), "title", "description1", DateTime.Now, GetElement("<payload/>"));
			XmlItem i2 = new XmlItem(i1.Id, "title", "description2", i1.Timestamp, GetElement("<payload/>"));

			Assert.AreNotEqual(i1, i2);
		}
	}
}
