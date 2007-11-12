namespace Mvp.Xml.Template.Tests
{
    using System;
    using System.Xml;
    
    
    public partial class CustomerTemplate
    {
        
        private Mvp.Xml.Template.Tests.Customer _customer;
        
        public Mvp.Xml.Template.Tests.Customer customer
        {
            get
            {
                return this._customer;
            }
            set
            {
                this._customer = value;
            }
        }
        
        public void Render(System.Xml.XmlWriter writer)
        {
            writer.WriteStartDocument(true);
            writer.WriteStartElement("", "Customer", "mvp-xml-templates");
            writer.WriteAttributeString("", "xmlns", "http://www.w3.org/2000/xmlns/", "mvp-xml-templates");
            writer.WriteAttributeString("", "Name", "", "{ customer.LastName + \", \" + customer.FirstName}");
            writer.WriteStartElement("", "Orders", "mvp-xml-templates");
            foreach (Order o in customer.Orders)  { //;
            writer.WriteStartElement("", "Order", "mvp-xml-templates");
            writer.WriteAttributeString("", "Id", "", "{o.Id}");
            writer.WriteAttributeString("", "Premium", "", "{(o.GrandTotal > 10000)}");
            writer.WriteStartElement("", "Items", "mvp-xml-templates");
            writer.WriteAttributeString("", "GrandTotal", "", "{CalculateTotals(o)}");
            foreach (Item i in o.Items)  { //;
            writer.WriteStartElement("", "Item", "mvp-xml-templates");
            writer.WriteAttributeString("", "Id", "", "{i.ProductId}");
            writer.WriteAttributeString("", "SubTotal", "", "{ i.Quantity * i.Price}");
            writer.WriteEndElement();
            }//;
            writer.WriteEndElement();
            writer.WriteStartElement("", "Recipe", "http://schemas.microsoft.com/pag/gax-core");
            writer.WriteAttributeString("", "xmlns", "http://www.w3.org/2000/xmlns/", "http://schemas.microsoft.com/pag/gax-core");
            writer.WriteAttributeString("", "Name", "", "Foo");
            writer.WriteStartElement("", "Caption", "http://schemas.microsoft.com/pag/gax-core");
            writer.WriteString("{o.DateOrdered}");
            writer.WriteEndElement();
            writer.WriteStartElement("", "Description", "http://schemas.microsoft.com/pag/gax-core");
            writer.WriteString("\n\t\t\t\t\tExample of escaping the curly braces: \n\t\t\t\t\tstring.Format(\"{{0}}\");\n\t\t\t\t\t{o" +
                    ".DateOrdered}\n\t\t\t\t");
            writer.WriteEndElement();
            writer.WriteEndElement();
            if (customer == null)  { //;
            writer.WriteString("\n\t\t\tNull!\n\t\t\t");
            }
			else if (customer != null)  { //;
            writer.WriteString("\n\t\t\tNot Null!\n\t\t\t");
            }
			else { //;
            writer.WriteString("\n\t\t\tNever Reached!\n\t\t\t");
            }//;
            writer.WriteEndElement();
            }//;
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        
        public class Converter : System.Xml.XmlConvert
        {
            
            public static string ToString(string value)
            {
                return value;
            }
        }
    }
}
