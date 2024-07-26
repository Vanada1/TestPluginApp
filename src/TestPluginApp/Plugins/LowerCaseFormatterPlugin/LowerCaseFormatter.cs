using Core;
using System.ComponentModel;

namespace LowerCaseFormatterPlugin
{
    /// <summary>
    /// Класс обработки строки в нижний регистр.
    /// </summary>
    public class LowerCaseFormatter : IPluginInterface
    {
        public LowerCaseFormatter()
        {
#if RELEASE
            if (!License.Status.Licensed)
            {
                throw new LicenseException(
                    typeof(LowerCaseFormatter),
                    this,
                    "You have not license!");
            }
#endif
        }

        /// <inheritdoc/>
        public string GetSomeString(string value)
        {
            return value.ToLower();
        }
    }
}
