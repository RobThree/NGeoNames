using System.Text.RegularExpressions;

namespace NGeoNames
{
    /// <summary>
    /// Internal class of extension(helper)methods
    /// </summary>
    internal static class ExtensionMethods
    {
        //TODO: Maybe we can even be a little stricter (excluding all non-printables etc.)? ^[\x20-\x7F]*$
        private static readonly Regex _isASCIIOnly = new Regex("^[\x00-\x7F]*$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Checks if a string contains ASCII-only characters.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>Returns true when the value contains only ASCII characters, false otherwise.</returns>
        public static bool IsASCIIOnly(this string value) {
            return _isASCIIOnly.IsMatch(value);
        }

        /// <summary>
        /// Checks if a string is null or contains ASCII-only characters.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>Returns true when the value is null or contains only ASCII characters, false otherwise.</returns>
        public static bool IsASCIIOnlyOrNull(this string value)
        {
            return value == null || value.IsASCIIOnly();
        }
    }
}
