using Core;

namespace UpperCaseFormatterPlugin
{
    /// <summary>
    /// Класс обработки строки в верхний регистр.
    /// </summary>
    public class UpperCaseFormatter : IPluginInterface
    {
        /// <inheritdoc/>
        public string GetSomeString(string value)
        {
            return value.ToUpper();
        }
    }
}
