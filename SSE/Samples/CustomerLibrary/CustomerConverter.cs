using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using Mvp.Xml.Synchronization;
using System.Xml.XPath;

namespace CustomerLibrary
{
	public class CustomerConverter
	{
		XmlSerializer serializer = new XmlSerializer(typeof(Customer));
		CustomerIdMapper mapper;

		public CustomerConverter(CustomerIdMapper mapper)
		{
			this.mapper = mapper;
		}

		public Customer Convert(IXmlItem item)
		{
			using (XmlReader reader = new XmlNodeReader(item.Payload))
			{
				while (reader.Read())
				{
					if (reader.LocalName == "Customer")
					{
						Customer customer = (Customer)serializer.Deserialize(reader);
						customer.Timestamp = item.Timestamp;
						return customer;
					}
				}
			}

			return null;
		}

		public IXmlItem Convert(Customer customer)
		{
			return new SerializerXmlItem<Customer>(
				mapper.Map(customer.Id),
				"{FirstName}, {LastName}", "Born on {Birthday}",
				customer.Timestamp, customer);
		}
	}
}
