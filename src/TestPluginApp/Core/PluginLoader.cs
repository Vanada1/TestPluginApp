using System.ComponentModel;
using System.Reflection;

namespace Core;

public static class PluginLoader
{
    public static List<IPluginInterface> GetPlugins()
    {
        var pluginPaths = GetDeviceAssembliesPaths();
        var assemblies = LoadAssemblies(pluginPaths);
        var plugins = new List<IPluginInterface>();
        foreach (var assembly in assemblies)
        {
            var implementations = typeof(IPluginInterface).GetInterfaceImplementations(assembly);
            plugins.AddRange(
                implementations.Select(
                    implementation =>
                        (IPluginInterface) Activator.CreateInstance(implementation)!));
        }

        return plugins;
    }

    private static IEnumerable<Assembly> LoadAssemblies(IEnumerable<string> deviceAssembliesPaths)
    {
        return deviceAssembliesPaths.Select(Assembly.LoadFrom);
    }

    private static IEnumerable<string> GetDeviceAssembliesPaths()
    {
        var executableDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        var pluginsDirectory = Path.Combine(executableDirectory, "Plugins");
        var files = Directory.GetFiles(pluginsDirectory, "*.dll");
        return files;
    }
}