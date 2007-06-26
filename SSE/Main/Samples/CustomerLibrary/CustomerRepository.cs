using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Data.Common;
using System.Data;
using CustomerLibrary;
using Mvp.Xml.Synchronization;

namespace CustomerLibrary
{
	public class CustomerRepository : IXmlRepository
	{
		CustomerDataAccess dac;
		CustomerConverter converter;
		CustomerIdMapper mapper;

		public CustomerRepository(DbProviderFactory providerFactory, string connectionString)
		{
			dac = new CustomerDataAccess(providerFactory, connectionString);
			mapper = new CustomerIdMapper(providerFactory, connectionString);
			converter = new CustomerConverter(mapper);
		}

		public DateTime Add(IXmlItem item)
		{
			Customer customer = converter.Convert(item);
			int customerId = dac.Add(customer);

			mapper.Map(item.Id, customerId);

			return customer.Timestamp;
		}

		public bool Contains(string id)
		{
			return dac.Exists(mapper.Map(id));
		}

		public DateTime Update(IXmlItem item)
		{
			Customer customer = converter.Convert(item);
			if (!dac.Update(customer))
				throw new InvalidOperationException("Could not update customer");

			return customer.Timestamp;
		}

		public bool Remove(string id)
		{
			return dac.Delete(mapper.Map(id));
		}

		public IXmlItem Get(string id)
		{
			Customer c = dac.GetById(mapper.Map(id));
			if (c == null) return null;

			return converter.Convert(c);
		}

		public IEnumerable<IXmlItem> GetAll()
		{
			foreach (Customer c in dac.GetAll())
			{
				yield return converter.Convert(c);
			}
		}

		public IEnumerable<IXmlItem> GetAllSince(DateTime date)
		{
			foreach (Customer c in dac.GetAll())
			{
				if (c.Timestamp >= date)
				{
					yield return converter.Convert(c);
				}
			}
		}

		public DateTime GetFirstUpdated()
		{
			DateTime first = DateTime.MaxValue;
			// Implementation unoptimized for the 
			// given store.
			foreach (Customer c in dac.GetAll())
			{
				if (c.Timestamp < first)
				{
					first = c.Timestamp;
				}
			}

			// If there were no items, return min date.
			if (first == DateTime.MaxValue) return DateTime.MinValue;

			return first;
		}

		public DateTime GetFirstUpdated(DateTime since)
		{
			DateTime first = DateTime.MaxValue;
			// Another unoptimized implementation.
			foreach (Customer c in dac.GetAll())
			{
				if (c.Timestamp < first && c.Timestamp > since)
					first = c.Timestamp;
			}

			// If there were no items, return min date.
			if (first == DateTime.MaxValue) return DateTime.MinValue;

			return first;
		}

		public DateTime GetLastUpdated()
		{
			DateTime last = DateTime.MinValue;

			foreach (Customer c in dac.GetAll())
			{
				if (c.Timestamp > last)
					last = c.Timestamp;
			}

			if (last == DateTime.MinValue) return DateTime.Now;

			return last;
		}
	}
}
