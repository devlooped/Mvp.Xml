using System;
using System.Xml.XPath;

namespace Mvp.Xml.ObjectXPathNavigator.NodePolicy
{
	/// <summary>
	/// Node policy for a node created for a still unresolved object's field or 
	/// property.
	/// </summary>
	public class MemberNodePolicy : NodePolicyBase
	{
		private static readonly INodePolicy _instance = new MemberNodePolicy();

		/// <summary>
		/// Creates a new <see cref="MemberNodePolicy"/> instance.
		/// </summary>
		protected MemberNodePolicy() {}

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
		/// See <see cref="INodePolicy.GetNewPolicy"/> for details.
		/// </summary>
		public override INodePolicy GetNewPolicy( Node node )
		{
			INodePolicy result = null;

			// Check if we have to switch the policy
			if( node.State == NodeState.ObjectKnown ) 
			{
				if( node.Object != null ) 
				{
					if( !node.ObjectType.IsSimpleType && node.Member.ConverterType == null )
					{
						// Create policy for a node that is not of simple type and
						// without explicitly set converter
						result = node.Context.GetNodePolicy( node.Object );
						string s = node.Name;
						s = node.Namespace;
					} 
					else
					{
						// This is a node with simple content
						result = TextNodePolicy.GetPolicy();	
					}
				} 
				else
				{
					// The value of this member is null
					result = NullValueNodePolicy.GetPolicy();
				}
			} 
			else if( node.State == NodeState.Exception )
			{
				// This is a node with exception
				result = ExceptionNodePolicy.GetPolicy();
			}
			return result;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetName"/> for details.
		/// </summary>
		public override string GetName(Node node)
		{
			return node.Member.Name;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetNamespace"/> for details.
		/// </summary>
		public override string GetNamespace(Node node)
		{
			return node.Member.Namespace;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetNodeType"/> for details.
		/// </summary>
		public override XPathNodeType GetNodeType(Node node)
		{
			return node.Member.NodeType;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetValue"/> for details.
		/// </summary>
		public override string GetValue(Node node)
		{
			// Check if we have to switch policy
			node.ResolveObject();
			if( PolicyChanged( node ) )
				return node.Policy.GetValue( node );

			return String.Empty;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetIsTransparent"/> for details.
		/// </summary>
		public override bool GetIsTransparent(Node node)
		{
			// Check if we have to switch policy
			node.ResolveObject();
			if( PolicyChanged( node ) )
				return node.Policy.GetIsTransparent( node );

			return node.Member.IsTransparent;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetAttributesCount"/> for details.
		/// </summary>
		public override int GetAttributesCount(Node node)
		{
			node.ResolveObject();
			if( PolicyChanged( node ) )
				return node.Policy.GetAttributesCount( node );

/*
			if( !_joinTargetNode )
				// If we do not join target node, there are no attributes
				return 0;
*/
			return 0;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetAttribute"/> for details.
		/// </summary>
		public override Node GetAttribute(Node node, int index)
		{
			node.ResolveObject();
			if( PolicyChanged( node ) )
				return node.Policy.GetAttribute( node, index );

			// The policy was not switched

			return null;
		}

		/// <summary>
		/// See <see cref="INodePolicy.FindAttribute"/> for details.
		/// </summary>
		public override int FindAttribute(Node node, string name, string ns)
		{
			// Check if we have to switch policy
			node.ResolveObject();
			if( PolicyChanged( node ) )
				return node.Policy.FindAttribute( node, name, ns );

			// The policy was not switched

			return -1;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChildrenCount"/> for details.
		/// </summary>
		public override int GetChildrenCount(Node node)
		{
			// Check if we have to switch policy
			node.ResolveObject();
			if( PolicyChanged( node ) )
				return node.Policy.GetChildrenCount( node );

			// The policy was not switched

			return 0;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChild"/> for details.
		/// </summary>
		public override Node GetChild(Node node, int index)
		{
			// Check if we have to switch policy
			node.ResolveObject();
			if( PolicyChanged( node ) )
				return node.Policy.GetChild( node, index );

			// The policy was not switched

			return null;
		}
	}
}
