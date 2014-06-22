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
        #endregion
    }
}
