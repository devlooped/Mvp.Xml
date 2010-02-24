using System;
using System.Xml.XPath;

namespace Mvp.Xml.ObjectXPathNavigator.NodePolicy
{
	/// <summary>
	/// Node policy for handling member nodes resolved to <see langword="null"/>.
	/// </summary>
	public class NullValueNodePolicy : NodePolicyBase
	{
		private static readonly INodePolicy _instance = new NullValueNodePolicy();

		/// <summary>
		/// Gets the node policy object.
		/// </summary>
		/// <returns>Returns an instance of this node policy.</returns>
		/// <remarks>This node policy object is stateless so all nodes shares
		/// the same instance.</remarks>
		public static INodePolicy GetPolicy()
		{
			return _instance;
		}

		/// <summary>
		/// Creates a new <see cref="NullValueNodePolicy"/> instance.
		/// </summary>
		protected NullValueNodePolicy() {}

		/// <summary>
		/// See <see cref="INodePolicy.GetIsTransparent"/> for details.
		/// </summary>
		public override bool GetIsTransparent( Node node )
		{
			return node.NodeType == XPathNodeType.Attribute || !node.Member.IsNullable;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetAttributesCount"/> for details.
		/// </summary>
		public override int GetAttributesCount( Node node )
		{
			return node.Member.IsNullable ? 1 : 0;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetAttribute"/> for details.
		/// </summary>
		public override Node GetAttribute( Node node, int index )
		{
			Node result = null;

			if( node.Member.IsNullable && index == 0 ) 
			{
				// Return xsi:nil attribute
				result = new Node( node.Context, null );
				result.NodeType = XPathNodeType.Attribute;
				result.Name = "nil";
				result.Namespace = ObjectXPathContext.Xsi;
				result.Value = "true";
			}

			return result;
		}

		/// <summary>
		/// See <see cref="INodePolicy.FindAttribute"/> for details.
		/// </summary>
		public override int FindAttribute( Node node, string name, string ns )
		{
			if( node.Member.IsNullable && name == "nil" && ns == ObjectXPathContext.Xsi )
				// If this is null-node
				return 0;
			return -1;
		}
	}
}
