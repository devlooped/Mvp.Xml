using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle(ThisAssembly.Title)]
[assembly: AssemblyDescription(ThisAssembly.Description)]
[assembly: AssemblyCompany(ThisAssembly.Company)]
[assembly: AssemblyCopyright(ThisAssembly.Copyright)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if Debug
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: ComVisible(false)]
[assembly: Guid("74262cce-9f30-418c-8e28-35ae800c9b0e")]

[assembly: AssemblyVersion(ThisAssembly.VersionString)]
[assembly: AssemblyFileVersion(ThisAssembly.VersionString)]

// Required by the XPathDocumentWriter.
[assembly: AllowPartiallyTrustedCallers()]

internal static partial class ThisAssembly
{
	public const string Title = "Mvp.Xml";
	public const string Description = "MVP XML Library";
	public const string Product = "Mvp.Xml";
	public const string Company = "MVP XML Project";
	public const string Copyright = "";
	public const string VersionString = "2.3.0.0";

	public static readonly Version Version = new Version(VersionString);
	
}