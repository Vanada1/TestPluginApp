using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core;
using Microsoft.Win32;

namespace TestPlugin.ViewModels;

public partial class PluginVM : ObservableObject
{
    private readonly IPluginInterface _pluginInterface;

    private string _filename;

    [ObservableProperty]
    private string _inputString;

    public PluginVM(IPluginInterface pluginInterface)
    {
        _pluginInterface = pluginInterface;
    }

    public string PluginString => _pluginInterface.GetSomeString(InputString);

    public bool HasLicence => _pluginInterface.License.IsValidLicenseAvailable;

    public bool IsHardwareLockEnabled => _pluginInterface.License.HardwareLockEnabled;

    public bool IsEvaluationLockEnable => _pluginInterface.License.IsEvaluationLockEnable;

    public int EvaluationTime => _pluginInterface.License.EvaluationTime;

    public string Filename
    {
        get => _filename;
        private set => SetProperty(ref _filename, value);
    }

    [RelayCommand]
    private void LoadLicence()
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.DefaultExt = ".license"; // Default file extension
        openFileDialog.Filter = "License file (.license)|*.license";
        var result = openFileDialog.ShowDialog();
        if (!result.GetValueOrDefault())
        {
            return;
        }

        Filename = openFileDialog.FileName;
        var isLoaded = _pluginInterface.License.LoadLicense(Filename);
        OnPropertyChanged(nameof(PluginString));
        OnPropertyChanged(nameof(HasLicence));
        OnPropertyChanged(nameof(IsHardwareLockEnabled));
        OnPropertyChanged(nameof(IsEvaluationLockEnable));
        OnPropertyChanged(nameof(EvaluationTime));
    }

    partial void OnInputStringChanged(string value)
    {
        OnPropertyChanged(nameof(PluginString));
    }
}