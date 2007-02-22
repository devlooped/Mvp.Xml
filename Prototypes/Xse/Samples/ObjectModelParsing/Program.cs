using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectModelParsing
{
	class Program
	{
		static void Main(string[] args)
		{
			// Product prices do not come from the serialized 
			// XML, but from our own store.
			Dictionary<int, double> productCatalog = GetProducts();
			List<Customer> customers = new List<Customer>();

			// We will use a custom stack to keep temporary values 
			// as we parse.
			TempStack temp = new TempStack();

			

		}

		private static Dictionary<int, double> GetProducts()
		{
			Dictionary<int, double> products = new Dictionary<int, double>();
			products.Add(1, 25.50);
			products.Add(2, 9.99);
			products.Add(3, 8.75);
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
