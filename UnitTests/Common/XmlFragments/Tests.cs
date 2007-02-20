using System;
using System.IO;
using System.Xml;

using Mvp.Xml.Common;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif


namespace Mvp.Xml.Tests.XmlFragments
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void ReadFragments()
		{
			XmlDocument doc = new XmlDocument();
			using (Stream fs = File.Open("../../Common/XmlFragments/publishers.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				doc.Load(new XmlTextReader(null, new XmlFragmentStream(fs)));
			}
		}

		[Test]
		public void ReadFragmentsRoot()
		{
			XmlDocument doc = new XmlDocument();
			using (Stream fs = File.Open("../../Common/XmlFragments/publishers.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				doc.Load(new XmlTextReader(null, new XmlFragmentStream(fs, "pubs")));
			}

			Assert.AreEqual("pubs", doc.DocumentElement.LocalName);
		}

		[Test]
		public void ReadFragmentsRootNs()
		{
			XmlDocument doc = new XmlDocument();
			using (Stream fs = File.Open("../../Common/XmlFragments/publishers.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				doc.Load(new XmlTextReader(null, new XmlFragmentStream(fs, "pubs", "mvp-xml")));
			}
		}
	}
}
