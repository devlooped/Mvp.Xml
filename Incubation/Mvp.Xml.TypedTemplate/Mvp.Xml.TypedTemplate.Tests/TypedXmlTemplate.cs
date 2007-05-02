using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;

namespace Mvp.Xml.TypedTemplate.Tests
{
	[TestClass]
	public class TypedXmlTemplate
	{
		[TestMethod]
		public void Indexes()
		{
			string foo = " foo";
			Console.WriteLine(foo.Substring(0, 0));
		}

		[TestMethod]
		public void RenderCustomer()
		{
			Customer c = BuildCustomer();

			//CustomerTemplate ct = new CustomerTemplate();
			//ct.customer = c;

			//XmlWriter writer = XmlWriter.Create(Console.Out);
			//ct.Render(writer);

			//writer.Close();
		}

		[TestMethod]
		public void CanReadFragment()
		{
			string value = "customer.LastName + &quot;, &quot; + customer.FirstName";
			XmlReaderSettings rs = new XmlReaderSettings();
			rs.CheckCharacters = true;
			rs.ConformanceLevel = ConformanceLevel.Fragment;
			//rs.LineNumberOffset = GetLine(inputFileContent, m.Index);
			using (XmlReader reader = XmlReader.Create(new StringReader(value), rs))
			{
				reader.Read();
				value = reader.ReadContentAsString();
			}

			Console.WriteLine(value);
		}

		Customer BuildCustomer()
		{
			Customer c = new Customer(
				"Daniel",
				"Cazzulino",
				new List<Order>(new Order[] 
				{
					new Order(
						DateTime.Now, 
						1, 
						new List<Item>(new Item[] 
						{
							new Item(1, 25, 99.99), 
							new Item(5, 75, 199), 
							new Item(2, 150, 25.35)
						})
					),
					new Order(
						DateTime.Now.Subtract(TimeSpan.FromDays(2)), 
						2, 
						new List<Item>(new Item[] 
						{
							new Item(2, 235, 25.35)
						})
					)
				})
			);

			return c;
		}
	}
}
