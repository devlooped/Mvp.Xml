using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: ComVisible(true)]
[assembly: CLSCompliant(false)]

[assembly: AssemblyTitle(ThisAssembly.Title)]
[assembly: AssemblyDescription(ThisAssembly.Description)]
[assembly: AssemblyVersion(ThisAssembly.Version)]

[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]

#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif

internal class ThisAssembly
{
	public const string Title = "Mvp.Xml.Template";
	public const string Description = "MVP XML Typed Templates";
	public const string Version = "0.1.0.0";
}