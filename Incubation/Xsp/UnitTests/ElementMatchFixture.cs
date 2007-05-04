#if NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif
#if VSTS
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.UnitTests
{
	[TestClass]
	public class ElementMatchFixture : TestFixtureBase
	{
		[TestMethod]
		public void ShouldMatchElement()
		{
			XmlReader reader = GetReader("<root><foo></foo></root>");
			ElementMatch match = new ElementMatch("foo", MatchMode.StartElement);

			reader.MoveToContent();
			reader.Read();

			Assert.IsTrue(match.Matches(reader, null));
		}

		[TestMethod]
		public void ShouldMatchStartElementCloseIfNoAttributes()
		{
			XmlReader reader = GetReader("<root></root>");
			ElementMatch match = new ElementMatch("root", MatchMode.StartElementClosed);

			reader.MoveToContent();

			Assert.IsTrue(match.Matches(reader, null));
		}

		[TestMethod]
		public void ShouldMatchStartElementCloseOnLastAttribute()
		{
			XmlReader reader = GetReader("<root id='1' title='foo'></root>");
			ElementMatch match = new ElementMatch("root", MatchMode.StartElementClosed);

			reader.MoveToContent();

			Assert.IsFalse(match.Matches(reader, null));
			reader.MoveToFirstAttribute();
			Assert.IsFalse(match.Matches(reader, null));
			reader.MoveToNextAttribute();
			Assert.IsTrue(match.Matches(reader, null));
		}

		[TestMethod]
		public void ShouldMatchEndElement()
		{
			XmlReader reader = GetReader("<root><foo></foo></root>");
			ElementMatch match = new ElementMatch("foo", MatchMode.EndElement);

			reader.MoveToContent();
			reader.Read();
			reader.Read();

			Assert.IsTrue(match.Matches(reader, null));
		}

		[TestMethod]
		public void ShouldMatchElementForRootToo()
		{
			XmlReader reader = GetReader("<root/>");
			ElementMatch match = new ElementMatch("root", MatchMode.StartElement);

			reader.MoveToContent();

			Assert.IsTrue(match.Matches(reader, null));
		}

		[TestMethod]
		public void ShouldMatchEndElementForRootToo()
		{
			XmlReader reader = GetReader("<root></root>");
			ElementMatch match = new ElementMatch("root", MatchMode.EndElement);

			reader.MoveToContent();
			reader.Read();

			Assert.IsTrue(match.Matches(reader, null));
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfUnknownMode()
		{
			ElementMatch match = new ElementMatch("root", (MatchMode)10);
		}
	}
}
