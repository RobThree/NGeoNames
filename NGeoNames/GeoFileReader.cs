using NGeoNames.Entities;
using NGeoNames.Parsers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace NGeoNames
{
    /// <summary>
    /// Provides methods to read/parse files from geonames.org.
    /// </summary>
    public class GeoFileReader
    {
        /// <summary>
        /// Reads records of type T, using the specified parser to parse the values.
        /// </summary>
        /// <typeparam name="T">The type of objects to read/parse.</typeparam>
        /// <param name="path">The path of the file to read/parse.</param>
        /// <param name="parser">The <see cref="IParser&lt;T&gt;"/> to use when reading the file.</param>
        /// <returns>Returns an IEnumerable of T representing the records read/parsed.</returns>
        /// <remarks>
        /// This method will try to "autodetect" the filetype; it will 'recognize' .txt and .gz (or .*.gz) files
        /// and act accordingly. If you use another extension you may want to explicitly specify the filetype
        /// using the <see cref="ReadRecords&lt;T&gt;(string, FileType, IParser&lt;T&gt;)"/> overload.
        /// </remarks>
        public IEnumerable<T> ReadRecords<T>(string path, IParser<T> parser)
        {
            return ReadRecords(path, FileType.AutoDetect, parser);
        }

        /// <summary>
        /// Reads records of type T, using the specified parser to parse the values.
        /// </summary>
        /// <typeparam name="T">The type of objects to read/parse.</typeparam>
        /// <param name="path">The path of the file to read/parse.</param>
        /// <param name="filetype">The <see cref="FileType"/> of the file.</param>
        /// <param name="parser">The <see cref="IParser&lt;T&gt;"/> to use when reading the file.</param>
        /// <returns>Returns an IEnumerable of T representing the records read/parsed.</returns>
        public IEnumerable<T> ReadRecords<T>(string path, FileType filetype, IParser<T> parser)
        {
            using (var f = GetStream(path, filetype))
            {
                foreach (var r in ReadRecords(f, parser))
                    yield return r;
            }
        }

        /// <summary>
        /// Reads records of type T, using the specified parser to parse the values.
        /// </summary>
        /// <typeparam name="T">The type of objects to read/parse.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <param name="parser">The <see cref="IParser&lt;T&gt;"/> to use when reading the file.</param>
        /// <returns>Returns an IEnumerable of T representing the records read/parsed.</returns>
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
                    if ((c > parser.SkipLines) && (line.Length > 0)  && (!parser.HasComments || (parser.HasComments && !line.StartsWith("#"))))
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
            FileType readastype = filetype == FileType.AutoDetect ? FileUtil.GetFileTypeFromExtension(path) : filetype;
            switch (readastype)
            {
                case FileType.Plain:
                    return filestream;
                case FileType.GZip:
                    return new GZipStream(filestream, CompressionMode.Decompress);
            }
            throw new System.NotSupportedException(string.Format("Filetype not supported: {0}", readastype));
        }


        #region Convenience methods
        /// <summary>
        /// Reads <see cref="ExtendedGeoName"/> records from the specified file, using the default <see cref="ExtendedGeoNameParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="ExtendedGeoName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<ExtendedGeoName> ReadExtendedGeoNames(string filename)
        {
            return new GeoFileReader().ReadRecords<ExtendedGeoName>(filename, new ExtendedGeoNameParser());
        }

        /// <summary>
        /// Reads <see cref="ExtendedGeoName"/> records from the <see cref="Stream"/>, using the default <see cref="ExtendedGeoNameParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="ExtendedGeoName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<ExtendedGeoName> ReadExtendedGeoNames(Stream stream)
        {
            return new GeoFileReader().ReadRecords<ExtendedGeoName>(stream, new ExtendedGeoNameParser());
        }

        /// <summary>
        /// Reads <see cref="GeoName"/> records from the specified file, using the default <see cref="GeoNameParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="GeoName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<GeoName> ReadGeoNames(string filename)
        {
            return ReadGeoNames(filename, true);
        }

        /// <summary>
        /// Reads <see cref="GeoName"/> records from the <see cref="Stream"/>, using the default <see cref="GeoNameParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="GeoName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<GeoName> ReadGeoNames(Stream stream)
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
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<GeoName> ReadGeoNames(string filename, bool useextendedfileformat)
        {
            return new GeoFileReader().ReadRecords<GeoName>(filename, new GeoNameParser(useextendedfileformat));
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
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<GeoName> ReadGeoNames(Stream stream, bool useextendedfileformat)
        {
            return new GeoFileReader().ReadRecords<GeoName>(stream, new GeoNameParser(useextendedfileformat));
        }

        /// <summary>
        /// Reads <see cref="Admin1Code"/> records from the specified file, using the default <see cref="Admin1CodeParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="Admin1Code"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<Admin1Code> ReadAdmin1Codes(string filename)
        {
            return new GeoFileReader().ReadRecords<Admin1Code>(filename, new Admin1CodeParser());
        }

        /// <summary>
        /// Reads <see cref="Admin1Code"/> records from the <see cref="Stream"/>, using the default <see cref="Admin1CodeParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="Admin1Code"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<Admin1Code> ReadAdmin1Codes(Stream stream)
        {
            return new GeoFileReader().ReadRecords<Admin1Code>(stream, new Admin1CodeParser());
        }

        /// <summary>
        /// Reads <see cref="Admin2Code"/> records from the specified file, using the default <see cref="Admin2CodeParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="Admin2Code"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<Admin2Code> ReadAdmin2Codes(string filename)
        {
            return new GeoFileReader().ReadRecords<Admin2Code>(filename, new Admin2CodeParser());
        }

        /// <summary>
        /// Reads <see cref="Admin2Code"/> records from the <see cref="Stream"/>, using the default <see cref="Admin2CodeParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="Admin2Code"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<Admin2Code> ReadAdmin2Codes(Stream stream)
        {
            return new GeoFileReader().ReadRecords<Admin2Code>(stream, new Admin2CodeParser());
        }

        /// <summary>
        /// Reads <see cref="AlternateName"/> records from the specified file, using the default <see cref="AlternateNameParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="AlternateName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<AlternateName> ReadAlternateNames(string filename)
        {
            return new GeoFileReader().ReadRecords<AlternateName>(filename, new AlternateNameParser());
        }

        /// <summary>
        /// Reads <see cref="AlternateName"/> records from the <see cref="Stream"/>, using the default <see cref="AlternateNameParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="AlternateName"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<AlternateName> ReadAlternateNames(Stream stream)
        {
            return new GeoFileReader().ReadRecords<AlternateName>(stream, new AlternateNameParser());
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

        /// <summary>
        /// Reads <see cref="Continent"/> records from the specified file, using the default <see cref="ContinentParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="Continent"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        /// <seealso cref="ReadBuiltInContinents"/>
        public static IEnumerable<Continent> ReadContinents(string filename)
        {
            return new GeoFileReader().ReadRecords<Continent>(filename, new ContinentParser());
        }

        /// <summary>
        /// Reads <see cref="Continent"/> records from the <see cref="Stream"/>, using the default <see cref="ContinentParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="Continent"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        /// <seealso cref="ReadBuiltInContinents"/>
        public static IEnumerable<Continent> ReadContinents(Stream stream)
        {
            return new GeoFileReader().ReadRecords<Continent>(stream, new ContinentParser());
        }

        /// <summary>
        /// Reads <see cref="CountryInfo"/> records from the specified file, using the default <see cref="CountryInfoParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="CountryInfo"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<CountryInfo> ReadCountryInfo(string filename)
        {
            return new GeoFileReader().ReadRecords<CountryInfo>(filename, new CountryInfoParser());
        }

        /// <summary>
        /// Reads <see cref="CountryInfo"/> records from the <see cref="Stream"/>, using the default <see cref="CountryInfoParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="CountryInfo"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<CountryInfo> ReadCountryInfo(Stream stream)
        {
            return new GeoFileReader().ReadRecords<CountryInfo>(stream, new CountryInfoParser());
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
        public static IEnumerable<FeatureClass> ReadBuiltInFeatureClasses()
        {
            return ReadBuiltInResource<FeatureClass>("featureClasses_en", new FeatureClassParser());
        }


        /// <summary>
        /// Reads <see cref="FeatureClass"/> records from the specified file, using the default <see cref="FeatureClassParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="FeatureClass"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        /// <seealso cref="ReadBuiltInFeatureClasses"/>
        public static IEnumerable<FeatureClass> ReadFeatureClasses(string filename)
        {
            return new GeoFileReader().ReadRecords<FeatureClass>(filename, new FeatureClassParser());
        }

        /// <summary>
        /// Reads <see cref="FeatureClass"/> records from the <see cref="Stream"/>, using the default <see cref="FeatureClassParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="FeatureClass"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        /// <seealso cref="ReadBuiltInFeatureClasses"/>
        public static IEnumerable<FeatureClass> ReadFeatureClasses(Stream stream)
        {
            return new GeoFileReader().ReadRecords<FeatureClass>(stream, new FeatureClassParser());
        }

        /// <summary>
        /// Reads <see cref="FeatureCode"/> records from the specified file, using the default <see cref="FeatureCodeParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="FeatureCode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<FeatureCode> ReadFeatureCodes(string filename)
        {
            return new GeoFileReader().ReadRecords<FeatureCode>(filename, new FeatureCodeParser());
        }

        /// <summary>
        /// Reads <see cref="FeatureCode"/> records from the <see cref="Stream"/>, using the default <see cref="FeatureCodeParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="FeatureCode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<FeatureCode> ReadFeatureCodes(Stream stream)
        {
            return new GeoFileReader().ReadRecords<FeatureCode>(stream, new FeatureCodeParser());
        }

        /// <summary>
        /// Reads <see cref="HierarchyNode"/> records from the specified file, using the default <see cref="HierarchyParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="HierarchyNode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<HierarchyNode> ReadHierarchy(string filename)
        {
            return new GeoFileReader().ReadRecords<HierarchyNode>(filename, new HierarchyParser());
        }

        /// <summary>
        /// Reads <see cref="HierarchyNode"/> records from the <see cref="Stream"/>, using the default <see cref="HierarchyParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="HierarchyNode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<HierarchyNode> ReadHierarchy(Stream stream)
        {
            return new GeoFileReader().ReadRecords<HierarchyNode>(stream, new HierarchyParser());
        }

        /// <summary>
        /// Reads <see cref="ISOLanguageCode"/> records from the specified file, using the default <see cref="ISOLanguageCodeParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="ISOLanguageCode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<ISOLanguageCode> ReadISOLanguageCodes(string filename)
        {
            return new GeoFileReader().ReadRecords<ISOLanguageCode>(filename, new ISOLanguageCodeParser());
        }

        /// <summary>
        /// Reads <see cref="ISOLanguageCode"/> records from the <see cref="Stream"/>, using the default <see cref="ISOLanguageCodeParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="ISOLanguageCode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<ISOLanguageCode> ReadISOLanguageCodes(Stream stream)
        {
            return new GeoFileReader().ReadRecords<ISOLanguageCode>(stream, new ISOLanguageCodeParser());
        }

        /// <summary>
        /// Reads <see cref="TimeZone"/> records from the specified file, using the default <see cref="TimeZoneParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="TimeZone"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<TimeZone> ReadTimeZones(string filename)
        {
            return new GeoFileReader().ReadRecords<TimeZone>(filename, new TimeZoneParser());
        }

        /// <summary>
        /// Reads <see cref="TimeZone"/> records from the <see cref="Stream"/>, using the default <see cref="TimeZoneParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="TimeZone"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<TimeZone> ReadTimeZones(Stream stream)
        {
            return new GeoFileReader().ReadRecords<TimeZone>(stream, new TimeZoneParser());
        }

        /// <summary>
        /// Reads <see cref="UserTag"/> records from the specified file, using the default <see cref="UserTagParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="UserTag"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<UserTag> ReadUserTags(string filename)
        {
            return new GeoFileReader().ReadRecords<UserTag>(filename, new UserTagParser());
        }

        /// <summary>
        /// Reads <see cref="UserTag"/> records from the <see cref="Stream"/>, using the default <see cref="UserTagParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="UserTag"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<UserTag> ReadUserTags(Stream stream)
        {
            return new GeoFileReader().ReadRecords<UserTag>(stream, new UserTagParser());
        }

        /// <summary>
        /// Reads <see cref="Postalcode"/> records from the specified file, using the default <see cref="PostalcodeParser"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <returns>Returns an IEnumerable of <see cref="Postalcode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the file is read.
        /// </remarks>
        public static IEnumerable<Postalcode> ReadPostalcodes(string filename)
        {
            return new GeoFileReader().ReadRecords<Postalcode>(filename, new PostalcodeParser());
        }

        /// <summary>
        /// Reads <see cref="Postalcode"/> records from the <see cref="Stream"/>, using the default <see cref="PostalcodeParser"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read/parse.</param>
        /// <returns>Returns an IEnumerable of <see cref="Postalcode"/> representing the records read/parsed.</returns>
        /// <remarks>
        /// This static method is a convenience-method; see the ReadRecords&lt;T&gt; overloaded instance-methods for
        /// more control over how the stream is read.
        /// </remarks>
        public static IEnumerable<Postalcode> ReadPostalcodes(Stream stream)
        {
            return new GeoFileReader().ReadRecords<Postalcode>(stream, new PostalcodeParser());
        }
        #endregion
    }
}