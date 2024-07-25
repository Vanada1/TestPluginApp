namespace TestPluginApp_without_dependencies;

/// <summary>
/// Менеджер лицензии.
/// </summary>
internal static class LicenseManager
{
    internal static bool CheckLicense()
    {
        return License.Status.Licensed;
    }

    internal static bool CheckPluginLicense(string pluginName)
    {
        return License.Status.KeyValueList.ContainsKey(pluginName);
    }
}