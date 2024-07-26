// See https://aka.ms/new-console-template for more information
using System.Reflection;
using Core;
using TestPluginApp;

var pluginPaths = GetDeviceAssembliesPaths();
var assemblies = LoadAssemblies(pluginPaths);
var plugins = new List<IPluginInterface>();
foreach (var assembly in assemblies)
{
    var implementations = typeof(IPluginInterface).GetInterfaceImplementations(assembly);
    foreach (var implementation in implementations)
    {
        IPluginInterface pluginInterface = null;
        try
        {
            pluginInterface = (IPluginInterface) Activator.CreateInstance(implementation);
        }
        catch (Exception e)
        {
            Console.WriteLine($"You have not license for {implementation.Name}!");
            continue;
        }

        plugins.Add(pluginInterface);
    }
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