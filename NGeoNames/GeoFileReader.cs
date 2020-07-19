using NGeoNames.Entities;
using NGeoNames.Parsers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Linq;

namespace NGeoNames
{
    /// <summary>
    /// Provides methods to read/parse files from geonames.org.
    /// </summary>
    public static class GeoFileReader
    {
        /// <summary>
        /// Reads records of type T, using the specified parser to parse the values.
        /// </summary>
        /// <typeparam name="T">The type of objects to read/parse.</typeparam>
        /// <param name="path">The path of the file to read/parse.</param>
        /// <param name="parser">The <see cref="IParser{T}"/> to use when reading the file.</param>
        /// <returns>Returns an IEnumerable of T representing the records read/parsed.</returns>
        /// <remarks>
        /// This method will try to "autodetect" the filetype; it will 'recognize' .txt and .gz (or .*.gz) files
        /// and act accordingly. If you use another extension you may want to explicitly specify the filetype
        /// using the <see cref="ReadRecordsAsync{T}(string, FileType, IParser{T})"/> overload.
        /// </remarks>
        public static IAsyncEnumerable<T> ReadRecordsAsync<T>(string path, IParser<T> parser)
        {
            return ReadRecordsAsync(path, FileType.AutoDetect, parser);
        }

        /// <summary>
        /// Reads records of type T, using the specified parser to parse the values.
        /// </summary>
        /// <typeparam name="T">The type of objects to read/parse.</typeparam>
        /// <param name="path">The path of the file to read/parse.</param>
        /// <param name="filetype">The <see cref="FileType"/> of the file.</param>
        /// <param name="parser">The <see cref="IParser{T}"/> to use when reading the file.</param>
        /// <returns>Returns an IEnumerable of T representing the records read/parsed.</returns>
        public async static IAsyncEnumerable<T> ReadRecordsAsync<T>(string path, FileType filetype, IParser<T> parser)
        {
            using (var f = GetStream(path, filetype))
                await foreach (var r in ReadRecordsAsync(f, parser))
                    yield return r;
        }

        /// <summary>
        /// Reads records of type T, using the specified parser to parse the values.
        /// </summary>
        /// <typeparam name="T">The type of objects to read/parse.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <param name="parser">The <see cref="IParser{T}"/> to use when reading the file.</param>
        /// <returns>Returns an IEnumerable of T representing the records read/parsed.</returns>
        public static async IAsyncEnumerable<T> ReadRecordsAsync<T>(Stream stream, IParser<T> parser)
        {
            using (var r = new StreamReader(stream, parser.Encoding))
            {
                string line = null;
                int linecount = 0;
                char[] separators = parser.FieldSeparators.Clone() as char[];
                while (!r.EndOfStream && (line = (await r.ReadLineAsync().ConfigureAwait(false))) != null)
                {
                    linecount++;
                    if ((linecount > parser.SkipLines) && (line.Length > 0)  && (!parser.HasComments || (parser.HasComments && !line.StartsWith("#", System.StringComparison.Ordinal))))
                    {
                        var data = line.Split(parser.FieldSeparators);
                        if (data.Length != parser.ExpectedNumberOfFields)
                            throw new ParserException($"Expected number of fields mismatch; expected: {parser.ExpectedNumberOfFields}, read: {data.Length}, line: {linecount}");
                        yield return parser.Parse(data);
                    }
                }
            }
        }

        private static Stream GetStream(string path, FileType filetype)
        {
            var filestream = File.OpenRead(path);

            //Figure out how we're supposed to read the file
            var readastype = filetype == FileType.AutoDetect ? FileUtil.GetFileTypeFromExtension(path) : filetype;
            switch (readastype)
            {
                case FileType.Plain:
                    return filestream;
                case FileType.GZip:
                    return new GZipStream(filestream, CompressionMode.Decompress);
            }
            throw new System.NotSupportedException($"Filetype not supported: {readastype}");
        }


        #region Convenience methods
        /// <summary>
        /// Reads <see cref="ExtendedGeoName"/> records from the specified file, using the default <see cref="ExtendedGeoNameParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="ExtendedGeoName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<ExtendedGeoName> ReadExtendedGeoNames(string filename)
        {
            return ReadRecordsAsync(filename, new ExtendedGeoNameParser());
        }

