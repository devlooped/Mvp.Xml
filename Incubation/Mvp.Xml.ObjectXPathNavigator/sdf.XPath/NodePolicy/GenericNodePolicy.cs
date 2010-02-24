using System;
using System.Xml.XPath;

namespace Mvp.Xml.ObjectXPathNavigator.NodePolicy
{
	/// <summary>
	/// Generic node policy implementation.
	/// </summary>
	public class GenericNodePolicy : NodePolicyBase
	{
		private static readonly INodePolicy _instance = new GenericNodePolicy();

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
		/// Creates a new <see cref="GenericNodePolicy"/> instance.
		/// </summary>
		protected GenericNodePolicy() {}

		/// <summary>
		/// See <see cref="INodePolicy.GetNewPolicy"/> for details.
		/// </summary>
		public override INodePolicy GetNewPolicy( Node node )
		{
			return null;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetName"/> for details.
		/// </summary>
		public override string GetName( Node node )
		{
			return node.ObjectType.Name;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetNamespace"/> for details.
		/// </summary>
		public override string GetNamespace( Node node )
		{
			return node.ObjectType.Namespace;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetValue"/> for details.
		/// </summary>
		public override string GetValue( Node node )
		{
			if( node.Object != null )
			{
				IConverter c = node.Context.ConverterFactory.GetConverter( node.ObjectType.Type );
				return c.ToString( node.Object );
			}
			else
				return string.Empty;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetAttributesCount"/> for details.
		/// </summary>
		public override int GetAttributesCount( Node node )
		{
			return node.ObjectType.GetAttributes().Length;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetAttribute"/> for details.
		/// </summary>
		public override Node GetAttribute( Node node, int index )
		{
			MemberInfo[] attributes = node.ObjectType.GetAttributes();
			if( index >= 0 && index < attributes.Length )
			{
				Node attr = new Node( node.Context, MemberNodePolicy.GetPolicy() );
				attr.Member = attributes[index];
				return attr;
			}
			return null;
		}

		/// <summary>
		/// See <see cref="INodePolicy.FindAttribute"/> for details.
		/// </summary>
		public override int FindAttribute( Node node, string name, string ns )
		{
			MemberInfo attrInfo = node.ObjectType.GetAttribute( name, ns );
			if( attrInfo != null )
				return attrInfo.Index;

			return -1;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChildrenCount"/> for details.
		/// </summary>
		public override int GetChildrenCount( Node node )
		{
			return node.ObjectType.GetElements().Length;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChild"/> for details.
		/// </summary>
		public override Node GetChild( Node node, int index )
		{
			MemberInfo[] elements = node.ObjectType.GetElements();
			if( index >= 0 && index < elements.Length )
			{
				INodePolicy policy;
				MemberInfo member = elements[index];
				if( member.NodePolicy != null )
					policy = node.Context.CreateNodePolicy( member.NodePolicy.NodePolicyType );
				else
					policy = MemberNodePolicy.GetPolicy();
				Node child = new Node( node.Context, policy );
				child.Member = member;
				return child;
			}
			return null;
		}
	}
}
