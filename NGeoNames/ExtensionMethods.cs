using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NGeoNames
{
    /// <summary>
    /// Internal class of extension(helper)methods
    /// </summary>
    internal static class ExtensionMethods
    {
        private static readonly Regex _isASCIIOnly = new Regex("^[\x00-\x7F]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Checks if a string contains ASCII-only characters.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>Returns true when the value contains only ASCII characters, false otherwiser</returns>
        public static bool IsASCIIOnly(this string value) {
            return _isASCIIOnly.IsMatch(value);
        }
    }
}
