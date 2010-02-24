using System.Collections;

namespace Mvp.Xml.ObjectXPathNavigator.NodePolicy
{
	/// <summary>
	/// Custom <see cref="INodePolicy"/> implementations.
	/// </summary>
	public class EnumerableNodePolicy : CollectionNodePolicyBase
	{
		private object[] _elements;

		/// <summary>
		/// Gets the node policy object.
		/// </summary>
		/// <returns>Returns an instance of this node policy.</returns>
		new public static INodePolicy GetPolicy()
		{
			return new EnumerableNodePolicy();
		}

		/// <summary>
		/// Creates a new <see cref="DictionaryNodePolicy"/> instance.
		/// </summary>
		protected EnumerableNodePolicy()
		{}

/*
		/// <summary>
		/// See <see cref="INodePolicy.GetIsTransparent"/> for details.
		/// </summary>
		public override bool GetIsTransparent( Node node )
		{
			return true;
		}
*/

		/// <summary>
		/// See <see cref="INodePolicy.GetAttributesCount"/> for details.
		/// </summary>
		public override int GetAttributesCount( Node node )
		{
			return 0;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChildrenCount"/> for details.
		/// </summary>
		public override int GetChildrenCount( Node node )
		{
			if( _elements == null )
			{
				PrepareElements( node.Object );
			}

			return _elements.Length;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChild"/> for details.
		/// </summary>
		public override Node GetChild( Node node, int index )
		{
			if( _elements == null )
			{
				PrepareElements( node.Object );
			}

			Node memberNode = null;
			if( index >= 0 && index < _elements.Length )
			{
				memberNode = CreateChildNode( _elements[ index ], node );
			}

			return memberNode;
		}

		/// <summary>
		/// Prepares the enumerated elements.
		/// </summary>
		/// <param name="source">An <see cref="IEnumerable"/> or 
		/// <see cref="IEnumerator"/>.</param>
		private void PrepareElements( object source )
		{
			if( _elements == null )
			{
				IEnumerator enumerator = source as IEnumerator;

				if( enumerator == null )
				{
					IEnumerable enumerable = source as IEnumerable;

					if( enumerable != null )
					{
						enumerator = enumerable.GetEnumerator();
					}
				}

				if( enumerator == null )
				{
					_elements = new object[0];
				}
				else
				{
					ArrayList elements = new ArrayList();
					while( enumerator.MoveNext() )
					{
						elements.Add( enumerator.Current );
					}

					_elements = elements.ToArray();
				}
			}
		}
	}
}