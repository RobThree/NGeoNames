using NGeoNames.Composers;
using NGeoNames.Entities;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace NGeoNames
{
    /// <summary>
    /// Provides methods to write/compose geonames.org compatible files.
    /// </summary>
    public class GeoFileWriter
    {
        /// <summary>
        /// The default separator used by the <see cref="GeoFileWriter"/> when writing records to a file/stream.
        /// </summary>
        public const string DEFAULTLINESEPARATOR = "\n";

        /// <summary>
        /// Writes records of type T, using the specified composer to compose the file.
        /// </summary>
        /// <typeparam name="T">The type of objects to write/compose.</typeparam>
        /// <param name="path">The path of the file to read/write.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <param name="composer">The <see cref="IComposer{T}"/> to use when writing the file.</param>
        /// <param name="lineseparator">The lineseparator to use (see <see cref="DEFAULTLINESEPARATOR"/>).</param>
        /// <remarks>
        /// This method will try to "autodetect" the filetype; it will 'recognize' .txt and .gz (or .*.gz) files
        /// and act accordingly. If you use another extension you may want to explicitly specify the filetype
        /// using the <see cref="WriteRecordsAsync{T}(string, IAsyncEnumerable{T}, IComposer{T}, FileType, string)"/> overload.
        /// </remarks>
        public static Task WriteRecordsAsync<T>(string path, IAsyncEnumerable<T> values, IComposer<T> composer, string lineseparator = DEFAULTLINESEPARATOR)
        {
            return WriteRecordsAsync(path, values, composer, FileUtil.GetFileTypeFromExtension(path), lineseparator);
        }

        /// <summary>
        /// Writes records of type T, using the specified composer to compose the file.
        /// </summary>
        /// <typeparam name="T">The type of objects to write/compose.</typeparam>
        /// <param name="path">The path of the file to read/write.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <param name="filetype">The <see cref="FileType"/> of the file.</param>
        /// <param name="composer">The <see cref="IComposer{T}"/> to use when writing the file.</param>
        /// <param name="lineseparator">The lineseparator to use (see <see cref="DEFAULTLINESEPARATOR"/>).</param>
        public static async Task WriteRecordsAsync<T>(string path, IAsyncEnumerable<T> values, IComposer<T> composer, FileType filetype, string lineseparator = DEFAULTLINESEPARATOR)
        {
            using (var s = GetStream(path, filetype))
                await WriteRecordsAsync(s, values, composer, lineseparator).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes records of type T, using the specified composer to compose the file.
        /// </summary>
        /// <typeparam name="T">The type of objects to write/compose.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <param name="composer">The <see cref="IComposer{T}"/> to use when writing the file.</param>
        /// <param name="lineseparator">The lineseparator to use (see <see cref="DEFAULTLINESEPARATOR"/>).</param>
        public static async Task WriteRecordsAsync<T>(Stream stream, IAsyncEnumerable<T> values, IComposer<T> composer, string lineseparator = DEFAULTLINESEPARATOR)
        {
            using (var w = new StreamWriter(stream, composer.Encoding))
            {
                await foreach (var v in values)
                {
                    w.Write(composer.Compose(v) + lineseparator);
                }
            }
        }

        private static Stream GetStream(string path, FileType filetype)
        {
            var filestream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read);

            //Figure out how we're supposed to read the file
            var writeastype = filetype == FileType.AutoDetect ? FileUtil.GetFileTypeFromExtension(path) : filetype;
            switch (writeastype)
            {
                case FileType.Plain:
                    return filestream;
                case FileType.GZip:
                    return new GZipStream(filestream, CompressionLevel.Optimal);
            }
            throw new System.NotSupportedException($"Filetype not supported: {writeastype}");
        }

        #region Convenience methods


        /// <summary>
        /// Writes <see cref="Admin1Code"/> records to the specified file, using the default <see cref="Admin1CodeComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteAdmin1Codes(string filename, IAsyncEnumerable<Admin1Code> values)
        {
            return WriteRecordsAsync(filename, values, new Admin1CodeComposer());
        }

        /// <summary>
        /// Writes <see cref="Admin1Code"/> records to the <see cref="Stream"/>, using the default <see cref="Admin1CodeComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteAdmin1Codes(Stream stream, IAsyncEnumerable<Admin1Code> values)
        {
            return WriteRecordsAsync(stream, values, new Admin1CodeComposer());
        }

        /// <summary>
        /// Writes <see cref="Admin2Code"/> records to the specified file, using the default <see cref="Admin2CodeComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteAdmin2Codes(string filename, IAsyncEnumerable<Admin2Code> values)
        {
            return WriteRecordsAsync(filename, values, new Admin2CodeComposer());
        }

        /// <summary>
        /// Writes <see cref="Admin2Code"/> records to the <see cref="Stream"/>, using the default <see cref="Admin2CodeComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteAdmin2Codes(Stream stream, IAsyncEnumerable<Admin2Code> values)
        {
            return WriteRecordsAsync(stream, values, new Admin2CodeComposer());
        }

        /// <summary>
        /// Writes <see cref="AlternateName"/> records to the specified file, using the default <see cref="AlternateNameComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteAlternateNames(string filename, IAsyncEnumerable<AlternateName> values)
        {
            return WriteRecordsAsync(filename, values, new AlternateNameComposer());
        }

        /// <summary>
        /// Writes <see cref="AlternateName"/> records to the <see cref="Stream"/>, using the default <see cref="AlternateNameComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteAlternateNames(Stream stream, IAsyncEnumerable<AlternateName> values)
        {
            return WriteRecordsAsync(stream, values, new AlternateNameComposer());
        }

        /// <summary>
        /// Writes <see cref="AlternateNameV2"/> records to the specified file, using the default <see cref="AlternateNameV2Composer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteAlternateNamesV2(string filename, IAsyncEnumerable<AlternateNameV2> values)
        {
            return WriteRecordsAsync(filename, values, new AlternateNameV2Composer());
        }

        /// <summary>
        /// Writes <see cref="AlternateNameV2"/> records to the <see cref="Stream"/>, using the default <see cref="AlternateNameV2Composer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteAlternateNamesV2(Stream stream, IAsyncEnumerable<AlternateNameV2> values)
        {
            return WriteRecordsAsync(stream, values, new AlternateNameV2Composer());
        }

        /// <summary>
        /// Writes <see cref="Continent"/> records to the specified file, using the default <see cref="ContinentComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteContinents(string filename, IAsyncEnumerable<Continent> values)
        {
            return WriteRecordsAsync(filename, values, new ContinentComposer());
        }

        /// <summary>
        /// Writes <see cref="Continent"/> records to the <see cref="Stream"/>, using the default <see cref="ContinentComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteContinents(Stream stream, IAsyncEnumerable<Continent> values)
        {
            return WriteRecordsAsync(stream, values, new ContinentComposer());
        }

        /// <summary>
        /// Writes <see cref="CountryInfo"/> records to the specified file, using the default <see cref="CountryInfoComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteCountryInfo(string filename, IAsyncEnumerable<CountryInfo> values)
        {
            return WriteRecordsAsync(filename, values, new CountryInfoComposer());
        }

        /// <summary>
        /// Writes <see cref="CountryInfo"/> records to the <see cref="Stream"/>, using the default <see cref="CountryInfoComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteCountryInfo(Stream stream, IAsyncEnumerable<CountryInfo> values)
        {
            return WriteRecordsAsync(stream, values, new CountryInfoComposer());
        }

        /// <summary>
        /// Writes <see cref="ExtendedGeoName"/> records to the specified file, using the default <see cref="ExtendedGeoNameComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteExtendedGeoNames(string filename, IAsyncEnumerable<ExtendedGeoName> values)
        {
            return WriteRecordsAsync(filename, values, new ExtendedGeoNameComposer());
        }

        /// <summary>
        /// Writes <see cref="ExtendedGeoName"/> records to the <see cref="Stream"/>, using the default <see cref="ExtendedGeoNameComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteExtendedGeoNames(Stream stream, IAsyncEnumerable<ExtendedGeoName> values)
        {
            return WriteRecordsAsync(stream, values, new ExtendedGeoNameComposer());
        }

        /// <summary>
        /// Writes <see cref="FeatureClass"/> records to the specified file, using the default <see cref="FeatureClassComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteFeatureClasses(string filename, IAsyncEnumerable<FeatureClass> values)
        {
            return WriteRecordsAsync(filename, values, new FeatureClassComposer());
        }

        /// <summary>
        /// Writes <see cref="FeatureClass"/> records to the <see cref="Stream"/>, using the default <see cref="FeatureClassComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteFeatureClasses(Stream stream, IAsyncEnumerable<FeatureClass> values)
        {
            return WriteRecordsAsync(stream, values, new FeatureClassComposer());
        }

        /// <summary>
        /// Writes <see cref="FeatureCode"/> records to the specified file, using the default <see cref="FeatureCodeComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteFeatureCodes(string filename, IAsyncEnumerable<FeatureCode> values)
        {
            return WriteRecordsAsync(filename, values, new FeatureCodeComposer());
        }

        /// <summary>
        /// Writes <see cref="FeatureCode"/> records to the <see cref="Stream"/>, using the default <see cref="FeatureCodeComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteFeatureCodes(Stream stream, IAsyncEnumerable<FeatureCode> values)
        {
            return WriteRecordsAsync(stream, values, new FeatureCodeComposer());
        }

        /// <summary>
        /// Writes <see cref="GeoName"/> records to the specified file, using the default <see cref="GeoNameComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written. This method will use the default 19 field (geonames.org compatible)
        /// fileformat. Use <see cref="WriteGeoNames(string, IAsyncEnumerable{GeoName}, bool)"/> if you want to use a
        /// more compact 4 field format.
        /// </remarks>
        /// <seealso cref="WriteGeoNames(string, IAsyncEnumerable{GeoName}, bool)"/>
        public static Task WriteGeoNames(string filename, IAsyncEnumerable<GeoName> values)
        {
            return WriteGeoNames(filename, values, false);
        }

        /// <summary>
        /// Writes <see cref="GeoName"/> records to the <see cref="Stream"/>, using the default <see cref="GeoNameComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written. This method will use the default 19 field (geonames.org compatible)
        /// fileformat. Use <see cref="WriteGeoNames(string, IAsyncEnumerable{GeoName}, bool)"/> if you want to use a
        /// more compact 4 field format.
        /// </remarks>
        /// <seealso cref="WriteGeoNames(Stream, IAsyncEnumerable{GeoName}, bool)"/>
        public static Task WriteGeoNames(Stream stream, IAsyncEnumerable<GeoName> values)
        {
            return WriteGeoNames(stream, values, false);
        }

        /// <summary>
        /// Writes <see cref="GeoName"/> records to the specified file, using the default <see cref="GeoNameComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <param name="useextendedfileformat">
        /// When true, the (default) 19 field format (geonames.org compatible) will be used, when false a more compact
        /// 4 field format (Id, Name, Latitude, Longitude) will be used.
        /// </param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        /// <seealso cref="WriteGeoNames(string, IAsyncEnumerable{GeoName})"/>
        public static Task WriteGeoNames(string filename, IAsyncEnumerable<GeoName> values, bool useextendedfileformat)
        {
            return WriteRecordsAsync(filename, values, new GeoNameComposer(useextendedfileformat));
        }

        /// <summary>
        /// Writes <see cref="GeoName"/> records to the <see cref="Stream"/>, using the default <see cref="GeoNameComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <param name="useextendedfileformat">
        /// When true, the (default) 19 field format (geonames.org compatible) will be used, when false a more compact
        /// 4 field format (Id, Name, Latitude, Longitude) will be used.
        /// </param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        /// <seealso cref="WriteGeoNames(Stream, IAsyncEnumerable{GeoName})"/>
        public static Task WriteGeoNames(Stream stream, IAsyncEnumerable<GeoName> values, bool useextendedfileformat)
        {
            return WriteRecordsAsync(stream, values, new GeoNameComposer(useextendedfileformat));
        }

        /// <summary>
        /// Writes <see cref="HierarchyNode"/> records to the specified file, using the default <see cref="HierarchyComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteHierarchy(string filename, IAsyncEnumerable<HierarchyNode> values)
        {
            return WriteRecordsAsync(filename, values, new HierarchyComposer());
        }

        /// <summary>
        /// Writes <see cref="HierarchyNode"/> records to the <see cref="Stream"/>, using the default <see cref="HierarchyComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteHierarchy(Stream stream, IAsyncEnumerable<HierarchyNode> values)
        {
            return WriteRecordsAsync(stream, values, new HierarchyComposer());
        }

        /// <summary>
        /// Writes <see cref="ISOLanguageCode"/> records to the specified file, using the default <see cref="ISOLanguageCodeComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteISOLanguageCodes(string filename, IAsyncEnumerable<ISOLanguageCode> values)
        {
            return WriteRecordsAsync(filename, values, new ISOLanguageCodeComposer());
        }

        /// <summary>
        /// Writes <see cref="ISOLanguageCode"/> records to the <see cref="Stream"/>, using the default <see cref="ISOLanguageCodeComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteISOLanguageCodes(Stream stream, IAsyncEnumerable<ISOLanguageCode> values)
        {
            return WriteRecordsAsync(stream, values, new ISOLanguageCodeComposer());
        }

        /// <summary>
        /// Writes <see cref="TimeZone"/> records to the specified file, using the default <see cref="TimeZoneComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteTimeZones(string filename, IAsyncEnumerable<TimeZone> values)
        {
            return WriteRecordsAsync(filename, values, new TimeZoneComposer());
        }

        /// <summary>
        /// Writes <see cref="TimeZone"/> records to the <see cref="Stream"/>, using the default <see cref="TimeZoneComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteTimeZones(Stream stream, IAsyncEnumerable<TimeZone> values)
        {
            return WriteRecordsAsync(stream, values, new TimeZoneComposer());
        }

        /// <summary>
        /// Writes <see cref="UserTag"/> records to the specified file, using the default <see cref="UserTagComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WriteUserTags(string filename, IAsyncEnumerable<UserTag> values)
        {
            return WriteRecordsAsync(filename, values, new UserTagComposer());
        }

        /// <summary>
        /// Writes <see cref="UserTag"/> records to the <see cref="Stream"/>, using the default <see cref="UserTagComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WriteUserTags(Stream stream, IAsyncEnumerable<UserTag> values)
        {
            return WriteRecordsAsync(stream, values, new UserTagComposer());
        }

        /// <summary>
        /// Writes <see cref="Postalcode"/> records to the specified file, using the default <see cref="PostalcodeComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written.
        /// </remarks>
        public static Task WritePostalcodes(string filename, IAsyncEnumerable<Postalcode> values)
        {
            return WriteRecordsAsync(filename, values, new PostalcodeComposer());
        }

        /// <summary>
        /// Writes <see cref="Postalcode"/> records to the <see cref="Stream"/>, using the default <see cref="PostalcodeComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the stream is written.
        /// </remarks>
        public static Task WritePostalcodes(Stream stream, IAsyncEnumerable<Postalcode> values)
        {
            return WriteRecordsAsync(stream, values, new PostalcodeComposer());
        }
        #endregion
    }
}
