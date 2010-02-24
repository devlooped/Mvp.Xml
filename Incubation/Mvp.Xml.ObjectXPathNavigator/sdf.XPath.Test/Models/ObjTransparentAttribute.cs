using System;
using System.Xml.Serialization;
using Mvp.Xml.ObjectXPathNavigator;

namespace Mvp.Xml.ObjectXPathNavigator.Test.Models
{
	[XmlRoot( "tran-attr" )]
	public class ObjTransparentAttribute
	{
		public ObjTransparentAttribute( bool init )
		{
			Name = "Joe";
			TheSimple = new SimpleObject( true );
		}

		public string Name;

		[XmlAnyElement]
		[Transparent( false )]
		public SimpleObject TheSimple;
	}
}
