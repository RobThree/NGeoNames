namespace NGeoNames.Entities
{
    public class ISOLanguageCode
    {
        /// <summary>
        /// Alpha-3 code for comprehensive coverage of languages
        /// </summary>
        public string ISO_639_3 { get; set; }
        /// <summary>
        /// Alpha-3 code
        /// </summary>
        public string ISO_639_2 { get; set; }
        /// <summary>
        /// Alpha-2 code
        /// </summary>
        public string ISO_639_1 { get; set; }
        /// <summary>
        /// Name of the language
        /// </summary>
        public string LanguageName { get; set; }
    }
}
