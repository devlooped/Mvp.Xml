using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Mvp.Xml.Template.Tests
{
    public partial class CustomerTemplate
    {
        // Example of a method in the manually coded partial class
        // that is called by the generated template code.
        private string CalculateTotals(Order order)
        {
            double total = 0;
            foreach (Item i in order.Items)
            {
                total += i.Quantity * i.Price;
            }

            return XmlConvert.ToString(total);
        }
    }
}
