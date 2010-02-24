using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly : CLSCompliant( true )]
[assembly : ComVisible( false )]

[assembly : AssemblyTitle( "sdf.XPath" )]
[assembly : AssemblyDescription( "Implementation of XPathNavigator over an object graph." )]
[assembly : AssemblyConfiguration( "" )]
[assembly : AssemblyCompany( "BYTE-force" )]
[assembly : AssemblyProduct( "SDF" )]
[assembly : AssemblyCopyright( "Copyright (c)2004-2009 BYTE-force" )]
[assembly : AssemblyTrademark( "" )]
[assembly : AssemblyCulture( "" )]
[assembly: AssemblyVersion("1.6.0.0")]


// Permissions
[assembly : ReflectionPermission( SecurityAction.RequestMinimum )]