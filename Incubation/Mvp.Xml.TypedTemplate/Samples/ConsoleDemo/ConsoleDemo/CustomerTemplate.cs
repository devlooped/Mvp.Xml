using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDemo
{
	public partial class CustomerTemplate
	{
		private double CalculateTotal(Order o)
		{
			double total = 0;
			foreach (Item i in o.Items)
			{
				total = i.Quantity * i.Price;
			}

			return total;
		}
	}
}
