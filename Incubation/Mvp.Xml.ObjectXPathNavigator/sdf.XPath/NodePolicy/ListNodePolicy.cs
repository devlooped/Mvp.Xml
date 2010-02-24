using System;
using System.Collections;
using System.Xml.XPath;

namespace Mvp.Xml.ObjectXPathNavigator.NodePolicy
{
	/// <summary>
	/// Implementation of node policy for handling nodes, associated with objects
	/// implementing <see cref="IList"/> interface.
	/// </summary>
	public class ListNodePolicy : CollectionNodePolicyBase
	{
		private static readonly INodePolicy _instance = new ListNodePolicy();

		/// <summary>
		/// Gets the node policy object.
		/// </summary>
		/// <returns>Returns an instance of this node policy.</returns>
		/// <remarks>This node policy object is stateless so all nodes shares
		/// the same instance.</remarks>
		public static new INodePolicy GetPolicy()
		{
			return _instance;
		}

		/// <summary>
		/// Creates a new <see cref="ListNodePolicy"/> instance.
		/// </summary>
		protected ListNodePolicy() {}

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
			return ((IList)node.Object).Count;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChild"/> for details.
		/// </summary>
		public override Node GetChild( Node node, int index )
		{
			IList list = (IList)node.Object;

			Node childNode = null;
			if( index >= 0 && index < list.Count )
			{
				childNode = CreateChildNode( list[index], node );
			}
			return childNode;
		}

	}
}
