using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;
using NGeoNames.Entities;
using NGeoNames.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Assert.AreEqual(target[0].Code, "CF.04");
            Assert.AreEqual(target[0].Name, "Mambéré-Kadéï");
            Assert.AreEqual(target[0].NameASCII, "Mambere-Kadei");
            Assert.AreEqual(target[0].GeoNameId, 2386161);

            Assert.AreEqual(target[1].Code, "This.is.a.VERY.LONG.ID");
            Assert.AreEqual(target[1].Name, "Iğdır");
            Assert.AreEqual(target[1].NameASCII, "Igdir");
            Assert.AreEqual(target[1].GeoNameId, 0);

            Assert.AreEqual(target[2].Code, "TR.80");
            Assert.AreEqual(target[2].Name, "Şırnak");
            Assert.AreEqual(target[2].NameASCII, "Sirnak");
            Assert.AreEqual(target[2].GeoNameId, 443189);

            Assert.AreEqual(target[3].Code, "UA.26");
            Assert.AreEqual(target[3].Name, "Zaporiz’ka Oblast’");
            Assert.AreEqual(target[3].NameASCII, "Zaporiz'ka Oblast'");
            Assert.AreEqual(target[3].GeoNameId, 687699);
        }

        [TestMethod]
        public void Admin2CodesParser_ParsesFileCorrectly()
        {
            var target = GeoFileReader.ReadAdmin2Codes(@"testdata\test_admin2Codes.txt").ToArray();
            Assert.AreEqual(2, target.Length);

            Assert.AreEqual(target[0].Code, "AF.01.7052666");
            Assert.AreEqual(target[0].Name, "Darwāz-e Bālā");
            Assert.AreEqual(target[0].NameASCII, "Darwaz-e Bala");
            Assert.AreEqual(target[0].GeoNameId, 7052666);

            Assert.AreEqual(target[1].Code, "CA.10.11");
            Assert.AreEqual(target[1].Name, "Gaspésie-Îles-de-la-Madeleine");
            Assert.AreEqual(target[1].NameASCII, "Gaspesie-Iles-de-la-Madeleine");
            Assert.AreEqual(target[1].GeoNameId, 0);
        }

        [TestMethod]
        public void CustomParser_IsUsedCorrectly()
        {
            var target = new GeoFileReader().ReadRecords<CustomEntity>(@"testdata\test_custom.txt", new CustomParser()).ToArray();

            Assert.AreEqual(target.Length, 2);
            CollectionAssert.AreEqual(target[0].Data, target[1].Data);
        }

        [TestMethod]
        public void FileReader_HandlesEmptyFilesCorrectly()
        {
            //Could use ANY entity
            var target1 = new GeoFileReader().ReadRecords<CustomEntity>(@"testdata\emptyfile.txt", new CustomParser()).ToArray();

            Assert.AreEqual(target1.Length, 0);

            //Here's a second one
            var target2 = GeoFileReader.ReadExtendedGeoNames(@"testdata\emptyfile.txt").ToArray();

            Assert.AreEqual(target2.Length, 0);
        }

        [TestMethod]
        public void FileReader_HandlesGZippedFilesCorrectly()
        {
            var target = GeoFileReader.ReadCountryInfo(@"testdata\countryInfo_gzip_compat.txt.gz").ToArray();

            Assert.AreEqual(target.Length, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]

        public void FileReader_ThrowsOnIncompatibleOrInvalidGZipFiles()
        {
            var target = GeoFileReader.ReadCountryInfo(@"testdata\countryInfo_not_gzip_compat.txt.gz").ToArray();
        }

        [TestMethod]
        public void FileReader_CanReadBuiltInsCorrectly()
        {
            var target_continents = GeoFileReader.ReadBuiltInContinents().ToArray();
            var target_featureclasses = GeoFileReader.ReadBuiltInFeatureClasses().ToArray();


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