        /// <summary>
        /// Reads <see cref="ExtendedGeoName"/> records from the <see cref="Stream"/>, using the default <see cref="ExtendedGeoNameParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="ExtendedGeoName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<ExtendedGeoName> ReadExtendedGeoNames(Stream stream)
        {
            return ReadRecordsAsync(stream, new ExtendedGeoNameParser());
        }

        /// <summary>
        /// Reads <see cref="GeoName"/> records from the specified file, using the default <see cref="GeoNameParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="GeoName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<GeoName> ReadGeoNames(string filename)
        {
            return ReadGeoNames(filename, true);
        }

        /// <summary>
        /// Reads <see cref="GeoName"/> records from the <see cref="Stream"/>, using the default <see cref="GeoNameParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="GeoName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<GeoName> ReadGeoNames(Stream stream)
        {
            return ReadGeoNames(stream, true);
        }

        /// <summary>
        /// Reads <see cref="GeoName"/> records from the specified file, using the default <see cref="GeoNameParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="useextendedfileformat">
        /// When this parameter is true (default) the 19 field format (default geonames.org file format) is assumed,
        /// when this parameter is false a custom 4 field format (containing only Id, Name, Latitude and Longitude)
        /// will be used.
        /// </param>
        /// <returns>Returns an IEnumerable of <see cref="GeoName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<GeoName> ReadGeoNames(string filename, bool useextendedfileformat)
        {
            return ReadRecordsAsync(filename, new GeoNameParser(useextendedfileformat));
        }

        /// <summary>
        /// Reads <see cref="GeoName"/> records from the <see cref="Stream"/>, using the default <see cref="GeoNameParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <param name="useextendedfileformat">
        /// When this parameter is true (default) the 19 field format (default geonames.org file format) is assumed,
        /// when this parameter is false a custom 4 field format (containing only Id, Name, Latitude and Longitude)
        /// will be used.
        /// </param>
        /// <returns>Returns an IEnumerable of <see cref="GeoName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<GeoName> ReadGeoNames(Stream stream, bool useextendedfileformat)
        {
            return ReadRecordsAsync(stream, new GeoNameParser(useextendedfileformat));
        }

        /// <summary>
        /// Reads <see cref="Admin1Code"/> records from the specified file, using the default <see cref="Admin1CodeParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="Admin1Code"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<Admin1Code> ReadAdmin1Codes(string filename)
        {
            return ReadRecordsAsync(filename, new Admin1CodeParser());
        }

        /// <summary>
        /// Reads <see cref="Admin1Code"/> records from the <see cref="Stream"/>, using the default <see cref="Admin1CodeParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="Admin1Code"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<Admin1Code> ReadAdmin1Codes(Stream stream)
        {
            return ReadRecordsAsync(stream, new Admin1CodeParser());
        }

        /// <summary>
        /// Reads <see cref="Admin2Code"/> records from the specified file, using the default <see cref="Admin2CodeParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="Admin2Code"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<Admin2Code> ReadAdmin2Codes(string filename)
        {
            return ReadRecordsAsync(filename, new Admin2CodeParser());
        }

        /// <summary>
        /// Reads <see cref="Admin2Code"/> records from the <see cref="Stream"/>, using the default <see cref="Admin2CodeParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="Admin2Code"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<Admin2Code> ReadAdmin2Codes(Stream stream)
        {
            return ReadRecordsAsync(stream, new Admin2CodeParser());
        }

        /// <summary>
        /// Reads <see cref="AlternateName"/> records from the specified file, using the default <see cref="AlternateNameParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="AlternateName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<AlternateName> ReadAlternateNames(string filename)
        {
            return ReadRecordsAsync(filename, new AlternateNameParser());
        }

        /// <summary>
        /// Reads <see cref="AlternateName"/> records from the <see cref="Stream"/>, using the default <see cref="AlternateNameParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="AlternateName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<AlternateName> ReadAlternateNames(Stream stream)
        {
            return ReadRecordsAsync(stream, new AlternateNameParser());
        }

