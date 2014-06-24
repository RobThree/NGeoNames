using System;

namespace NGeoNames.Parsers
{
    /// <summary>
    /// The exception that is thrown when parsing an object from supplied data fails.
    /// </summary>
    public class ParserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ParserException(string message)
            : base(message) { }
    }
}
