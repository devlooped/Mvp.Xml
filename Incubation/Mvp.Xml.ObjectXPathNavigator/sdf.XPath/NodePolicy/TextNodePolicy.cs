using System;
using System.Xml.XPath;

namespace Mvp.Xml.ObjectXPathNavigator.NodePolicy
{
	/// <summary>
	/// Node policy for nodes related to objects of simple type, or nodes with 
	/// converter explicitely defined.
	/// </summary>
	public class TextNodePolicy : NodePolicyBase
	{
		private static readonly INodePolicy _instance = new TextNodePolicy();

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
		/// Creates a new <see cref="TextNodePolicy"/> instance.
		/// </summary>
		protected TextNodePolicy() {}

		/// <summary>
		/// See <see cref="INodePolicy.GetValue"/> for details.
		/// </summary>
		public override string GetValue( Node node )
		{
			IConverter c;
			if( node.Member != null && node.Member.ConverterType != null )
				c = node.Context.ConverterFactory.GetConverter( node.Member.ConverterType, node.Member.Type );
			else
				c = node.Context.ConverterFactory.GetConverter( node.ObjectType.Type );
			return c.ToString( node.Object );
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChildrenCount"/> for details.
		/// </summary>
		public override int GetChildrenCount( Node node )
		{
			return 1;
		}

		/// <summary>
		/// See <see cref="INodePolicy.GetChild"/> for details.
		/// </summary>
		public override Node GetChild( Node node, int index )
		{
			if( index == 0 )
			{
				return new Node( node.Context, InnerNodePolicy.GetPolicy() );
			}
			return null;
		}
	}
}
