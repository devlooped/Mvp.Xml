using System;
using System.Collections;

namespace Mvp.Xml.ObjectXPathNavigator
{
	/// <summary>
	/// Used to get converters for types involved in tree traversal.
	/// </summary>
	/// <remarks>
	/// To get the instance of
	/// the type use the <see cref="ObjectXPathContext.ConverterFactory"/> property.
	/// </remarks>
	/// <threadsafety static="true" instance="true"/>
	public class ConverterFactory
	{
		private IConverter _genericConverter;
		private IConverter _emptyConverter;
		private Hashtable _cache;

		/// <summary>
		/// Initializes a new instance of <see cref="ConverterFactory"/> class.
		/// </summary>
		/// <remarks>
		/// The converter factory should not be constructed from your code. The 
		/// constructor is public only for test reasons. To get the instance of
		/// the type use the <see cref="ObjectXPathContext.ConverterFactory"/> property.
		/// </remarks>
		public ConverterFactory()
		{
			_genericConverter = new GenericConverter();
			_emptyConverter = new EmptyConverter();
			_cache = new Hashtable();
			AddConverter( typeof( bool ), new BooleanConverter() );
			AddConverter( typeof( DateTime ), new DateTimeConverter() );
			AddConverter( typeof( Single ), new SingleConverter() );
			AddConverter( typeof( Double ), new DoubleConverter() );
			AddConverter( typeof( Decimal ), new DecimalConverter() );
			AddConverter( typeof( Exception ), new ExceptionConverter() );
		}

		/// <summary>
		/// Gets a converter for specified type.
		/// </summary>
		/// <param name="forType">Type of objects the converter will be work with.</param>
		/// <returns>Returns the converter which knows how to convert objects of
		/// specified type.</returns>
		/// <remarks>
		/// The type of converter to create is determined at the following:
		/// <list type="bullet">
		/// <item><description>if <paramref name="forType"/> is enum, then enum conveter
		/// will be returned;</description></item>
		/// <item><description>if it has the <see cref="ConverterAttribute"/> 
		/// specified, then declared converter type will be used;</description></item>
		/// <item><description>if this is primitive type, generic converter;</description></item>
		/// <item><description>if no specific converter found, empty converter will
		/// be returned.</description></item>
		/// </list><br/>
		/// The factory caches creates convertes so several trheads could get the same
		/// converter object simultaneously.
		/// </remarks>
		public IConverter GetConverter( Type forType )
		{
			IConverter c = (IConverter)_cache[forType];
			if( c == null )
				if( forType.IsEnum )
				{
					// Build new enum converter
					c = StoreConverter( forType, new EnumConverter( forType ) );
				}
				else if( typeof( Exception ).IsAssignableFrom( forType ) )
				{
					c = GetConverter( typeof( Exception ) );
				}
				else
				{
					// Find converter for this type
					TypeInfo typeInfo = TypeInfoCache.Instance.GetTypeInfo( forType );
					Type convType = typeInfo.ConverterType;
					if( convType == null )
						// Look in implemented interfaces
						foreach( Type iface in forType.GetInterfaces())
						{
							convType = TypeInfoCache.Instance.GetTypeInfo( iface ).ConverterType;
							if( convType != null )
								break;
						}
					if( convType != null )
					{
						c = CreateConverter( convType, forType );
						c = StoreConverter( forType, c );
					}
				}

			if( c != null )
				return c;
			/*else 
				if( forType.IsPrimitive || forType == typeof( string ) || forType == typeof( DateTime ))
					return _genericConverter;
			return _emptyConverter;*/
			return _genericConverter;
		}

		/// <summary>
		/// Gets an instance of specified converter type initialized for given type.
		/// </summary>
		/// <param name="converterType">Type of converter.</param>
		/// <param name="argumentType">Type of objects that will be converted.</param>
		/// <returns>Returns the converter of given type, that is ready to work
		/// with object of <paramref name="argumentType"/> type.</returns>
		/// <remarks>
		/// Converter type must implement <see cref="IConverter"/> interface and has
		/// public constructor with argument of <see cref="System.Type"/> type.
		/// The factory caches creates convertes so several trheads could get the same
		/// converter object simultaneously.
		/// </remarks>
		public IConverter GetConverter( Type converterType, Type argumentType )
		{
			ConverterKey converterKey = new ConverterKey( converterType,  argumentType );

			IConverter c = (IConverter)_cache[converterKey];
			if( c == null ) 
			{
				c = CreateConverter( converterType, argumentType );
				c = StoreConverter( converterKey, c );
			}
			return c;
		}

		/// <summary>
		/// Removes a converter for the specified type from cache. 
		/// </summary>
		/// <param name="forType">Converter for which type should be removed.</param>
		/// <returns>Returns the converter instance that was just removed, or 
		/// <see langword="null"/> if converter for this type was not cached.</returns>
		public IConverter RemoveConverter( Type forType )
		{
			IConverter c = (IConverter)_cache[forType];
			if( c != null ) 
			{
				lock( _cache.SyncRoot )
				{
					if( _cache.Contains( forType )) 
						_cache.Remove( forType );
					else
						// If someone has removed converter during evaluation
						c = null;
				} 
			}
			return c;
		}

		/// <summary>
		/// Adds the converter to cache.
		/// </summary>
		/// <param name="forType">For which type this converter intended.</param>
		/// <param name="converter">The converter object.</param>
		/// <returns>Returns a converter object from cache.</returns>
		/// <remarks>If cache already had the converter for this type, then state
		/// of the cache will not be altered, old cached converter will be returned.
		/// </remarks>
		public IConverter AddConverter( Type forType, IConverter converter )
		{
			IConverter c = (IConverter)_cache[forType];
			if( c == null ) 
				c = StoreConverter( forType, converter );
			return c;
		}

		/// <summary>
		/// Gets the generic converter.
		/// </summary>
		/// <value>Converter which uses <see cref="Object.ToString"/> method.</value>
		public IConverter GenericConverter
		{
			get { return _genericConverter; }
		}

		/// <summary>
		/// Gets the empty converter.
		/// </summary>
		/// <value>Converter which always return <see cref="String.Empty"/>.</value>
		public IConverter EmptyConverter
		{
			get { return _emptyConverter; }
		}

		private static IConverter CreateConverter( Type converterType, Type argumentType )
		{
			IConverter c;
			try 
			{
				c = (IConverter)System.Activator.CreateInstance( converterType, new object[] { argumentType } );
			} catch( MissingMethodException e )
			{
				throw new ArgumentException(
					string.Format( "Converter type {0} must implement public constructor with argument of System.Type type.", converterType.Name ),
					"converterType", e );
			}
			return c;
		}

		private IConverter StoreConverter( object key, IConverter converter )
		{
			IConverter c;
			lock( _cache.SyncRoot )
			{
				if( !_cache.Contains( key )) 
				{
					c = converter;
					_cache.Add( key, c );
				} 
				else
					// If someone has added an info during evaluation
					c = (IConverter)_cache[key];
			} 
			return c;
		}

		private class ConverterKey
		{
			public Type ConverterType;
			public Type ForType;

			public ConverterKey( Type converterType, Type forType )
			{
				ConverterType = converterType;
				ForType = forType;
			}

			public override bool Equals( object obj )
			{
				ConverterKey other = obj as ConverterKey;
				if( other == null )
					return false;
				return other.ConverterType == ConverterType && other.ForType == ForType;
			}

			public override int GetHashCode()
			{
				return (int)(((Int64)ConverterType.GetHashCode() + ForType.GetHashCode()) % Int32.MaxValue);
			}
		}
	}
}
