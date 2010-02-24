using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Xml.Serialization;
using R = System.Reflection;

namespace Mvp.Xml.ObjectXPathNavigator
{
	internal class EnumConverter : IConverter
	{
		private HybridDictionary _namedValues;
		private HybridDictionary _valuedNames;
		private string _enumName;

		public EnumConverter( Type enumType )
		{
			if( !enumType.IsEnum )
				throw new ArgumentException( "The specified type isn't enum.", "enumType" );
			AnalyzeEnum( enumType );
		}

		public string ToString( object obj )
		{
			string name;
			lock( _valuedNames.SyncRoot )
				name = (string)_valuedNames[obj];
			if( name != null )
				return name;
			else
				return obj.ToString();
		}

		public object ParseString( string str )
		{
			object value;
			lock( _namedValues.SyncRoot )
				value = _namedValues[str];
			if( value == null )
				throw new ArgumentException( string.Format( "{0} enumeration doesn't contain field named {1}.", _enumName, str ), "str" );
			return value;
		}

		private const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

		private void AnalyzeEnum( Type enumType )
		{
			_enumName = enumType.Name;

			R.FieldInfo[] fieldInfos = enumType.GetFields( BindingFlags.Static | BindingFlags.Public );

			_namedValues = new HybridDictionary( fieldInfos.Length, false );
			_valuedNames = new HybridDictionary( fieldInfos.Length, false );

			foreach( R.FieldInfo fi in fieldInfos )
			{
				if( ( fi.Attributes & EnumField ) == EnumField )
				{
					string name = fi.Name;
					object value = fi.GetValue( null );

					Attribute[] attrs =
						Attribute.GetCustomAttributes( fi, typeof( XmlEnumAttribute ) );
					if( attrs.Length > 0 )
					{
						XmlEnumAttribute attr = (XmlEnumAttribute)attrs[0];
						name = attr.Name;
					}

					_namedValues.Add( name, value );
					if( !_valuedNames.Contains( value ))
						_valuedNames.Add( value, name );
				}
			}
		}
	}
}