using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectModelParsing
{
	public class Customer
	{
		int id;
		string firstName;
		string lastName;
		List<Order> orders = new List<Order>();

		public Customer(int id, string firstName, string lastName)
		{
			this.id = id;
			this.firstName = firstName;
			this.lastName = lastName;
		}

		public int Id
		{
			get { return id; }
		}

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		public IList<Order> Orders
		{
			get { return orders; }
		}
	}

	public class Order
	{
		int id;
		DateTime orderDate;
		List<Item> items = new List<Item>();

		public Order(int id, DateTime orderDate)
		{
			this.id = id;
			this.orderDate = orderDate;
		}

		public int Id
		{
			get { return id; }
		}

		public DateTime OrderDate
		{
			get { return orderDate; }
		}

		public IList<Item> Items
		{
			get { return items; }
		}

		public double Amount
		{
			get
			{
				double amount = 0;
				foreach (Item item in items)
				{
					amount += item.Price * item.Quantity;
				}

				return amount;
			}
		}
	}

	public class Item
	{
		int productId;
		int quantity;
		double price;

		public Item(int productId, double price, int quantity)
		{
			this.productId = productId;
			this.price = price;
			this.quantity = quantity;
		}
		
		public int ProductId
		{
			get { return productId; }
		}

		public int Quantity
		{
			get { return quantity; }
		}

		public double Price
		{
			get { return price; }
		}
	}
}
