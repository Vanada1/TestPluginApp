using Core;

namespace UpperCaseFormatterPlugin
{
    /// <summary>
    /// Класс обработки строки в верхний регистр.
    /// </summary>
    public class UpperCaseFormatter : IPluginInterface
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

            return value.ToUpper();
        }
    }
}
