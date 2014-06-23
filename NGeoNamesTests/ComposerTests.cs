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

        [TestMethod]
        public void ExtendedGeoNamesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_extendedgeonames.txt";
            var dst = @"testdata\test_extendedgeonames.out";

            GeoFileWriter.WriteExtendedGeoNames(dst, GeoFileReader.ReadExtendedGeoNames(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 19, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void FeatureClassComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_featureClasses_en.txt";
            var dst = @"testdata\test_featureClasses_en.out";

            GeoFileWriter.WriteFeatureClasses(dst, GeoFileReader.ReadFeatureClasses(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 2, 0, new[] { '\t' }, Encoding.UTF8, true);
        }

        [TestMethod]
        public void FeatureCodeComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_featureCodes_en.txt";
            var dst = @"testdata\test_featureCodes_en.out";

            GeoFileWriter.WriteFeatureCodes(dst, GeoFileReader.ReadFeatureCodes(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 3, 0, new[] { '\t' }, Encoding.UTF8, true);
        }

        [TestMethod]
        public void GeoNamesComposerSimple_ComposesFileCorrectly()
        {
            // In this test we test the "simple file format" (e.g. GeoName, not ExtendedGeoName)
            var src = @"testdata\test_geonames_simple.txt";
            var dst = @"testdata\test_geonames_simple.out";

            GeoFileWriter.WriteGeoNames(dst, GeoFileReader.ReadGeoNames(src, false), false);

            EnsureFilesAreFunctionallyEqual(src, dst, 4, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void GeoNamesComposerExtended_ComposesFileCorrectly()
        {
            // In this test we test the "extended file format" (e.g. ExtendedGeoName, not GeoName)
            // But since GeoName cannot provide all values, all other properties should be null/empty when writing
            var src = @"testdata\test_geonames_ext.txt";
            var dst = @"testdata\test_geonames_ext.out";

            GeoFileWriter.WriteGeoNames(dst, GeoFileReader.ReadGeoNames(src, true), true);

            EnsureFilesAreFunctionallyEqual(src, dst, 19, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void HierarchyComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_hierarchy.txt";
            var dst = @"testdata\test_hierarchy.out";

            GeoFileWriter.WriteHierarchy(dst, GeoFileReader.ReadHierarchy(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 3, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void ISOLanguageCodeComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_iso-languagecodes.txt";
            var dst = @"testdata\test_iso-languagecodes.out";

            GeoFileWriter.WriteISOLanguageCodes(dst, GeoFileReader.ReadISOLanguageCodes(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 4, 1, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void TimeZoneComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_timeZones.txt";
            var dst = @"testdata\test_timeZones.out";

            GeoFileWriter.WriteTimeZones(dst, GeoFileReader.ReadTimeZones(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 5, 1, new[] { '\t' }, Encoding.UTF8, false);
        }


        [TestMethod]
        public void UserTagComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_userTags.txt";
            var dst = @"testdata\test_userTags.out";

            GeoFileWriter.WriteUserTags(dst, GeoFileReader.ReadUserTags(src));

            EnsureFilesAreFunctionallyEqual(src, dst, 2, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void CustomComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_custom.txt";
            var dst = @"testdata\test_custom.out";

            new GeoFileWriter().WriteRecords<CustomEntity>(dst, new GeoFileReader().ReadRecords<CustomEntity>(src, new CustomParser(19, 5, new[] { '☃' }, Encoding.UTF7, true)), new CustomComposer(Encoding.UTF7, '☃'));

            EnsureFilesAreFunctionallyEqual(src, dst, 19, 5, new[] { '☃' }, Encoding.UTF7, true);
        }

        // Compares two data files using a GenericEntity to easily compare actual values without bothering with newline differences, 
        // comments etc. nor trying to "understand" what they mean
        private void EnsureFilesAreFunctionallyEqual(string src, string dst, int expectedfields, int skiplines, char[] fieldseparators, Encoding encoding, bool hascomments)
        {
            var parser_in = new GenericParser(expectedfields, skiplines, fieldseparators, encoding, hascomments);
            var parser_out = new GenericParser(expectedfields, 0, fieldseparators, encoding, false);

            var expected = new GeoFileReader().ReadRecords<GenericEntity>(src, FileType.Plain, parser_in).ToArray();
            var actual = new GeoFileReader().ReadRecords<GenericEntity>(dst, FileType.Plain, parser_out).ToArray();

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
