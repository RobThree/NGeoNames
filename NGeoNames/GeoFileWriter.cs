using NGeoNames.Composers;
using NGeoNames.Entities;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

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
        /// using the <see cref="WriteRecords{T}(string, IEnumerable{T}, IComposer{T}, FileType, string)"/> overload.
        /// </remarks>
        public void WriteRecords<T>(string path, IEnumerable<T> values, IComposer<T> composer, string lineseparator = DEFAULTLINESEPARATOR)
        {
            WriteRecords(path, values, composer, FileUtil.GetFileTypeFromExtension(path), lineseparator);
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
        public void WriteRecords<T>(string path, IEnumerable<T> values, IComposer<T> composer, FileType filetype, string lineseparator = DEFAULTLINESEPARATOR)
        {
            using (var s = GetStream(path, filetype))
                WriteRecords(s, values, composer, lineseparator);
        }

        /// <summary>
        /// Writes records of type T, using the specified composer to compose the file.
        /// </summary>
        /// <typeparam name="T">The type of objects to write/compose.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <param name="composer">The <see cref="IComposer{T}"/> to use when writing the file.</param>
        /// <param name="lineseparator">The lineseparator to use (see <see cref="DEFAULTLINESEPARATOR"/>).</param>
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
            throw new System.NotSupportedException(string.Format("Filetype not supported: {0}", writeastype));
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
        public static void WriteAdmin1Codes(string filename, IEnumerable<Admin1Code> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new Admin1CodeComposer());
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
        public static void WriteAdmin1Codes(Stream stream, IEnumerable<Admin1Code> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new Admin1CodeComposer());
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
        public static void WriteAdmin2Codes(string filename, IEnumerable<Admin2Code> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new Admin2CodeComposer());
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
        public static void WriteAdmin2Codes(Stream stream, IEnumerable<Admin2Code> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new Admin2CodeComposer());
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
        public static void WriteAlternateNames(string filename, IEnumerable<AlternateName> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new AlternateNameComposer());
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
        public static void WriteAlternateNames(Stream stream, IEnumerable<AlternateName> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new AlternateNameComposer());
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
        public static void WriteAlternateNamesV2(string filename, IEnumerable<AlternateNameV2> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new AlternateNameV2Composer());
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
        public static void WriteAlternateNamesV2(Stream stream, IEnumerable<AlternateNameV2> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new AlternateNameV2Composer());
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
        public static void WriteContinents(string filename, IEnumerable<Continent> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new ContinentComposer());
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
        public static void WriteContinents(Stream stream, IEnumerable<Continent> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new ContinentComposer());
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
        public static void WriteCountryInfo(string filename, IEnumerable<CountryInfo> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new CountryInfoComposer());
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
        public static void WriteCountryInfo(Stream stream, IEnumerable<CountryInfo> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new CountryInfoComposer());
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
        public static void WriteExtendedGeoNames(string filename, IEnumerable<ExtendedGeoName> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new ExtendedGeoNameComposer());
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
        public static void WriteExtendedGeoNames(Stream stream, IEnumerable<ExtendedGeoName> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new ExtendedGeoNameComposer());
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
        public static void WriteFeatureClasses(string filename, IEnumerable<FeatureClass> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new FeatureClassComposer());
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
        public static void WriteFeatureClasses(Stream stream, IEnumerable<FeatureClass> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new FeatureClassComposer());
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
        public static void WriteFeatureCodes(string filename, IEnumerable<FeatureCode> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new FeatureCodeComposer());
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
        public static void WriteFeatureCodes(Stream stream, IEnumerable<FeatureCode> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new FeatureCodeComposer());
        }

        /// <summary>
        /// Writes <see cref="GeoName"/> records to the specified file, using the default <see cref="GeoNameComposer"/>.
        /// </summary>
        /// <param name="filename">The name/path of the file.</param>
        /// <param name="values">The values to write to the file.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written. This method will use the default 19 field (geonames.org compatible)
        /// fileformat. Use <see cref="WriteGeoNames(string, IEnumerable{GeoName}, bool)"/> if you want to use a
        /// more compact 4 field format.
        /// </remarks>
        /// <seealso cref="WriteGeoNames(string, IEnumerable{GeoName}, bool)"/>
        public static void WriteGeoNames(string filename, IEnumerable<GeoName> values)
        {
            WriteGeoNames(filename, values, false);
        }

        /// <summary>
        /// Writes <see cref="GeoName"/> records to the <see cref="Stream"/>, using the default <see cref="GeoNameComposer"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        /// <param name="values">The values to write to the stream.</param>
        /// <remarks>
        /// This static method is a convenience-method; see the WriteRecords{T} overloaded instance-methods for
        /// more control over how the file is written. This method will use the default 19 field (geonames.org compatible)
        /// fileformat. Use <see cref="WriteGeoNames(string, IEnumerable{GeoName}, bool)"/> if you want to use a
        /// more compact 4 field format.
        /// </remarks>
        /// <seealso cref="WriteGeoNames(Stream, IEnumerable{GeoName}, bool)"/>
        public static void WriteGeoNames(Stream stream, IEnumerable<GeoName> values)
        {
            WriteGeoNames(stream, values, false);
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
        /// <seealso cref="WriteGeoNames(string, IEnumerable{GeoName})"/>
        public static void WriteGeoNames(string filename, IEnumerable<GeoName> values, bool useextendedfileformat)
        {
            new GeoFileWriter().WriteRecords(filename, values, new GeoNameComposer(useextendedfileformat));
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
        /// <seealso cref="WriteGeoNames(Stream, IEnumerable{GeoName})"/>
        public static void WriteGeoNames(Stream stream, IEnumerable<GeoName> values, bool useextendedfileformat)
        {
            new GeoFileWriter().WriteRecords(stream, values, new GeoNameComposer(useextendedfileformat));
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
        public static void WriteHierarchy(string filename, IEnumerable<HierarchyNode> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new HierarchyComposer());
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
        public static void WriteHierarchy(Stream stream, IEnumerable<HierarchyNode> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new HierarchyComposer());
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
        public static void WriteISOLanguageCodes(string filename, IEnumerable<ISOLanguageCode> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new ISOLanguageCodeComposer());
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
        public static void WriteISOLanguageCodes(Stream stream, IEnumerable<ISOLanguageCode> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new ISOLanguageCodeComposer());
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
        public static void WriteTimeZones(string filename, IEnumerable<TimeZone> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new TimeZoneComposer());
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
        public static void WriteTimeZones(Stream stream, IEnumerable<TimeZone> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new TimeZoneComposer());
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
        public static void WriteUserTags(string filename, IEnumerable<UserTag> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new UserTagComposer());
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
        public static void WriteUserTags(Stream stream, IEnumerable<UserTag> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new UserTagComposer());
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
        public static void WritePostalcodes(string filename, IEnumerable<Postalcode> values)
        {
            new GeoFileWriter().WriteRecords(filename, values, new PostalcodeComposer());
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
        public static void WritePostalcodes(Stream stream, IEnumerable<Postalcode> values)
        {
            new GeoFileWriter().WriteRecords(stream, values, new PostalcodeComposer());
        }
        #endregion
    }
}
