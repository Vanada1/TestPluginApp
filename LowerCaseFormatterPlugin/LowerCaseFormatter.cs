using Core;

namespace LowerCaseFormatterPlugin
{
    /// <summary>
    /// Класс обработки строки в нижний регистр.
    /// </summary>
    public class LowerCaseFormatter : IPluginInterface
    {
        /// <inheritdoc/>
        public string GetSomeString(string value)
        {
            return value.ToLower();
        }
    }
}
