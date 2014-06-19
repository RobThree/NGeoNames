using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;
using NGeoNames.Parsers;
using System.IO;
using System.Linq;
using System.Text;

namespace NGeoNamesTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public void ParserThrowsOnInvalidData()
        {
            var target = GeoFileReader.ReadAdmin1Codes(@"testdata\invalid_admin1CodesASCII.txt").ToArray();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ParserThrowsOnNonExistingFile()
        {
            var target = GeoFileReader.ReadAdmin1Codes(@"testdata\non_existing_file.txt").ToArray();
        }

        [TestMethod]
        public void Admin1CodesParser_ParsesFileCorrectly()
        {
            var target = GeoFileReader.ReadAdmin1Codes(@"testdata\test_admin1CodesASCII.txt").ToArray();
            Assert.AreEqual(4, target.Length);

            Assert.AreEqual("CF.04", target[0].Code);
            Assert.AreEqual("Mambéré-Kadéï", target[0].Name);
            Assert.AreEqual("Mambere-Kadei", target[0].NameASCII);
            Assert.AreEqual(2386161, target[0].GeoNameId);

            Assert.AreEqual("This.is.a.VERY.LONG.ID", target[1].Code);
            Assert.AreEqual("Iğdır", target[1].Name);
            Assert.AreEqual("Igdir", target[1].NameASCII);
            Assert.AreEqual(0, target[1].GeoNameId);

            Assert.AreEqual("TR.80", target[2].Code);
            Assert.AreEqual("Şırnak", target[2].Name);
            Assert.AreEqual("Sirnak", target[2].NameASCII);
            Assert.AreEqual(443189, target[2].GeoNameId);

            Assert.AreEqual("UA.26", target[3].Code);
            Assert.AreEqual("Zaporiz’ka Oblast’", target[3].Name);
            Assert.AreEqual("Zaporiz'ka Oblast'", target[3].NameASCII);
            Assert.AreEqual(687699, target[3].GeoNameId);
        }

        [TestMethod]
        public void Admin2CodesParser_ParsesFileCorrectly()
        {
            var target = GeoFileReader.ReadAdmin2Codes(@"testdata\test_admin2Codes.txt").ToArray();
            Assert.AreEqual(2, target.Length);

            Assert.AreEqual("AF.01.7052666", target[0].Code);
            Assert.AreEqual("Darwāz-e Bālā", target[0].Name);
            Assert.AreEqual("Darwaz-e Bala", target[0].NameASCII);
            Assert.AreEqual(7052666, target[0].GeoNameId);

            Assert.AreEqual("CA.10.11", target[1].Code);
            Assert.AreEqual("Gaspésie-Îles-de-la-Madeleine", target[1].Name);
            Assert.AreEqual("Gaspesie-Iles-de-la-Madeleine", target[1].NameASCII);
            Assert.AreEqual(0, target[1].GeoNameId);
        }

        [TestMethod]
        public void AlternateNamesParser_ParsesFileCorrectly()
        {
            var target = GeoFileReader.ReadAlternateNames(@"testdata\test_alternateNames.txt").ToArray();
            Assert.AreEqual(17, target.Length);

            Assert.AreEqual("رودخانه زاکلی", target[0].Name);
            Assert.AreEqual("fa", target[0].ISOLanguage);       //Ensure we have an ISO code
            Assert.IsNull(target[0].Type);                      //When ISO code is specified, type should be null
            Assert.AreEqual(false, target[0].IsPreferredName);  //All these...
            Assert.AreEqual(false, target[0].IsColloquial);     //...bits should...
            Assert.AreEqual(false, target[0].IsShortName);      //...be set...
            Assert.AreEqual(false, target[0].IsHistoric);       //...to false

            Assert.AreEqual("http://en.wikipedia.org/wiki/Takht-e_Qeysar", target[1].Name);
            Assert.IsNull(target[1].ISOLanguage);           //Should be null when a type is specified (e.g. length of ISO code field > 3)
            Assert.AreEqual("link", target[1].Type);        //Ensure we have a type

            Assert.AreEqual("Нагольная", target[2].Name);

            Assert.AreEqual("บ้านน้ำฉ่า", target[3].Name);
            Assert.AreEqual("th", target[3].ISOLanguage);

            Assert.AreEqual("글렌로시스", target[4].Name);
            Assert.AreEqual("ko", target[4].ISOLanguage);

            //Postal code
            Assert.AreEqual("TW13", target[5].Name);
            Assert.AreEqual("post", target[5].Type);

            //IATA - International Air Transport Association; airport code
            Assert.AreEqual("FAB", target[6].Name);
            Assert.AreEqual("iata", target[6].Type);

            //ICAO - International Civil Aviation Organization airport code
            Assert.AreEqual("LSGG", target[7].Name);
            Assert.AreEqual("icao", target[7].Type);

            //ICAO - International Civil Aviation Organization airport code
            Assert.AreEqual("GSN", target[8].Name);
            Assert.AreEqual("faac", target[8].Type);

            Assert.AreEqual("Saipan International Airport", target[9].Name);
            Assert.AreEqual("", target[9].ISOLanguage); //No language,...
            Assert.IsNull(target[9].Type);              //...no type

            //Link
            Assert.AreEqual("http://en.wikipedia.org/wiki/Saipan_International_Airport", target[10].Name);
            Assert.AreEqual("link", target[10].Type);

            //fr_1793 - French Revolution name
            Assert.AreEqual("Ile-de-la-Liberté", target[11].Name);
            Assert.AreEqual("fr_1793", target[11].Type);
            
            Assert.AreEqual("ཞིང་རི", target[12].Name);
            Assert.AreEqual("bo", target[12].ISOLanguage);

            //Abbreviation
            Assert.AreEqual("TRTO", target[13].Name);
            Assert.AreEqual("abbr", target[13].Type);

            //Check flags/bits
            Assert.AreEqual("The Jekyll & Hyde Pub", target[14].Name);
            Assert.AreEqual(true, target[14].IsPreferredName);
            Assert.AreEqual(false, target[14].IsColloquial);
            Assert.AreEqual(true, target[14].IsShortName);
            Assert.AreEqual(false, target[14].IsHistoric);

            Assert.AreEqual("Torre Fiumicelli", target[15].Name);
            Assert.AreEqual(false, target[15].IsPreferredName);
            Assert.AreEqual(true, target[15].IsColloquial);
            Assert.AreEqual(false, target[15].IsShortName);
            Assert.AreEqual(false, target[15].IsHistoric);

            Assert.AreEqual("Abbaye Saint-Antoine-des-Champs", target[16].Name);
            Assert.AreEqual(false, target[16].IsPreferredName);
            Assert.AreEqual(false, target[16].IsColloquial);
            Assert.AreEqual(false, target[16].IsShortName);
            Assert.AreEqual(true, target[16].IsHistoric);
        }

        [TestMethod]
        public void CountryInfoParser_ParsesFileCorrectly()
        {
            var target = GeoFileReader.ReadCountryInfo(@"testdata\test_countryInfo.txt").ToArray();
            Assert.AreEqual(2, target.Length);  //Should've skipped comment lines

            Assert.AreEqual("BZ", target[0].ISO_Alpha2);
            Assert.AreEqual("BLZ", target[0].ISO_Alpha3);
            Assert.AreEqual("084", target[0].ISO_Numeric);  //Check if leading zeroes are preserved
            Assert.AreEqual("BH", target[0].FIPS);
            Assert.AreEqual("Belize", target[0].Country);
            Assert.AreEqual("Belmopan", target[0].Capital);
            Assert.AreEqual(22966, target[0].Area);
            Assert.AreEqual(314522, target[0].Population);
            Assert.AreEqual("NA", target[0].Continent);
            Assert.AreEqual(".bz", target[0].Tld);
            Assert.AreEqual("BZD", target[0].CurrencyCode);
            Assert.AreEqual("Dollar", target[0].CurrencyName);
            Assert.AreEqual("+501", target[0].Phone);       //Intl. dialingcodes should be prefixed with a + even though they're not in the file
            Assert.AreEqual(string.Empty, target[0].PostalCodeFormat);
            Assert.AreEqual(string.Empty, target[0].PostalCodeRegex);
            CollectionAssert.AreEqual(new[] { "en-BZ", "es" }, target[0].Languages);
            Assert.AreEqual(3582678, target[0].GeoNameId);
            CollectionAssert.AreEqual(new[] { "GT", "MX" }, target[0].Neighbours);
            Assert.AreEqual(string.Empty, target[0].EquivalentFipsCode);

            Assert.AreEqual("XX", target[1].ISO_Alpha2);
            Assert.AreEqual("XXX", target[1].ISO_Alpha3);
            Assert.AreEqual("999", target[1].ISO_Numeric);
            Assert.AreEqual(string.Empty, target[1].FIPS);
            Assert.AreEqual("MADE UP COUNTRY", target[1].Country);
            Assert.AreEqual(string.Empty, target[1].Capital);
            Assert.IsNull(target[1].Area);  //Can be unspecified/null
            Assert.AreEqual(0, target[1].Population);
            Assert.AreEqual("EU", target[1].Continent);
            Assert.AreEqual(".XX", target[1].Tld);
            Assert.AreEqual("XXX", target[1].CurrencyCode);
            Assert.AreEqual("X-Dollar", target[1].CurrencyName);
            Assert.AreEqual("+1", target[1].Phone);       //Intl. dialingcodes should be prefixed with a + even though they're not in the file
            Assert.AreEqual("@# #@@|@## #@@|@@# #@@|@@## #@@|@#@ #@@|@@#@ #@@|GIR0AA", target[1].PostalCodeFormat);
            Assert.AreEqual(@"^(([A-Z]\d{2}[A-Z]{2})|([A-Z]\d{3}[A-Z]{2})|([A-Z]{2}\d{2}[A-Z]{2})|([A-Z]{2}\d{3}[A-Z]{2})|([A-Z]\d[A-Z]\d[A-Z]{2})|([A-Z]{2}\d[A-Z]\d[A-Z]{2})|(GIR0AA))$", target[1].PostalCodeRegex);
            Assert.AreEqual(0, target[1].Languages.Length);
            Assert.IsNull(target[1].GeoNameId);
            Assert.AreEqual(0, target[1].Neighbours.Length);
            Assert.AreEqual("XXX", target[1].EquivalentFipsCode);
        }

        //TODO: ExtendedGeoNameParser

        [TestMethod]
        public void FeatureCodeParser_ParsesFileCorrectly()
        {
            var target = GeoFileReader.ReadFeatureCodes(@"testdata\test_featureCodes_en.txt").ToArray();
            Assert.AreEqual(3, target.Length);

            //A.ADM1 should result in a "A" class and "ADM1" code
            Assert.AreEqual("A", target[0].Class);
            Assert.AreEqual("ADM1", target[0].Code);

            Assert.AreEqual("first-order administrative division", target[0].Name);
            Assert.AreEqual("a primary administrative division of a country, such as a state in the United States", target[0].Description);

            ///When no dot in the featurecode is found, the class property should contain the entire string and code property should be null
            Assert.AreEqual("XXX", target[1].Class);
            Assert.IsNull(target[1].Code);
            
            //When the featurecode is "null" both the code and class property should be null
            Assert.IsNull(target[2].Class);
            Assert.IsNull(target[2].Code);
        }

        //TODO: GeoNameParser
        [TestMethod]
        public void HierarchyParser_ParsesFileCorrectly()
        {
            var target = GeoFileReader.ReadHierarchy(@"testdata\test_hierarchy.txt").ToArray();
            Assert.AreEqual(4, target.Length); 

            //"Normal" record
            Assert.AreEqual(6295630, target[0].ParentId);
            Assert.AreEqual(6255146, target[0].ChildId);
            Assert.AreEqual("ADM", target[0].Type);

            //Zero-child
            Assert.AreEqual(6255149, target[1].ParentId);
            Assert.AreEqual(0, target[1].ChildId);
            Assert.AreEqual("ADM", target[1].Type);

            //No-type
            Assert.AreEqual(672027, target[2].ParentId);
            Assert.AreEqual(663875, target[2].ChildId);
            Assert.AreEqual(string.Empty, target[2].Type);

            //-1 parent
            Assert.AreEqual(-1, target[3].ParentId);
            Assert.AreEqual(3623365, target[3].ChildId);
            Assert.AreEqual(string.Empty, target[3].Type);
        }

        [TestMethod]
        public void IsoLanguageCodeParser_ParsesFileCorrectly()
        {
            var target = GeoFileReader.ReadISOLanguageCodes(@"testdata\test_iso-languagecodes.txt").ToArray();
            Assert.AreEqual(2, target.Length);  //First line in file should've been skipped

            
            Assert.AreEqual("alw", target[0].ISO_639_3);
            Assert.AreEqual(string.Empty, target[0].ISO_639_2);
            Assert.AreEqual(string.Empty, target[0].ISO_639_1);
            Assert.AreEqual("Alaba-K’abeena", target[0].LanguageName);

            Assert.AreEqual("zul", target[1].ISO_639_3); 
            Assert.AreEqual("zul", target[1].ISO_639_2);    
            Assert.AreEqual("zu", target[1].ISO_639_1);
            Assert.AreEqual("Zulu", target[1].LanguageName);
        }

        //TODO: TimeZoneParser

        [TestMethod]
        public void UserTagParser_ParsesFileCorrectly()
        {
            var target = GeoFileReader.ReadUserTags(@"testdata\test_userTags.txt").ToArray();
            Assert.AreEqual(3, target.Length);

            //"Normal" tag
            Assert.AreEqual(2599253, target[0].GeoNameId);
            Assert.AreEqual("opengeodb", target[0].Tag);

            //Tags contain all sorts of randum stuff like URLs
            Assert.AreEqual(6255065, target[1].GeoNameId);
            Assert.AreEqual("http://de.wikipedia.org/wiki/Gotthardgebäude", target[1].Tag);

            Assert.AreEqual(6941058, target[2].GeoNameId);
            Assert.AreEqual("lyžařské", target[2].Tag);
        }
        
        [TestMethod]
        public void CustomParser_IsUsedCorrectly()
        {
            var target = new GeoFileReader().ReadRecords<CustomEntity>(@"testdata\test_custom.txt", new CustomParser()).ToArray();

            Assert.AreEqual(2, target.Length);
            CollectionAssert.AreEqual(target[0].Data, target[1].Data);
        }

        [TestMethod]
        public void FileReader_HandlesEmptyFilesCorrectly()
        {
            //Could use ANY entity
            var target1 = new GeoFileReader().ReadRecords<CustomEntity>(@"testdata\emptyfile.txt", new CustomParser()).ToArray();

            Assert.AreEqual(0, target1.Length);

            //Here's a second one
            var target2 = GeoFileReader.ReadExtendedGeoNames(@"testdata\emptyfile.txt").ToArray();

            Assert.AreEqual(0, target2.Length);
        }

        [TestMethod]
        public void FileReader_HandlesGZippedFilesCorrectly()
        {
            var target = GeoFileReader.ReadCountryInfo(@"testdata\countryInfo_gzip_compat.txt.gz").ToArray();

            Assert.AreEqual(2, target.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]

        public void FileReader_ThrowsOnIncompatibleOrInvalidGZipFiles()
        {
            var target = GeoFileReader.ReadCountryInfo(@"testdata\countryInfo_not_gzip_compat.txt.gz").ToArray();
        }

        [TestMethod]
        public void FileReader_CanReadBuiltInContinentsCorrectly()
        {
            var target = GeoFileReader.ReadBuiltInContinents().ToArray();

            Assert.AreEqual(7, target.Length);
            CollectionAssert.AreEqual(target.OrderBy(c => c.Code).Select(c => c.Code).ToArray(), new[] { "AF", "AN", "AS", "EU", "NA", "OC", "SA" });
        }

        [TestMethod]
        public void FileReader_CanReadBuiltInFeatureClassesCorrectly()
        {
            var target = GeoFileReader.ReadBuiltInFeatureClasses().ToArray();

            Assert.AreEqual(9, target.Length);
            CollectionAssert.AreEqual(target.OrderBy(c => c.Class).Select(c => c.Class).ToArray(), new[] { "A", "H", "L", "P", "R", "S", "T", "U", "V" });
        }

        private class CustomParser : IParser<CustomEntity>
        {
            public bool HasComments { get { return true; } }
            public int SkipLines { get { return 5; } }
            public int ExpectedNumberOfFields { get { return 19; } }
            public Encoding Encoding { get { return Encoding.UTF7; } }
            public char[] FieldSeparators { get { return new[] { '☃' }; } }

            public CustomEntity Parse(string[] fields)
            {
                return new CustomEntity { Data = fields };
            }
        }

        private class CustomEntity
        {
            public string[] Data { get; set; }
        }
    }
}
