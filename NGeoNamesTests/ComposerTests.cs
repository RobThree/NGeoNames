using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;
using NGeoNames.Parsers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGeoNamesTests
{
    [TestClass]
    public class ComposerTests
    {
        [TestMethod]
        public void Admin1CodesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_admin1CodesASCII.txt";
            var dst = @"testdata\test_admin1CodesASCII.out";

            GeoFileWriter.WriteAdmin1Codes(dst, GeoFileReader.ReadAdmin1Codes(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 4, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void Admin2CodesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_admin2Codes.txt";
            var dst = @"testdata\test_admin2Codes.out";

            GeoFileWriter.WriteAdmin2Codes(dst, GeoFileReader.ReadAdmin2Codes(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 4, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void AlternateNamesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_alternateNames.txt";
            var dst = @"testdata\test_alternateNames.out";

            GeoFileWriter.WriteAlternateNames(dst, GeoFileReader.ReadAlternateNames(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 8, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void ContinentComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_continentCodes.txt";
            var dst = @"testdata\test_continentCodes.out";

            GeoFileWriter.WriteContinents(dst, GeoFileReader.ReadContinents(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 3, 0, new[] { '\t' }, Encoding.UTF8, true);
        }

        [TestMethod]
        public void CountryInfoComposer_ComposesFileCorrectly()
        {
            // We use a *slightly* different file (test_countryInfo2 instead of test_countryInfo) because the
            // CountryInfoParser "fixes" phonenumbers with a missing + (e.g. 31 vs. +31); this way the files
            // would always differ; test_countryInfo2 has these values fixed
            var src = @"testdata\test_countryInfo2.txt";
            var dst = @"testdata\test_countryInfo2.out";

            GeoFileWriter.WriteCountryInfo(dst, GeoFileReader.ReadCountryInfo(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 19, 0, new[] { '\t' }, Encoding.UTF8, true);
        }

        //TODO: ExtendedGeoNameComposer
        //TODO: FeatureClassComposer
        //TODO: FeatureCodeComposer
        //TODO: GeoNameComposer
        //TODO: HierarchyComposer
        //TODO: ISOLanguageCodeComposer
        //TODO: TimeZoneComposer
        //TODO: UserTagComposer

        //Compares two data files using a GenericEntity to easily compare actual values without bothering with newline differences, comments etc.
        private void EnsureFilesAreFunctionallyEqual(string src, string dst, int expectedfields, int skiplines, char[] fieldseparators, Encoding encoding, bool hascomments)
        {
            var parser = new GenericParser(expectedfields, skiplines, fieldseparators, encoding, hascomments);

            var expected = new GeoFileReader().ReadRecords<GenericEntity>(src, FileType.Plain, parser).ToArray();
            var actual = new GeoFileReader().ReadRecords<GenericEntity>(dst, FileType.Plain, parser).ToArray();

            CollectionAssert.AreEqual(expected, actual, new GenericEntityComparer());
        }

        private class GenericEntityComparer : IComparer<GenericEntity>, IComparer
        {
            public int Compare(GenericEntity x, GenericEntity y)
            {
                int r = x.Data.Length.CompareTo(y.Data.Length);
                if (r != 0)
                    return r;

                for (int i = 0; i < x.Data.Length && r == 0; i++)
                    r = x.Data[i].CompareTo(y.Data[i]);
                return r;
            }

            public int Compare(object x, object y)
            {
                return this.Compare(x as GenericEntity, y as GenericEntity);
            }
        }


        private class GenericParser : IParser<GenericEntity>
        {
            public bool HasComments { get; private set; }
            public int SkipLines { get; private set; }
            public int ExpectedNumberOfFields { get; private set; }
            public Encoding Encoding { get; private set; }
            public char[] FieldSeparators { get; private set; }

            public GenericParser(int expectedfields, int skiplines, char[] fieldseparators, Encoding encoding, bool hascomments)
            {
                this.SkipLines = skiplines;
                this.ExpectedNumberOfFields = expectedfields;
                this.FieldSeparators = fieldseparators;
                this.Encoding = encoding;
                this.HasComments = hascomments;
            }

            public GenericEntity Parse(string[] fields)
            {
                Assert.AreEqual(this.ExpectedNumberOfFields, fields.Length);
                return new GenericEntity { Data = fields };
            }
        }

        private class GenericEntity
        {
            public string[] Data { get; set; }
        }
    }
}
