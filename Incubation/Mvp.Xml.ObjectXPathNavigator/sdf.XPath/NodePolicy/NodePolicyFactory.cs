/*
  $Revision: $
  $History: $
  $Archive: $
  $Author: $
*/
using System;
using System.Collections;
using System.Reflection;
using Mvp.Xml.ObjectXPathNavigator.NodePolicy;

namespace Mvp.Xml.ObjectXPathNavigator
{
	/// <summary>
	/// Class for creating node policies on request.
	/// </summary>
	internal class NodePolicyFactory
	{
		private static Hashtable _cache = new Hashtable();
		private Hashtable _typePolicy;
		private ObjectXPathContext _context;

		/// <summary>
		/// Creates a new <see cref="NodePolicyFactory"/> instance.
		/// </summary>
		/// <param name="context">Context to which this factory will belong.</param>
		public NodePolicyFactory( ObjectXPathContext context )
		{
			_typePolicy = new Hashtable();
			_context = context;
		}

		/// <summary>
		/// Creates the policy of given type.
		/// </summary>
		/// <param name="policyType">Type of policy to create.</param>
		/// <returns>An instance of the requested policy type.</returns>
		/// <remarks>This could be either new instance, created especially for
		/// this request, or instance shared by other nodes. This behavior is
		/// solely determined by GetPolicy method of given policy type.</remarks>
		public INodePolicy CreatePolicy( Type policyType )
		{
			PolicyInfo pi = (PolicyInfo)_cache[policyType];
			if( pi == null ) 
			{
				lock( _cache.SyncRoot ) 
				{
					pi = (PolicyInfo)_cache[policyType];
					if( pi == null )
					{
						pi = new PolicyInfo( policyType );
						_cache.Add( policyType, pi );
					}
				}
			}
			return pi.GetNodePolicy();
		}

		/// <summary>
		/// Gets the policy for specific type of objects.
		/// </summary>
		/// <param name="forType">The type of objects which will be serverd by the
		/// policy.</param>
		/// <returns>An policy responsible for handling object of specified type.</returns>
		/// <remarks>This could be either new instance, created especially for
		/// this request, or instance shared by other nodes. This behavior is
		/// solely determined by GetPolicy method of given policy type.</remarks>
		public INodePolicy GetPolicy( Type forType )
		{
			INodePolicy policy;
			
			// Look in the type policies
			Type policyType = (Type)_typePolicy[forType];
			if( policyType == null )
			{
				// Look for policies defined for base types
				Type baseType = forType.BaseType;
				while( baseType != null ) 
				{
					policyType = (Type)_typePolicy[baseType];
					if( policyType != null )
					{
						RegisterPolicy( forType, policyType );
						goto PolicyFound;
					}
					baseType = baseType.BaseType;
				}

				if( policyType == null )
				{
					// Look for interfaces
					foreach( Type iface in forType.GetInterfaces() ) {
						policyType = (Type)_typePolicy[iface];
						if( policyType != null ) 
						{
							RegisterPolicy( forType, policyType );
							goto PolicyFound;
						}
					}
				}
			}

			if( policyType == null ) 
			{
				// If no overrides found, look into type information
				TypeInfo typeInfo = _context.TypeInfoCache.GetTypeInfo( forType );
				policyType = typeInfo.PolicyType;
			}

		PolicyFound:
			policy = CreatePolicy( policyType );
			return policy;
		}

		/// <summary>
		/// Registers the type of policies for handling object of specified type.
		/// </summary>
		/// <param name="forType">For which type of object this policy will be used.</param>
		/// <param name="policyType">Policy type.</param>
		public void RegisterPolicy( Type forType, Type policyType )
		{
			lock( _typePolicy.SyncRoot ) 
			{
				_typePolicy[forType] = policyType;
			}
		}

		private delegate INodePolicy GetNodePolicyDelegate();

		private class PolicyInfo
		{
			public GetNodePolicyDelegate GetNodePolicy;

			public PolicyInfo( Type policyType )
			{
				MethodInfo getPolicy = policyType.GetMethod( "GetPolicy", BindingFlags.Static | BindingFlags.Public );
				if( getPolicy == null || getPolicy.GetParameters().Length > 0 )
					throw new ArgumentException( "NodePolicy type must have public static GetPolicy method.", "policyType" );
				GetNodePolicy = (GetNodePolicyDelegate)Delegate.CreateDelegate( typeof( GetNodePolicyDelegate ), getPolicy );
			}
		}
	}
}
