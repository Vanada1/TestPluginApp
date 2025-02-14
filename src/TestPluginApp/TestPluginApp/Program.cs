﻿// See https://aka.ms/new-console-template for more information
using System.Reflection;
using Core;
using TestPluginApp;

#if RELEASE
if(!LicenseManager.CheckLicense())
{
    Console.WriteLine("You have not license!");
    Console.ReadKey();
    return;
}
#endif

var pluginPaths = GetDeviceAssembliesPaths();
var assemblies = LoadAssemblies(pluginPaths);
var plugins = new List<IPluginInterface>();
foreach (var assembly in assemblies)
{
#if RELEASE
    var pluginName = assembly.GetName().Name;
    if (!LicenseManager.CheckPluginLicense(pluginName))
    {
        Console.WriteLine($"You have not license on {pluginName}!");
        continue;
    }
#endif

    var implementations = typeof(IPluginInterface).GetInterfaceImplementations(assembly);
    plugins.AddRange(
        implementations.Select(
            implementation => (IPluginInterface) Activator.CreateInstance(implementation)!));
}

Console.WriteLine("Choose plugin:");
for (var i = 0; i < plugins.Count; i++)
{
    var plugin = plugins[i];
    Console.WriteLine($"{i}. {plugin.GetType().Name}");
}

Console.WriteLine();
var selectedPluginNumber = Console.ReadLine();
if (!int.TryParse(selectedPluginNumber, out var number) || number >= plugins.Count || number < 0)
{
    Console.WriteLine("Bad value");
    return;
}

var selectedPlugin = plugins[number];
Console.WriteLine("Enter string");
var enteredString = Console.ReadLine();
Console.WriteLine($"Result: {selectedPlugin.GetSomeString(enteredString)}");
Console.ReadKey();
return;

IEnumerable<Assembly> LoadAssemblies(IEnumerable<string> deviceAssembliesPaths)
{
    return deviceAssembliesPaths.Select(Assembly.LoadFrom);
}

IEnumerable<string> GetDeviceAssembliesPaths()
{
    var executableDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
    var pluginsDirectory = Path.Combine(executableDirectory, "Plugins");
    var files = Directory.GetFiles(pluginsDirectory, "*.dll");
    return files;
}