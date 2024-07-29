using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Core;

namespace TestPlugin.ViewModels;

public class MainVM : ObservableObject
{
    public MainVM()
    {
        var plugins = PluginLoader.GetPlugins();
        PluginInterfaces =
            new ObservableCollection<PluginVM>(plugins.Select(plugin => new PluginVM(plugin)));
    }

    public ObservableCollection<PluginVM> PluginInterfaces { get; }
}