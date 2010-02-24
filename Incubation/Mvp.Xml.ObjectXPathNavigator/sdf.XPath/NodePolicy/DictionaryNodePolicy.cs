using System;
using System.Collections;
using System.Xml.XPath;

namespace Mvp.Xml.ObjectXPathNavigator.NodePolicy
{
	/// <summary>
	/// Node policy realization for classes implementing <see cref="IDictionary"/>
	/// interface.
	/// </summary>
	public class DictionaryNodePolicy : CollectionNodePolicyBase
	{
		private IList _children;

		/// <summary>
		/// Gets the node policy object.
		/// </summary>
		/// <returns>Returns an instance of this node policy.</returns>
		public static new INodePolicy GetPolicy()
		{
			return new DictionaryNodePolicy();
		}

		/// <summary>
		/// Creates a new <see cref="DictionaryNodePolicy"/> instance.
		/// </summary>
		protected DictionaryNodePolicy() {}

		/// <summary>
		/// See <see cref="INodePolicy.GetAttributesCount"/> for details.
		/// </summary>
		public override int GetAttributesCount( Node node )
		{
			return 0;
		}

		private void PrepareChildren( IDictionary dic )
		{
			if( _children == null ) 
			{
				_children = new ArrayList( dic.Keys );
			}		
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChildrenCount"/> for details.
		/// </summary>
		public override int GetChildrenCount( Node node )
		{
			return ((IDictionary)node.Object).Count;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChild"/> for details.
		/// </summary>
		public override Node GetChild( Node node, int index )
		{
			IDictionary dic = (IDictionary)node.Object;
			PrepareChildren( dic );

			Node childNode = null;
			if( index >= 0 && index < dic.Count )
			{
				childNode = CreateChildNode( dic[_children[index]], node );
			}
			return childNode;
		}
	}
}
