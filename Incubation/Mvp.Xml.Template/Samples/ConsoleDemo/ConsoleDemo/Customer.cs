using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDemo
{
	public class Customer
	{
		public Customer(string firstName, string lastName, List<Order> orders)
		{
			this.firstName = firstName;
			this.lastName = lastName;
			this.orders = orders;
		}

		private string firstName;

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		private string lastName;

		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		private List<Order> orders;

		public List<Order> Orders
		{
			get { return orders; }
			set { orders = value; }
		}
	}

	public class Order
	{
		public Order(DateTime dateOrdered, int id, List<Item> items)
		{
			this.dateOrdered = dateOrdered;
			this.id = id;
			this.items = items;
		}

		private DateTime dateOrdered;

		public DateTime DateOrdered
		{
			get { return dateOrdered; }
			set { dateOrdered = value; }
		}

		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private List<Item> items;

		public List<Item> Items
		{
			get { return items; }
			set { items = value; }
		}

		public double GrandTotal
		{
			get 
			{
				double total = 0;
				foreach (Item i in items)
				{
					total += i.Quantity * i.Price;
				}

				return total; 
			}
		}

	}

	public class Item
	{
		public Item(int productId, int quantity, double price)
		{
			this.productId = productId;
			this.quantity = quantity;
			this.price = price;
		}

		private int productId;

		public int ProductId
		{
			get { return productId; }
			set { productId = value; }
		}

		private int quantity;

		public int Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}

		private double price;

		public double Price
		{
			get { return price; }
			set { price = value; }
		}
	}
}
