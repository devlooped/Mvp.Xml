using System;
using System.Xml;

namespace Mvp.Xml.ObjectXPathNavigator.Test.Models
{
	internal class SimpleConverter : IConverter
	{
		public SimpleConverter( Type type )
		{
			
		}

		public string ToString( object obj )
		{
			return XmlConvert.ToString( (DateTime)obj );
		}

		public object ParseString( string str )
		{
			throw new NotImplementedException();
		}
	}
}
