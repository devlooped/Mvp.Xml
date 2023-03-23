using System.Runtime.CompilerServices;
using System.Text;

static class AssemblyInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
}
