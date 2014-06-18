using NGeoNames.Entities;
using NGeoNames.Parsers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace NGeoNames
{
    public enum FileType
    {
        AutoDetect = 0,
        Plain = 1,
        GZip = 2
    }

    public class GeoFileReader
    {
        public IEnumerable<T> ReadRecords<T>(string path, IParser<T> parser)
        {
            return ReadRecords(path, FileType.AutoDetect, parser);
        }

        public IEnumerable<T> ReadRecords<T>(string path, FileType filetype, IParser<T> parser)
        {
            using (var f = GetStream(path, filetype))
            {
                foreach (var r in this.ReadRecords(f, parser))
                    yield return r;
            }
        }

        public IEnumerable<T> ReadRecords<T>(Stream stream, IParser<T> parser)
        {
            using (var r = new StreamReader(stream, parser.Encoding))
            {
                string line = null;
                int c = 0;
                char[] separators = parser.FieldSeparators.Clone() as char[];
                while (!r.EndOfStream && (line = r.ReadLine()) != null)
                {
                    c++;
                    if ((c > parser.SkipLines) && (!parser.HasComments || (parser.HasComments && !line.StartsWith("#"))))
                    {
                        var data = line.Split(parser.FieldSeparators);
                        if (data.Length != parser.ExpectedNumberOfFields)
                            throw new ParserException(string.Format("Expected number of fields mismatch; expected: {0}, read: {1}, line: {2}", parser.ExpectedNumberOfFields, data.Length, c));
                        yield return parser.Parse(data);
                    }
                }
            }
        }

        private static Stream GetStream(string path, FileType filetype)
        {
            var filestream = File.OpenRead(path);

            //Figure out how we're supposed to read the file
            FileType readastype = filetype == FileType.AutoDetect ? GetFileTypeFromExtension(path) : filetype;
            switch (readastype)
            {
                case FileType.Plain:
                    return filestream;
                case FileType.GZip:
                    return new GZipStream(filestream, CompressionMode.Decompress);
            }
            throw new System.NotSupportedException(string.Format("Filetype not supported: {0}", readastype));
        }

        private static FileType GetFileTypeFromExtension(string path)
        {
            var fi = new FileInfo(path);
            switch (fi.Extension.ToLowerInvariant())
            {
                case ".txt":
                    return FileType.Plain;
                case ".gz":
                    return FileType.GZip;
            }
            throw new System.NotSupportedException("Unable to detect filetype from file extension");
        }

        #region Convenience methods
        public static IEnumerable<ExtendedGeoName> ReadExtendedGeoNames(string filename)
        {
            return new GeoFileReader().ReadRecords<ExtendedGeoName>(filename, new ExtendedGeoNameParser());
        }

        public static IEnumerable<ExtendedGeoName> ReadExtendedGeoNames(Stream stream)
        {
            return new GeoFileReader().ReadRecords<ExtendedGeoName>(stream, new ExtendedGeoNameParser());
        }

        public static IEnumerable<GeoName> ReadGeoNames(string filename)
        {
            return new GeoFileReader().ReadRecords<GeoName>(filename, new GeoNameParser());
        }

        public static IEnumerable<GeoName> ReadGeoNames(Stream stream)
        {
            return new GeoFileReader().ReadRecords<GeoName>(stream, new GeoNameParser());
        }

        public static IEnumerable<Admin1Code> ReadAdmin1Codes(string filename)
        {
            return new GeoFileReader().ReadRecords<Admin1Code>(filename, new Admin1CodeParser());
        }

        public static IEnumerable<Admin1Code> ReadAdmin1Codes(Stream stream)
        {
            return new GeoFileReader().ReadRecords<Admin1Code>(stream, new Admin1CodeParser());
        }

        public static IEnumerable<Admin2Code> ReadAdmin2Codes(string filename)
        {
            return new GeoFileReader().ReadRecords<Admin2Code>(filename, new Admin2CodeParser());
        }

        public static IEnumerable<Admin2Code> ReadAdmin2Codes(Stream stream)
        {
            return new GeoFileReader().ReadRecords<Admin2Code>(stream, new Admin2CodeParser());
        }

        public static IEnumerable<AlternateName> ReadAlternateNames(string filename)
        {
            return new GeoFileReader().ReadRecords<AlternateName>(filename, new AlternateNameParser());
        }

        public static IEnumerable<AlternateName> ReadAlternateNames(Stream stream)
        {
            return new GeoFileReader().ReadRecords<AlternateName>(stream, new AlternateNameParser());
        }

        public static IEnumerable<Continent> ReadBuiltInContinents()
        {
            return ReadBuiltInResource<Continent>("continentCodes", new ContinentParser());
        }

        private static IEnumerable<T> ReadBuiltInResource<T>(string name, IParser<T> parser)
        {
            using (var s = new MemoryStream(parser.Encoding.GetBytes(Properties.Resources.ResourceManager.GetString(name))))
                foreach (var i in new GeoFileReader().ReadRecords<T>(s, parser))
                    yield return i;
        }

        public static IEnumerable<Continent> ReadContinents(string filename)
        {
            return new GeoFileReader().ReadRecords<Continent>(filename, new ContinentParser());
        }

        public static IEnumerable<Continent> ReadContinents(Stream stream)
        {
            return new GeoFileReader().ReadRecords<Continent>(stream, new ContinentParser());
        }

        public static IEnumerable<CountryInfo> ReadCountryInfo(string filename)
        {
            return new GeoFileReader().ReadRecords<CountryInfo>(filename, new CountryInfoParser());
        }

        public static IEnumerable<CountryInfo> ReadCountryInfo(Stream stream)
        {
            return new GeoFileReader().ReadRecords<CountryInfo>(stream, new CountryInfoParser());
        }

        public static IEnumerable<FeatureClass> ReadBuiltInFeatureClasses()
        {
            return ReadBuiltInResource<FeatureClass>("featureClasses_en", new FeatureClassParser());
        }

        public static IEnumerable<FeatureClass> ReadFeatureClasses(string filename)
        {
            return new GeoFileReader().ReadRecords<FeatureClass>(filename, new FeatureClassParser());
        }

        public static IEnumerable<FeatureClass> ReadFeatureClasses(Stream stream)
        {
            return new GeoFileReader().ReadRecords<FeatureClass>(stream, new FeatureClassParser());
        }

        public static IEnumerable<FeatureCode> ReadFeatureCodes(string filename)
        {
            return new GeoFileReader().ReadRecords<FeatureCode>(filename, new FeatureCodeParser());
        }

        public static IEnumerable<FeatureCode> ReadFeatureCodes(Stream stream)
        {
            return new GeoFileReader().ReadRecords<FeatureCode>(stream, new FeatureCodeParser());
        }

        public static IEnumerable<HierarchyNode> ReadHierarchy(string filename)
        {
            return new GeoFileReader().ReadRecords<HierarchyNode>(filename, new HierarchyParser());
        }

        public static IEnumerable<HierarchyNode> ReadHierarchy(Stream stream)
        {
            return new GeoFileReader().ReadRecords<HierarchyNode>(stream, new HierarchyParser());
        }

        public static IEnumerable<ISOLanguageCode> ReadISOLanguageCodes(string filename)
        {
            return new GeoFileReader().ReadRecords<ISOLanguageCode>(filename, new ISOLanguageCodeParser());
        }

        public static IEnumerable<ISOLanguageCode> ReadISOLanguageCodes(Stream stream)
        {
            return new GeoFileReader().ReadRecords<ISOLanguageCode>(stream, new ISOLanguageCodeParser());
        }

        public static IEnumerable<TimeZone> ReadTimeZones(string filename)
        {
            return new GeoFileReader().ReadRecords<TimeZone>(filename, new TimeZoneParser());
        }

        public static IEnumerable<TimeZone> ReadTimeZones(Stream stream)
        {
            return new GeoFileReader().ReadRecords<TimeZone>(stream, new TimeZoneParser());
        }

        public static IEnumerable<UserTag> ReadUserTags(string filename)
        {
            return new GeoFileReader().ReadRecords<UserTag>(filename, new UserTagParser());
        }

        public static IEnumerable<UserTag> ReadUserTags(Stream stream)
        {
            return new GeoFileReader().ReadRecords<UserTag>(stream, new UserTagParser());
        }
        #endregion
    }
}