        /// <summary>
        /// Reads <see cref="AlternateNameV2"/> records from the specified file, using the default <see cref="AlternateNameParserV2"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="AlternateNameV2"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<AlternateNameV2> ReadAlternateNamesV2(string filename)
        {
            return ReadRecordsAsync(filename, new AlternateNameParserV2());
        }

        /// <summary>
        /// Reads <see cref="AlternateNameV2"/> records from the <see cref="Stream"/>, using the default <see cref="AlternateNameParserV2"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="AlternateNameV2"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<AlternateNameV2> ReadAlternateNamesV2(Stream stream)
        {
            return ReadRecordsAsync(stream, new AlternateNameParserV2());
        }

        /// <summary>
        /// Reads <see cref="Continent"/> records from the built-in data, using the default <see cref="ContinentParser"/>.
        /// </summary>
        /// <returns>Returns an IEnumerable of <see cref="Continent"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// Geonames.org doesn't provide a file for continents; you can provide your own file (see
        /// <see cref="ReadContinents(string)"/> or <see cref="ReadContinents(Stream)"/>) or use the built-in
        /// values provided by this method.
        /// </remarks>
        public static async Task<IEnumerable<Continent>> ReadBuiltInContinents()
        {
            return await ReadBuiltInResource("continentCodes", new ContinentParser()).ConfigureAwait(false);
        }

