namespace Mvp.Xml.TypedTemplate.Tests
{
    using System;
    using System.IO;
    using Mvp.Xml.TypedTemplate.Tests;
    
    
    public class CustomerTemplate
    {
        
        private Mvp.Xml.TypedTemplate.Tests.Customer _customer;
        
        public Mvp.Xml.TypedTemplate.Tests.Customer customer
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
            
            #line 1 "CustomerTemplate.xml"
            System.IO.StringWriter output = new System.IO.StringWriter();
            
            #line default
            #line hidden
            
            #line 4 "CustomerTemplate.xml"
            output.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            
            #line default
            #line hidden
            
            #line 5 "CustomerTemplate.xml"
            output.Write("<Customer xmlns=\"mvp-xml-templates\" Name=\"");
            
            #line default
            #line hidden
            
            #line 5 "CustomerTemplate.xml"
            output.Write( customer.LastName + ", " + customer.FirstName );
            
            #line default
            #line hidden
            
            #line 5 "CustomerTemplate.xml"
            output.WriteLine("\">");
            
            #line default
            #line hidden
            
            #line 6 "CustomerTemplate.xml"
            output.WriteLine("    <Orders>");
            
            #line default
            #line hidden
            
            #line 7 "CustomerTemplate.xml"
            output.Write("        ");
            
            #line default
            #line hidden
            
            #line 7 "CustomerTemplate.xml"
 foreach (Order o in customer.Orders) { 
            
            #line default
            #line hidden
            
            #line 7 "CustomerTemplate.xml"
            output.WriteLine();
            
            #line default
            #line hidden
            
            #line 8 "CustomerTemplate.xml"
            output.Write("        <Order Id=\"");
            
            #line default
            #line hidden
            
            #line 8 "CustomerTemplate.xml"
            output.Write( o.Id );
            
            #line default
            #line hidden
            
            #line 8 "CustomerTemplate.xml"
            output.Write("\" ");
            
            #line default
            #line hidden
            
            #line 8 "CustomerTemplate.xml"
 if (o.GrandTotal > 10000) { 
            
            #line default
            #line hidden
            
            #line 8 "CustomerTemplate.xml"
            output.Write("Premium=\"true\"");
            
            #line default
            #line hidden
            
            #line 8 "CustomerTemplate.xml"
 } 
            
            #line default
            #line hidden
            
            #line 8 "CustomerTemplate.xml"
            output.WriteLine(">");
            
            #line default
            #line hidden
            
            #line 9 "CustomerTemplate.xml"
            output.WriteLine("            <Items>");
            
            #line default
            #line hidden
            
            #line 10 "CustomerTemplate.xml"
            output.Write("                ");
            
            #line default
            #line hidden
            
            #line 10 "CustomerTemplate.xml"
 foreach (Item i in o.Items) { 
            
            #line default
            #line hidden
            
            #line 10 "CustomerTemplate.xml"
            output.WriteLine();
            
            #line default
            #line hidden
            
            #line 11 "CustomerTemplate.xml"
            output.Write("                <Item Id=\"");
            
            #line default
            #line hidden
            
            #line 11 "CustomerTemplate.xml"
            output.Write( i.ProductId );
            
            #line default
            #line hidden
            
            #line 11 "CustomerTemplate.xml"
            output.Write("\" SubTotal=\"");
            
            #line default
            #line hidden
            
            #line 11 "CustomerTemplate.xml"
            output.Write( i.Quantity * i.Price );
            
            #line default
            #line hidden
            
            #line 11 "CustomerTemplate.xml"
            output.WriteLine("\" />");
            
            #line default
            #line hidden
            
            #line 12 "CustomerTemplate.xml"
            output.Write("                ");
            
            #line default
            #line hidden
            
            #line 12 "CustomerTemplate.xml"
 } 
            
            #line default
            #line hidden
            
            #line 12 "CustomerTemplate.xml"
            output.WriteLine();
            
            #line default
            #line hidden
            
            #line 13 "CustomerTemplate.xml"
            output.WriteLine("            </Items>");
            
            #line default
            #line hidden
            
            #line 14 "CustomerTemplate.xml"
            output.WriteLine("        </Order>");
            
            #line default
            #line hidden
            
            #line 15 "CustomerTemplate.xml"
            output.Write("        ");
            
            #line default
            #line hidden
            
            #line 15 "CustomerTemplate.xml"
 } 
            
            #line default
            #line hidden
            
            #line 15 "CustomerTemplate.xml"
            output.WriteLine();
            
            #line default
            #line hidden
            
            #line 16 "CustomerTemplate.xml"
            output.WriteLine("    </Orders>");
            
            #line default
            #line hidden
            
            #line 17 "CustomerTemplate.xml"
            output.Write("</Customer>");
            
            #line default
            #line hidden
            
            #line 17 "CustomerTemplate.xml"
            output.Write("");
            
            #line default
            #line hidden
            writer.WriteNode(System.Xml.XmlReader.Create(new System.IO.StringReader(output.ToString())), false);
            writer.Flush();
        }
    }
}
