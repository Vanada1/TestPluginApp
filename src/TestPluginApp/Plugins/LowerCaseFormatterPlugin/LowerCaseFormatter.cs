using Core;

namespace LowerCaseFormatterPlugin
{
    /// <summary>
    /// Класс обработки строки в нижний регистр.
    /// </summary>
    public class LowerCaseFormatter : IPluginInterface
    {
        /// <inheritdoc/>
        public ILicense License { get; } = new CoreLicense();

        /// <inheritdoc/>
        public string GetSomeString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.ToLower();
        }
    }
}
