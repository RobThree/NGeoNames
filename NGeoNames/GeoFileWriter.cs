using NGeoNames.Composers;
using NGeoNames.Entities;
using System.Collections.Generic;
using System.IO;

namespace NGeoNames
{
    public class GeoFileWriter
    {
        public const string DEFAULTLINESEPARATOR = "\n";

        public void WriteRecords<T>(string path, IEnumerable<T> values, IComposer<T> composer, string lineseparator = DEFAULTLINESEPARATOR)
        {
            using (var s = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                this.WriteRecords(s, values, composer, lineseparator);
        }

        public void WriteRecords<T>(Stream stream, IEnumerable<T> values, IComposer<T> composer, string lineseparator = DEFAULTLINESEPARATOR)
        {
            using (var w = new StreamWriter(stream, composer.Encoding))
            {
                foreach (var v in values)
                {
                    w.Write(composer.Compose(v) + lineseparator);
                }
            }
        }

        #region Convenience methods
        public static void WriteAdmin1Codes(string filename, IEnumerable<Admin1Code> values)
        {
            new GeoFileWriter().WriteRecords<Admin1Code>(filename, values, new Admin1CodeComposer());
        }

        public static void WriteAdmin1Codes(Stream stream, IEnumerable<Admin1Code> values)
        {
            new GeoFileWriter().WriteRecords<Admin1Code>(stream, values, new Admin1CodeComposer());
        }

        public static void WriteAdmin2Codes(string filename, IEnumerable<Admin2Code> values)
        {
            new GeoFileWriter().WriteRecords<Admin2Code>(filename, values, new Admin2CodeComposer());
        }

        public static void WriteAdmin2Codes(Stream stream, IEnumerable<Admin2Code> values)
        {
            new GeoFileWriter().WriteRecords<Admin2Code>(stream, values, new Admin2CodeComposer());
        }

        public static void WriteAlternateNames(string filename, IEnumerable<AlternateName> values)
        {
            new GeoFileWriter().WriteRecords<AlternateName>(filename, values, new AlternateNameComposer());
        }

        public static void WriteAlternateNames(Stream stream, IEnumerable<AlternateName> values)
        {
            new GeoFileWriter().WriteRecords<AlternateName>(stream, values, new AlternateNameComposer());
        }

        public static void WriteContinents(string filename, IEnumerable<Continent> values)
        {
            new GeoFileWriter().WriteRecords<Continent>(filename, values, new ContinentComposer());
        }

        public static void WriteContinents(Stream stream, IEnumerable<Continent> values)
        {
            new GeoFileWriter().WriteRecords<Continent>(stream, values, new ContinentComposer());
        }

        public static void WriteCountryInfo(string filename, IEnumerable<CountryInfo> values)
        {
            new GeoFileWriter().WriteRecords<CountryInfo>(filename, values, new CountryInfoComposer());
        }

        public static void WriteCountryInfo(Stream stream, IEnumerable<CountryInfo> values)
        {
            new GeoFileWriter().WriteRecords<CountryInfo>(stream, values, new CountryInfoComposer());
        }

        public static void WriteExtendedGeoNames(string filename, IEnumerable<ExtendedGeoName> values)
        {
            new GeoFileWriter().WriteRecords<ExtendedGeoName>(filename, values, new ExtendedGeoNameComposer());
        }

        public static void WriteExtendedGeoNames(Stream stream, IEnumerable<ExtendedGeoName> values)
        {
            new GeoFileWriter().WriteRecords<ExtendedGeoName>(stream, values, new ExtendedGeoNameComposer());
        }

        public static void WriteFeatureClasses(string filename, IEnumerable<FeatureClass> values)
        {
            new GeoFileWriter().WriteRecords<FeatureClass>(filename, values, new FeatureClassComposer());
        }

        public static void WriteFeatureClasses(Stream stream, IEnumerable<FeatureClass> values)
        {
            new GeoFileWriter().WriteRecords<FeatureClass>(stream, values, new FeatureClassComposer());
        }

        public static void WriteFeatureCodes(string filename, IEnumerable<FeatureCode> values)
        {
            new GeoFileWriter().WriteRecords<FeatureCode>(filename, values, new FeatureCodeComposer());
        }

        public static void WriteFeatureCodes(Stream stream, IEnumerable<FeatureCode> values)
        {
            new GeoFileWriter().WriteRecords<FeatureCode>(stream, values, new FeatureCodeComposer());
        }

        public static void WriteGeoNames(string filename, IEnumerable<GeoName> values)
        {
            WriteGeoNames(filename, values, false);
        }

        public static void WriteGeoNames(Stream stream, IEnumerable<GeoName> values)
        {
            WriteGeoNames(stream, values, false);
        }

        public static void WriteGeoNames(string filename, IEnumerable<GeoName> values, bool useextendedfileformat)
        {
            new GeoFileWriter().WriteRecords<GeoName>(filename, values, new GeoNameComposer(useextendedfileformat));
        }

        public static void WriteGeoNames(Stream stream, IEnumerable<GeoName> values, bool useextendedfileformat)
        {
            new GeoFileWriter().WriteRecords<GeoName>(stream, values, new GeoNameComposer(useextendedfileformat));
        }


        public static void WriteHierarchy(string filename, IEnumerable<HierarchyNode> values)
        {
            new GeoFileWriter().WriteRecords<HierarchyNode>(filename, values, new HierarchyComposer());
        }

        public static void WriteHierarchy(Stream stream, IEnumerable<HierarchyNode> values)
        {
            new GeoFileWriter().WriteRecords<HierarchyNode>(stream, values, new HierarchyComposer());
        }

        public static void WriteISOLanguageCodes(string filename, IEnumerable<ISOLanguageCode> values)
        {
            new GeoFileWriter().WriteRecords<ISOLanguageCode>(filename, values, new ISOLanguageCodeComposer());
        }

        public static void WriteISOLanguageCodes(Stream stream, IEnumerable<ISOLanguageCode> values)
        {
            new GeoFileWriter().WriteRecords<ISOLanguageCode>(stream, values, new ISOLanguageCodeComposer());
        }

        public static void WriteTimeZones(string filename, IEnumerable<TimeZone> values)
        {
            new GeoFileWriter().WriteRecords<TimeZone>(filename, values, new TimeZoneComposer());
        }

        public static void WriteTimeZones(Stream stream, IEnumerable<TimeZone> values)
        {
            new GeoFileWriter().WriteRecords<TimeZone>(stream, values, new TimeZoneComposer());
        }

        public static void WriteUserTags(string filename, IEnumerable<UserTag> values)
        {
            new GeoFileWriter().WriteRecords<UserTag>(filename, values, new UserTagComposer());
        }

        public static void WriteUserTags(Stream stream, IEnumerable<UserTag> values)
        {
            new GeoFileWriter().WriteRecords<UserTag>(stream, values, new UserTagComposer());
        }
        #endregion
    }
}
