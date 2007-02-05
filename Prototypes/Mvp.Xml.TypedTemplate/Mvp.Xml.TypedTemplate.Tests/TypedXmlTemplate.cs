using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;

namespace Mvp.Xml.TypedTemplate.Tests
{
	[TestClass]
	public class TypedXmlTemplate
	{
		[TestMethod]
		public void RenderCustomer()
		{
			Customer c = BuildCustomer();
			CustomerTemplate ct = new CustomerTemplate();
			ct.customer = c;

			XmlWriter writer = XmlWriter.Create(Console.Out);
			ct.Render(writer);

			writer.Close();
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
