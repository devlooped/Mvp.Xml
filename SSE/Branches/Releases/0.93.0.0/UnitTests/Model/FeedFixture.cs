#if PocketPC
using Microsoft.Practices.Mobile.TestTools.UnitTesting;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;

namespace Mvp.Xml.Synchronization.Tests
{
	[TestClass]
	public class FeedFixture
	{
		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfTitleNull()
		{
			new Feed(null, "asdfas", "asdfasdf");
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ShouldThrowIfTitleEmpty()
		{
			new Feed(String.Empty, "asdfas", "asdfasdf");
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfDescriptionNull()
		{
			new Feed("foo", null, "asdfasdf");
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ShouldThrowIfDescriptionEmpty()
		{
			new Feed("foo", String.Empty, "asdfasdf");
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ShouldThrowIfLinkNull()
		{
			new Feed("asdfas", "asdfasdf", null);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ShouldThrowIfLinkEmpty()
		{
			new Feed("foo", "asdfas", String.Empty);
		}

		[TestMethod]
		public void ShouldMatchConstructorWithProperties()
		{
			Feed f = new Feed("title", "link", "description");
			Assert.AreEqual("title", f.Title);
			Assert.AreEqual("link", f.Link);
			Assert.AreEqual("description", f.Description);
		}
	}
}
