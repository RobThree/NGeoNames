using NGeoNames.Entities;
namespace NGeoNames.Parsers
{
    public class ISOLanguageCodeParser : BaseParser<ISOLanguageCode>
    {
        public override bool HasComments
        {
            get { return false; }
        }

        public override int SkipLines
        {
            get { return 1; }
        }

        public override int ExpectedNumberOfFields
        {
            get { return 4; }
        }

        public override ISOLanguageCode Parse(string[] fields)
        {
            return new ISOLanguageCode
            {
                ISO_639_3 = fields[0],
                ISO_639_2 = fields[1],
                ISO_639_1 = fields[2],
                LanguageName = fields[3]
            };
        }
    }
}
