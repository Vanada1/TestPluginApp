namespace Core;

/// <summary>
/// Интерфейс для плагина.
/// </summary>
public interface IPluginInterface
{
    /// <summary>
    /// Возвращает строку по введенному значению.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <returns>Финальная строка.</returns>
    string GetSomeString(string value);
}