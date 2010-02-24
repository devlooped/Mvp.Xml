using System;
using System.Xml;

namespace Mvp.Xml.ObjectXPathNavigator.NodePolicy
{
	/// <summary>
	/// Node policy for nodes related to instances of <see cref="XmlNode"/> or its 
	/// descendants.
	/// </summary>
	public class XmlNodeNodePolicy : GenericNodePolicy
	{
		private static readonly INodePolicy _instance = new XmlNodeNodePolicy();

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
		/// Creates a new <see cref="XmlNodeNodePolicy"/> instance.
		/// </summary>
		protected XmlNodeNodePolicy() {}

		/// <summary>
		/// See <see cref="INodePolicy.GetIsTransparent"/> for details.
		/// </summary>
		public override bool GetIsTransparent( Node node )
		{
			XmlNode xmlNode = node.Object as XmlNode;
			if( xmlNode.NodeType == XmlNodeType.Document 
				|| xmlNode.NodeType == XmlNodeType.DocumentFragment )
				return true;
			else
				return node.Member != null ? node.Member.IsTransparent : false;
		}
	}
}
