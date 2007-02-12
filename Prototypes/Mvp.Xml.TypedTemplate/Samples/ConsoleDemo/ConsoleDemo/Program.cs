using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace ConsoleDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			CustomerTemplate ct = new CustomerTemplate();
			ct.Customer = BuildCustomer();

			XmlWriterSettings ws = new XmlWriterSettings();
			ws.Indent = true;

			using (XmlWriter writer = XmlWriter.Create(Console.Out, ws))
			{
				ct.Render(writer);
			}

			Console.WriteLine();
		}

		// Build dummy object model.
		static Customer BuildCustomer()
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
