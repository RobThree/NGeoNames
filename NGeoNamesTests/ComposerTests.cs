using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGeoNamesTests
{
    [TestClass]
    public class ComposerTests
    {
        [TestMethod]
        public async Task Admin1CodesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_admin1CodesASCII.txt";
            var dst = @"testdata\test_admin1CodesASCII.out.txt";

            await GeoFileWriter.WriteAdmin1Codes(dst, GeoFileReader.ReadAdmin1Codes(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 4, 0, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task Admin2CodesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_admin2Codes.txt";
            var dst = @"testdata\test_admin2Codes.out.txt";

            await GeoFileWriter.WriteAdmin2Codes(dst, GeoFileReader.ReadAdmin2Codes(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 4, 0, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task AlternateNamesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_alternateNames.txt";
            var dst = @"testdata\test_alternateNames.out.txt";

            await GeoFileWriter.WriteAlternateNames(dst, GeoFileReader.ReadAlternateNames(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 8, 0, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task AlternateNamesComposerV2_ComposesFileCorrectly()
        {
            var src = @"testdata\test_alternateNamesV2.txt";
            var dst = @"testdata\test_alternateNamesV2.out.txt";

            await GeoFileWriter.WriteAlternateNamesV2(dst, GeoFileReader.ReadAlternateNamesV2(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 10, 0, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ContinentComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_continentCodes.txt";
            var dst = @"testdata\test_continentCodes.out.txt";

            await GeoFileWriter.WriteContinents(dst, GeoFileReader.ReadContinents(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 3, 0, new[] { '\t' }, Encoding.UTF8, true).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task CountryInfoComposer_ComposesFileCorrectly()
        {
            // We use a *slightly* different file (test_countryInfo2 instead of test_countryInfo) because the
            // CountryInfoParser "fixes" phonenumbers with a missing + (e.g. 31 vs. +31); this way the files
            // would always differ; test_countryInfo2 has these values fixed
            var src = @"testdata\test_countryInfo2.txt";
            var dst = @"testdata\test_countryInfo2.out.txt";

            await GeoFileWriter.WriteCountryInfo(dst, GeoFileReader.ReadCountryInfo(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 19, 0, new[] { '\t' }, Encoding.UTF8, true).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ExtendedGeoNamesComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_extendedgeonames.txt";
            var dst = @"testdata\test_extendedgeonames.out.txt";

            await GeoFileWriter.WriteExtendedGeoNames(dst, GeoFileReader.ReadExtendedGeoNames(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 19, 0, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task FeatureClassComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_featureClasses_en.txt";
            var dst = @"testdata\test_featureClasses_en.out.txt";

            await GeoFileWriter.WriteFeatureClasses(dst, GeoFileReader.ReadFeatureClasses(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 2, 0, new[] { '\t' }, Encoding.UTF8, true).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task FeatureCodeComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_featureCodes_en.txt";
            var dst = @"testdata\test_featureCodes_en.out.txt";

            await GeoFileWriter.WriteFeatureCodes(dst, GeoFileReader.ReadFeatureCodes(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 3, 0, new[] { '\t' }, Encoding.UTF8, true).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GeoNamesComposerSimple_ComposesFileCorrectly()
        {
            // In this test we test the "compact file format" (e.g. GeoName, not ExtendedGeoName)
            var src = @"testdata\test_geonames_simple.txt";
            var dst = @"testdata\test_geonames_simple.out.txt";

            await GeoFileWriter.WriteGeoNames(dst, GeoFileReader.ReadGeoNames(src, false), false);

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 4, 0, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GeoNamesComposerExtended_ComposesFileCorrectly()
        {
            // In this test we test the "extended file format" (e.g. ExtendedGeoName, not GeoName)
            // But since GeoName cannot provide all values, all other properties should be null/empty when writing
            var src = @"testdata\test_geonames_ext.txt";
            var dst = @"testdata\test_geonames_ext.out.txt";

            await GeoFileWriter.WriteGeoNames(dst, GeoFileReader.ReadGeoNames(src, true), true);

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 19, 0, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task HierarchyComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_hierarchy.txt";
            var dst = @"testdata\test_hierarchy.out.txt";

            await GeoFileWriter.WriteHierarchy(dst, GeoFileReader.ReadHierarchy(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 3, 0, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ISOLanguageCodeComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_iso-languagecodes.txt";
            var dst = @"testdata\test_iso-languagecodes.out.txt";

            await GeoFileWriter.WriteISOLanguageCodes(dst, GeoFileReader.ReadISOLanguageCodes(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 4, 1, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task TimeZoneComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_timeZones.txt";
            var dst = @"testdata\test_timeZones.out.txt";

            await GeoFileWriter.WriteTimeZones(dst, GeoFileReader.ReadTimeZones(src));

            FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 5, 1, new[] { '\t' }, Encoding.UTF8, false);
        }

        [TestMethod]
        public async Task UserTagComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_userTags.txt";
            var dst = @"testdata\test_userTags.out.txt";

            await GeoFileWriter.WriteUserTags(dst, GeoFileReader.ReadUserTags(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 2, 0, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task PostalcodeComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_postalCodes.txt";
            var dst = @"testdata\test_postalCodes.out.txt";

            await GeoFileWriter.WritePostalcodes(dst, GeoFileReader.ReadPostalcodes(src));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 12, 0, new[] { '\t' }, Encoding.UTF8, false).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task CustomComposer_ComposesFileCorrectly()
        {
            var src = @"testdata\test_custom.txt";
            var dst = @"testdata\test_custom.out.txt";

            await GeoFileWriter.WriteRecordsAsync(dst, GeoFileReader.ReadRecordsAsync(src, new CustomParser(19, 5, new[] { '☃' }, Encoding.UTF7, true)), new CustomComposer(Encoding.UTF7, '☃'));

            await FileUtil.EnsureFilesAreFunctionallyEqual(src, dst, 19, 5, new[] { '☃' }, Encoding.UTF7, true).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task Composer_HandlesGZippedFilesCorrectly()
        {
            var src = @"testdata\test_extendedgeonames.txt";
            var dst = @"testdata\test_extendedgeonames.out.gz";

            await GeoFileWriter.WriteExtendedGeoNames(dst, GeoFileReader.ReadExtendedGeoNames(src));

            Assert.AreEqual(7, await GeoFileReader.ReadExtendedGeoNames(dst).CountAsync());
        }
    }
}
