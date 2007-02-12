namespace ConsoleDemo
{
    using System;
    using System.Xml;
    using ConsoleDemo;
    
    
    public partial class CustomerTemplate
    {
        
        private Customer _customer;
        
        public Customer Customer
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
            writer.WriteStartElement("", "Customer", "mvp-xml-templates");
            writer.WriteAttributeString("", "xmlns", "http://www.w3.org/2000/xmlns/", "mvp-xml-templates");
            writer.WriteAttributeString("", "FullName", "", Converter.ToString(Customer.LastName + ", " + Customer.FirstName));
            foreach (Order o in this.Customer.Orders)  { //;
            writer.WriteStartElement("", "Order", "mvp-xml-templates");
            writer.WriteAttributeString("", "Id", "", Converter.ToString(o.Id));
            writer.WriteAttributeString("", "Premium", "", Converter.ToString(CalculateTotal(o) > 5000));
            writer.WriteAttributeString("", "Date", "", Converter.ToString(o.DateOrdered));
            writer.WriteStartElement("", "GrandTotal", "mvp-xml-templates");
            writer.WriteString(Converter.ToString(CalculateTotal(o)));
            writer.WriteEndElement();
            if (o.Items != null)  { //;
            foreach (Item i in o.Items)  { //;
            writer.WriteStartElement("", "Item", "mvp-xml-templates");
            writer.WriteAttributeString("", "Id", "", Converter.ToString(i.ProductId));
            writer.WriteAttributeString("", "SubTotal", "", Converter.ToString(i.Quantity * i.Price));
            writer.WriteStartElement("", "Quantity", "mvp-xml-templates");
            writer.WriteString(Converter.ToString(i.Quantity));
            writer.WriteEndElement();
            writer.WriteStartElement("", "Price", "mvp-xml-templates");
            writer.WriteString(Converter.ToString(i.Price));
            writer.WriteEndElement();
            writer.WriteEndElement();
            }//;
            }//;
            writer.WriteEndElement();
            }//;
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
