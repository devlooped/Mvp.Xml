using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Serilog;

// configure serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Log.Information($"MethodRenamer started with {args.Length} arguments");

if (args.Length != 2)
{
    Log.Error("MethodRenamer: missing parameter(s).");
    Console.WriteLine("Usage: MethodRenamer [path to config file] [path to assembly]");
    return;
}

// read app settings
var dictionary = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(args[0], false, false)
    .Build()
    .AsEnumerable()
    .ToDictionary(item => item.Key, item => item.Value);

using var assembly = AssemblyDefinition.ReadAssembly(args[1], new ReaderParameters
{
    ReadSymbols = true,
    SymbolReaderProvider = new PortablePdbReaderProvider(),
    ReadWrite = true,
    InMemory = true,
});

foreach (var type in assembly.MainModule.Types)
{
    foreach (var method in type.Methods)
    {
        if (dictionary.TryGetValue(method.Name, out string renamed))
        {
            Console.WriteLine($"Found '{method.Name}' method, renamed to '{renamed}'");
            method.Name = renamed;
        }
    }
}

// Save the modified assembly
assembly.Write(args[1], new WriterParameters
{
    WriteSymbols = true,
    SymbolWriterProvider = new PortablePdbWriterProvider()
});