using System;

namespace NGeoNames.Parsers
{
    public class ParserException : Exception
    {
        public ParserException(string message)
            : base(message) { }
    }
}