        private static async Task<IEnumerable<T>> ReadBuiltInResource<T>(string name, IParser<T> parser)
        {
            using (var s = new MemoryStream(parser.Encoding.GetBytes(Properties.Resources.ResourceManager.GetString(name, CultureInfo.InvariantCulture))))
                return await ReadRecordsAsync(s, parser).ToArrayAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Reads <see cref="Continent"/> records from the specified file, using the default <see cref="ContinentParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="Continent"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        /// <seealso cref="ReadBuiltInContinents"/>
        public static IAsyncEnumerable<Continent> ReadContinents(string filename)
        {
            return ReadRecordsAsync(filename, new ContinentParser());
        }

        /// <summary>
        /// Reads <see cref="Continent"/> records from the <see cref="Stream"/>, using the default <see cref="ContinentParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="Continent"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        /// <seealso cref="ReadBuiltInContinents"/>
        public static IAsyncEnumerable<Continent> ReadContinents(Stream stream)
        {
            return ReadRecordsAsync(stream, new ContinentParser());
        }

        /// <summary>
        /// Reads <see cref="CountryInfo"/> records from the specified file, using the default <see cref="CountryInfoParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="CountryInfo"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<CountryInfo> ReadCountryInfo(string filename)
        {
            return ReadRecordsAsync(filename, new CountryInfoParser());
        }

        /// <summary>
        /// Reads <see cref="CountryInfo"/> records from the <see cref="Stream"/>, using the default <see cref="CountryInfoParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="CountryInfo"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<CountryInfo> ReadCountryInfo(Stream stream)
        {
            return ReadRecordsAsync(stream, new CountryInfoParser());
        }

        /// <summary>
        /// Reads <see cref="FeatureClass"/> records from the built-in data, using the default <see cref="FeatureClassParser"/>.
        /// </summary>
        /// <returns>Returns an IEnumerable of <see cref="FeatureClass"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// Geonames.org doesn't provide a file for featueclasses; you can provide your own file (see
        /// <see cref="ReadFeatureClasses(string)"/> or <see cref="ReadFeatureClasses(Stream)"/>) or use the built-in
        /// values provided by this method.
        /// </remarks>
        public static async Task<IEnumerable<FeatureClass>> ReadBuiltInFeatureClasses()
        {
            return await ReadBuiltInResource("featureClasses_en", new FeatureClassParser()).ConfigureAwait(false);
        }


        /// <summary>
        /// Reads <see cref="FeatureClass"/> records from the specified file, using the default <see cref="FeatureClassParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="FeatureClass"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        /// <seealso cref="ReadBuiltInFeatureClasses"/>
        public static IAsyncEnumerable<FeatureClass> ReadFeatureClasses(string filename)
        {
            return ReadRecordsAsync(filename, new FeatureClassParser());
        }

        /// <summary>
        /// Reads <see cref="FeatureClass"/> records from the <see cref="Stream"/>, using the default <see cref="FeatureClassParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="FeatureClass"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        /// <seealso cref="ReadBuiltInFeatureClasses"/>
        public static IAsyncEnumerable<FeatureClass> ReadFeatureClasses(Stream stream)
        {
            return ReadRecordsAsync(stream, new FeatureClassParser());
        }

        /// <summary>
        /// Reads <see cref="FeatureCode"/> records from the specified file, using the default <see cref="FeatureCodeParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="FeatureCode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<FeatureCode> ReadFeatureCodes(string filename)
        {
            return ReadRecordsAsync(filename, new FeatureCodeParser());
        }

        /// <summary>
        /// Reads <see cref="FeatureCode"/> records from the <see cref="Stream"/>, using the default <see cref="FeatureCodeParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="FeatureCode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<FeatureCode> ReadFeatureCodes(Stream stream)
        {
            return ReadRecordsAsync(stream, new FeatureCodeParser());
        }

        /// <summary>
        /// Reads <see cref="HierarchyNode"/> records from the specified file, using the default <see cref="HierarchyParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="HierarchyNode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<HierarchyNode> ReadHierarchy(string filename)
        {
            return ReadRecordsAsync(filename, new HierarchyParser());
        }

        /// <summary>
        /// Reads <see cref="HierarchyNode"/> records from the <see cref="Stream"/>, using the default <see cref="HierarchyParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="HierarchyNode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<HierarchyNode> ReadHierarchy(Stream stream)
        {
            return ReadRecordsAsync(stream, new HierarchyParser());
        }

        /// <summary>
        /// Reads <see cref="ISOLanguageCode"/> records from the specified file, using the default <see cref="ISOLanguageCodeParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="ISOLanguageCode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<ISOLanguageCode> ReadISOLanguageCodes(string filename)
        {
            return ReadRecordsAsync(filename, new ISOLanguageCodeParser());
        }

        /// <summary>
        /// Reads <see cref="ISOLanguageCode"/> records from the <see cref="Stream"/>, using the default <see cref="ISOLanguageCodeParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="ISOLanguageCode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<ISOLanguageCode> ReadISOLanguageCodes(Stream stream)
        {
            return ReadRecordsAsync(stream, new ISOLanguageCodeParser());
        }

        /// <summary>
        /// Reads <see cref="TimeZone"/> records from the specified file, using the default <see cref="TimeZoneParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="TimeZone"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<TimeZone> ReadTimeZones(string filename)
        {
            return ReadRecordsAsync(filename, new TimeZoneParser());
        }

        /// <summary>
        /// Reads <see cref="TimeZone"/> records from the <see cref="Stream"/>, using the default <see cref="TimeZoneParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="TimeZone"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<TimeZone> ReadTimeZones(Stream stream)
        {
            return ReadRecordsAsync(stream, new TimeZoneParser());
        }

        /// <summary>
        /// Reads <see cref="UserTag"/> records from the specified file, using the default <see cref="UserTagParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="UserTag"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<UserTag> ReadUserTags(string filename)
        {
            return ReadRecordsAsync(filename, new UserTagParser());
        }

        /// <summary>
        /// Reads <see cref="UserTag"/> records from the <see cref="Stream"/>, using the default <see cref="UserTagParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="UserTag"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<UserTag> ReadUserTags(Stream stream)
        {
            return ReadRecordsAsync(stream, new UserTagParser());
        }

        /// <summary>
        /// Reads <see cref="Postalcode"/> records from the specified file, using the default <see cref="PostalcodeParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="Postalcode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IAsyncEnumerable<Postalcode> ReadPostalcodes(string filename)
        {
            return ReadRecordsAsync(filename, new PostalcodeParser());
        }

        /// <summary>
        /// Reads <see cref="Postalcode"/> records from the <see cref="Stream"/>, using the default <see cref="PostalcodeParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="Postalcode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords{T} overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IAsyncEnumerable<Postalcode> ReadPostalcodes(Stream stream)
        {
            return ReadRecordsAsync(stream, new PostalcodeParser());
        }
        #endregion
    }
}