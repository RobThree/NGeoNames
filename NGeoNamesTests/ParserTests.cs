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
