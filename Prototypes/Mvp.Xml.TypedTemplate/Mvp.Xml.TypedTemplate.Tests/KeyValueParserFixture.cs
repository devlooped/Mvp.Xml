using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mvp.Xml.TypedTemplate.Tests
{
	[TestClass]
	public class KeyValueParserFixture
	{
		[TestMethod]
		public void ShouldParseAttributes()
		{
			string value = "Name=\"Foo\" Value=\"Hey\"";
			IDictionary<string, string> props = KeyValueParser.Parse(value);

			Assert.AreEqual(2, props.Count);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void ThrowsIfNullValue()
		{
			KeyValueParser.Parse(null);
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void ThrowsIfDuplicateAttribute()
		{
			string value = "Name=\"Foo\" Name=\"Hey\"";
			IDictionary<string, string> props = KeyValueParser.Parse(value);

			Assert.AreEqual(2, props.Count);
		}

		[TestMethod]
		public void ShouldBuildCaseInsensitiveDictionary()
		{
			string value = "Name=\"Foo\"";
			IDictionary<string, string> props = KeyValueParser.Parse(value);

			Assert.IsTrue(props.ContainsKey("name"));
		}

		[TestMethod]
		public void ShouldSupportMixingQuoteTypes()
		{
			string value = "Name='Foo' Value=\"Bar\"";
			IDictionary<string, string> props = KeyValueParser.Parse(value);

			Assert.AreEqual(2, props.Count);
		}
	}
}
