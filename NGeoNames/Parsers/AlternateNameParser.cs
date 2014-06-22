using NGeoNames.Entities;

namespace NGeoNames.Parsers
{
    public class AlternateNameParser : BaseParser<AlternateName>
    {
        public override bool HasComments
        {
            get { return false; }
        }

        public override int SkipLines
        {
            get { return 0; }
        }

        public override int ExpectedNumberOfFields
        {
            get { return 8; }
        }

        public override AlternateName Parse(string[] fields)
        {
            return new AlternateName
            {
                Id = int.Parse(fields[0]),
                GeoNameId = int.Parse(fields[1]),
                ISOLanguage = fields[2].Length <= 3 ? fields[2] : null,
                Type = fields[2].Length <= 3 ? null : fields[2],
                Name = fields[3],
                IsPreferredName = Bool2String(fields[4]),
                IsShortName = Bool2String(fields[5]),
                IsColloquial = Bool2String(fields[6]),
                IsHistoric = Bool2String(fields[7])
            };
        }

        private static bool Bool2String(string value)
        {
            return value.Equals("1");
        }
    }
}
