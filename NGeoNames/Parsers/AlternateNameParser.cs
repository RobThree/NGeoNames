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
                IsPreferredName = fields[4].Equals("1"),
                IsShortName = fields[5].Equals("1"),
                IsColloquial = fields[6].Equals("1"),
                IsHistoric = fields[7].Equals("1")
            };
        }
    }
}
