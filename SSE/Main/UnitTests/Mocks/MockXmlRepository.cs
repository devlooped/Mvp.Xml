using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Configuration;
using System.Xml;

namespace Mvp.Xml.Synchronization.Tests
{

	public class MockXmlRepository : IXmlRepository
	{
		Dictionary<string, IXmlItem> items = new Dictionary<string, IXmlItem>();

		private XmlElement GetElement(string xml)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xml);
			return doc.DocumentElement;
		}

		public MockXmlRepository AddOneItem()
		{
			string id = Guid.NewGuid().ToString();
			items.Add(id, new XmlItem(id,
				"Foo Title", "Foo Description", DateTime.Now,
				GetElement("<Foo Title='Foo'/>")));

			return this;
		}

		public MockXmlRepository AddTwoItems()
		{
			string id = Guid.NewGuid().ToString();
			items.Add(id, new XmlItem(id,
				"Foo Title", "Foo Description", DateTime.Now,
				GetElement("<Foo Title='Foo'/>")));

			id = Guid.NewGuid().ToString();
			items.Add(id, new XmlItem(id,
				"Bar Title", "Bar Description", DateTime.Now,
				GetElement("<Foo Title='Bar'/>")));

			return this;
		}

		public MockXmlRepository AddThreeItemsByDays()
		{
			string id = Guid.NewGuid().ToString();
			items.Add(id, new XmlItem(id,
				"Foo Title", "Foo Description", DateTime.Now,
				GetElement("<Foo Title='Foo'/>")));

			id = Guid.NewGuid().ToString();
			items.Add(id, new XmlItem(id,
				"Bar Title", "Bar Description", DateTime.Now.Subtract(TimeSpan.FromDays(1)),
				GetElement("<Foo Title='Bar'/>")));

			id = Guid.NewGuid().ToString();
			items.Add(id, new XmlItem(id,
				"Baz Title", "Baz Description", DateTime.Now.Subtract(TimeSpan.FromDays(3)),
				GetElement("<Foo Title='Baz'/>")));

			return this;
		}

		public DateTime Add(IXmlItem item)
		{
			Guard.ArgumentNotNullOrEmptyString(item.Id, "item.Id");
			IXmlItem clone = item.Clone();
			clone.Timestamp = DateTime.Now;

			items.Add(item.Id, clone);

			return clone.Timestamp;
		}

		public bool Contains(string id)
		{
			return items.ContainsKey(id);
		}

		public IXmlItem Get(string id)
		{
			if (items.ContainsKey(id))
			{
				return items[id].Clone();
			}

			return null;
		}

		public bool Remove(string id)
		{
			return items.Remove(id);
		}

		public DateTime Update(IXmlItem item)
		{
			if (!items.ContainsKey(item.Id))
				throw new KeyNotFoundException();


			IXmlItem clone = item.Clone();
			clone.Timestamp = DateTime.Now;
			items[item.Id] = clone;

			return clone.Timestamp;
		}

		public IEnumerable<IXmlItem> GetAll()
		{
			foreach (IXmlItem item in items.Values)
			{
				yield return item.Clone();
			}
		}

		public IEnumerable<IXmlItem> GetAllSince(DateTime date)
		{
			foreach (IXmlItem item in items.Values)
			{
				if (item.Timestamp >= date)
					yield return item.Clone();
			}
		}

		public DateTime GetFirstUpdated()
		{
			if (items.Count == 0) return DateTime.MinValue;

			DateTime first = DateTime.MaxValue;

			foreach (IXmlItem item in items.Values)
			{
				if (item.Timestamp < first)
					first = item.Timestamp;
			}

			return first;
		}

		public DateTime GetFirstUpdated(DateTime since)
		{
			if (items.Count == 0) return since;

			DateTime first = DateTime.MaxValue;

			foreach (IXmlItem item in items.Values)
			{
				if (item.Timestamp < first && item.Timestamp > since)
					first = item.Timestamp;
			}

			return first;
		}

		public DateTime GetLastUpdated()
		{
			if (items.Count == 0) return DateTime.Now;

			DateTime last = DateTime.MinValue;

			foreach (IXmlItem item in items.Values)
			{
				if (item.Timestamp > last)
					last = item.Timestamp;
			}

			return last;
		}
	}

}
