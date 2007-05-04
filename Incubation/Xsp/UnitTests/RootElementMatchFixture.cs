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
	public class RootElementMatchFixture : TestFixtureBase
	{
		[TestMethod]
		public void ShouldMatchRootElement()
		{
			XmlReader reader = GetReader("<root/>");
			RootElementMatch match = new RootElementMatch("root", MatchMode.StartElement);

			reader.MoveToContent();

			Assert.IsTrue(match.Matches(reader, null));
		}

		[TestMethod]
		public void ShouldMatchRootEndElement()
		{
			XmlReader reader = GetReader("<root></root>");
			RootElementMatch match = new RootElementMatch("root", MatchMode.EndElement);

			reader.MoveToContent();
			reader.Read();

			Assert.IsTrue(match.Matches(reader, null));
		}
	}
}
