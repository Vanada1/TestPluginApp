namespace TestPlugin;

/// <summary>
/// Менеджер лицензии.
/// </summary>
internal static class LicenseManager
{
    /// <summary>
    /// Проверяет, добавлен ли файл лицензии.
    /// </summary>
    /// <returns>True, если есть лицензия. Иначе false.</returns>
    internal static bool CheckLicense()
    {
        return License.Status.Licensed;
    }

    /// <summary>
    /// Проверяет, есть ли в записи о плагине в файле лицензии.
    /// </summary>
    /// <param name="pluginName">Название плагина.</param>
    /// <returns>True, если есть запись о проверяемом плагине. Иначе false.</returns>
    internal static bool CheckPluginLicense(string pluginName)
    {
        return License.Status.KeyValueList.ContainsKey(pluginName);
    }
}