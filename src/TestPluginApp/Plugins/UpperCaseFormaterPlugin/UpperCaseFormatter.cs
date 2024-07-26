using System.ComponentModel;
using Core;

namespace UpperCaseFormatterPlugin
{
    /// <summary>
    /// Класс обработки строки в верхний регистр.
    /// </summary>
    public class UpperCaseFormatter : IPluginInterface
    {
        public UpperCaseFormatter()
        {
#if RELEASE
            if (!License.Status.Licensed)
            {
                throw new LicenseException(
                    typeof(UpperCaseFormatter),
                    this,
                    "You have not license!");
            }
#endif
        }

        /// <inheritdoc/>
        public string GetSomeString(string value)
        {
            return value.ToUpper();
        }
    }
}
