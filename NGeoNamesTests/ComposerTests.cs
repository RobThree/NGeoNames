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
            var dst = @"testdata\test_admin1CodesASCII.out.txt";

            GeoFileWriter.WriteAdmin1Codes(dst, GeoFileReader.ReadAdmin1Codes(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 4, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void Admin2CodesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_admin2Codes.txt";
            var dst = @"testdata\test_admin2Codes.out.txt";

            GeoFileWriter.WriteAdmin2Codes(dst, GeoFileReader.ReadAdmin2Codes(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 4, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void AlternateNamesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_alternateNames.txt";
            var dst = @"testdata\test_alternateNames.out.txt";

            GeoFileWriter.WriteAlternateNames(dst, GeoFileReader.ReadAlternateNames(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 8, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void ContinentComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_continentCodes.txt";
            var dst = @"testdata\test_continentCodes.out.txt";

            GeoFileWriter.WriteContinents(dst, GeoFileReader.ReadContinents(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 3, 0, new[] { '\t' }, Encoding.UTF8, true);
        }

        [TestMethod]
        public void CountryInfoComposer_ComposesFileCorrectly()
        {
            // We use a *slightly* different file (test_countryInfo2 instead of test_countryInfo) because the
            // CountryInfoParser "fixes" phonenumbers with a missing + (e.g. 31 vs. +31); this way the files
            // would always differ; test_countryInfo2 has these values fixed
            var src = @"testdata\test_countryInfo2.txt";
            var dst = @"testdata\test_countryInfo2.out.txt";

            GeoFileWriter.WriteCountryInfo(dst, GeoFileReader.ReadCountryInfo(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 19, 0, new[] { '\t' }, Encoding.UTF8, true);
        }

        [TestMethod]
        public void ExtendedGeoNamesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_extendedgeonames.txt";
            var dst = @"testdata\test_extendedgeonames.out.txt";

            GeoFileWriter.WriteExtendedGeoNames(dst, GeoFileReader.ReadExtendedGeoNames(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 19, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void FeatureClassComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_featureClasses_en.txt";
            var dst = @"testdata\test_featureClasses_en.out.txt";

            GeoFileWriter.WriteFeatureClasses(dst, GeoFileReader.ReadFeatureClasses(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 2, 0, new[] { '\t' }, Encoding.UTF8, true);
        }

        [TestMethod]
        public void FeatureCodeComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_featureCodes_en.txt";
            var dst = @"testdata\test_featureCodes_en.out.txt";

            GeoFileWriter.WriteFeatureCodes(dst, GeoFileReader.ReadFeatureCodes(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 3, 0, new[] { '\t' }, Encoding.UTF8, true);
        }

        [TestMethod]
        public void GeoNamesComposerSimple_ComposesFileCorrectly()
        {
            // In this test we test the "compact file format" (e.g. GeoName, not ExtendedGeoName)
            var src = @"testdata\test_geonames_simple.txt";
            var dst = @"testdata\test_geonames_simple.out.txt";

            GeoFileWriter.WriteGeoNames(dst, GeoFileReader.ReadGeoNames(src, false), false);

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 4, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void GeoNamesComposerExtended_ComposesFileCorrectly()
        {
            // In this test we test the "extended file format" (e.g. ExtendedGeoName, not GeoName)
            // But since GeoName cannot provide all values, all other properties should be null/empty when writing
            var src = @"testdata\test_geonames_ext.txt";
            var dst = @"testdata\test_geonames_ext.out.txt";

            GeoFileWriter.WriteGeoNames(dst, GeoFileReader.ReadGeoNames(src, true), true);

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 19, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void HierarchyComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_hierarchy.txt";
            var dst = @"testdata\test_hierarchy.out.txt";

            GeoFileWriter.WriteHierarchy(dst, GeoFileReader.ReadHierarchy(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 3, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void ISOLanguageCodeComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_iso-languagecodes.txt";
            var dst = @"testdata\test_iso-languagecodes.out.txt";

            GeoFileWriter.WriteISOLanguageCodes(dst, GeoFileReader.ReadISOLanguageCodes(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 4, 1, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void TimeZoneComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_timeZones.txt";
            var dst = @"testdata\test_timeZones.out.txt";

            GeoFileWriter.WriteTimeZones(dst, GeoFileReader.ReadTimeZones(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 5, 1, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void UserTagComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_userTags.txt";
            var dst = @"testdata\test_userTags.out.txt";

            GeoFileWriter.WriteUserTags(dst, GeoFileReader.ReadUserTags(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 2, 0, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public void CustomComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_custom.txt";
            var dst = @"testdata\test_custom.out.txt";

            new GeoFileWriter().WriteRecords<CustomEntity>(dst, new GeoFileReader().ReadRecords<CustomEntity>(src, new CustomParser(19, 5, new[] { '☃' }, Encoding.UTF7, true)), new CustomComposer(Encoding.UTF7, '☃'));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 19, 5, new[] { '☃' }, Encoding.UTF7, true);
        }

        [TestMethod]
        public void Composer_HandlesGZippedFilesCorrectly()
        {
            var src = @"testdata\test_extendedgeonames.txt";
            var dst = @"testdata\test_extendedgeonames.out.gz";

            GeoFileWriter.WriteExtendedGeoNames(dst, GeoFileReader.ReadExtendedGeoNames(src));

            Assert.AreEqual(7, GeoFileReader.ReadExtendedGeoNames(dst).Count());
        }
    }
}
