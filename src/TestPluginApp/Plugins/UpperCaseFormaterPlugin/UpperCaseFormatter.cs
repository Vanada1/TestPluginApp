using Core;

namespace UpperCaseFormatterPlugin
{
    /// <summary>
    /// Класс обработки строки в верхний регистр.
    /// </summary>
    public partial class UpperCaseFormatter : IPluginInterface
    {
        /// <inheritdoc/>
        public ILicense License { get; } = new UpperCaseFormatterLicense();

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
