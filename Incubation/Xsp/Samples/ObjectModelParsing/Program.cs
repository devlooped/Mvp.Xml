using System;
using System.Collections.Generic;
using System.Text;
using Mvp.Xml;
using System.Xml;
using System.Diagnostics;

namespace ObjectModelParsing
{
	class Program
	{
		static void Main(string[] args)
		{
			// Product prices do not come from the serialized 
			// XML, but from our own store.
			Dictionary<int, double> productCatalog = GetProducts();

			// Object model to parse.
			List<Customer> customers = new List<Customer>();

			// Temporary bag of values to cache while parsing 
			// used to hold constructor arguments.
			Dictionary<string, string> values = new Dictionary<string, string>();
			Customer currentCustomer = null;
			Order currentOrder = null;

			// We specify full end elements as we'll need to match empty elements closing.
			XmlPathReader reader = new XmlPathReader(XmlReader.Create("Customers.xml"), true);
			XmlNamespaceManager ns = new XmlNamespaceManager(reader.NameTable);
			ns.AddNamespace("crm", "http://www.codeplex.com/mvpxml");

			// Customer matching
			//reader.AddPathAction("crm:customer/@id",
			//    delegate { values["id"] = reader.Value; },
			//    ns);
			reader.AddPathAction("crm:customer/crm:firstName",
				delegate { values["firstName"] = reader.ReadElementContentAsString(); },
				ns);
			reader.AddPathAction("crm:customer/crm:lastName",
				delegate { values["lastName"] = reader.ReadElementContentAsString(); },
				ns);
			// showcases matching regardless of namespace
			// we match orders because we neled the child elements 
			// from customer.
			//reader.AddPathAction("*:orders",
			//    MatchMode.StartElement, 
			//    delegate
			//    {
			//        currentCustomer = new Customer(
			//            Int32.Parse(values["id"]),
			//            values["firstName"],
			//            values["lastName"]);
			//    });
			//reader.AddPathAction("crm:customer",
			//    MatchMode.EndElement, 
			//    delegate { customers.Add(currentCustomer); },
			//    ns);

			//// Order matching
			//reader.AddPathAction("crm:order/@id",
			//    delegate { values["id"] = reader.Value; },
			//    ns);
			//reader.AddPathAction("crm:order/@date",
			//    delegate { values["date"] = reader.Value; },
			//    ns);
			//reader.AddPathAction("crm:order",
			//    MatchMode.StartElementClosed,
			//    delegate { currentOrder = new Order(Int32.Parse(values["id"]), DateTime.Parse(values["date"])); },
			//    ns);
			//reader.AddPathAction("crm:order",
			//    MatchMode.EndElement,
			//    delegate { currentCustomer.Orders.Add(currentOrder); },
			//    ns);

			//// Item matching
			//reader.AddPathAction("crm:item/@productId",
			//    delegate { values["productId"] = reader.Value; },
			//    ns);
			//reader.AddPathAction("crm:item/@quantity",
			//    delegate { values["quantity"] = reader.Value; },
			//    ns);
			//reader.AddPathAction("crm:item",
			//    MatchMode.StartElementClosed,
			//    delegate 
			//    { 
			//        int productId = Int32.Parse(values["productId"]);
			//        currentOrder.Items.Add(new Item(
			//            productId, 
			//            productCatalog[productId], 
			//            Int32.Parse(values["quantity"])));
			//    }, 
			//    ns);


			// Finally, read to end and cause all parsing to occur.
			reader.ReadToEnd();

			Debug.Assert(customers.Count == 1);
			Debug.Assert(customers[0].Id == 1);
			Debug.Assert(customers[0].LastName == "Cazzulino");
			Debug.Assert(customers[0].Orders.Count == 1);
			Debug.Assert(customers[0].Orders[0].Items.Count == 3);
		}

		private static Dictionary<int, double> GetProducts()
		{
			Dictionary<int, double> products = new Dictionary<int, double>();
			products.Add(1, 25.50);
			products.Add(2, 9.99);
			products.Add(3, 8.75);

			return products;
		}

		class TempStack : Stack<Object>
		{
			public T Peek<T>()
			{
				return (T)base.Peek();
			}

			public T Pop<T>()
			{
				return (T)base.Pop();
			}
		}
	}
}
