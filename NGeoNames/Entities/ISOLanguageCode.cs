namespace NGeoNames.Entities
{
    /// <summary>
    /// Represents a language.
    /// </summary>
    public class ISOLanguageCode
    {
        /// <summary>
        /// Gets/sets the ISO-639 Alpha-3 code for comprehensive coverage of languages.
        /// </summary>
        /// <remarks>See <a href="https://en.wikipedia.org/wiki/ISO_639-3">https://en.wikipedia.org/wiki/ISO_639-3</a>.</remarks>
        public string ISO_639_3 { get; set; }

        /// <summary>
        /// Gets/sets the ISO-639 Alpha-3 code.
        /// </summary>
        /// <remarks>See <a href="https://en.wikipedia.org/wiki/ISO_639-2">https://en.wikipedia.org/wiki/ISO_639-2</a>.</remarks>
        public string ISO_639_2 { get; set; }

        /// <summary>
        /// Gets/sets the ISO-639 Alpha-2 code.
        /// </summary>
        /// <remarks>See <a href="https://en.wikipedia.org/wiki/ISO_639-1">https://en.wikipedia.org/wiki/ISO_639-1</a>.</remarks>
        public string ISO_639_1 { get; set; }

        /// <summary>
        /// Gets/sets the name of the language.
        /// </summary>
        public string LanguageName { get; set; }
    }
}
