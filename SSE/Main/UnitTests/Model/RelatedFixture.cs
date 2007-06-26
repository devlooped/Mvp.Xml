#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class RelatedFixture
	{
		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfNullLink()
		{
			new Related(null, RelatedType.Complete);
		}

		[TestMethod]
		public void ShouldSetProperties()
		{
			Related r = new Related("foo", RelatedType.Complete, "title");

			Assert.AreEqual("foo", r.Link);
			Assert.AreEqual(RelatedType.Complete, r.Type);
			Assert.AreEqual("title", r.Title);
		}
	}
}
