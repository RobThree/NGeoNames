using NGeoNames;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace DumpTester
{
    /// <summary>
    /// Quick'n'Dirty program to download all dumpfiles and iterate over all of them to ensure parsing works for
    /// all files. Use at own risk!
    /// </summary>
    /// <remarks>
    /// This code is full of hacks / shortcuts / sloppy work; it's not intended to be used in producten
    /// </remarks>
    class Program
    {
        private static Uri BaseUri = new Uri(ConfigurationManager.AppSettings["baseuri"], UriKind.Absolute);
        private static string DownloadDirectory = ConfigurationManager.AppSettings["downloaddirectory"];

        static void Main(string[] args)
        {
            var dumpfiles = GetDumps();
            var downloader = new GeoFileDownloader(BaseUri);

            foreach (var geofile in dumpfiles)
            {
                Console.Write("Download: {0}", geofile.Filename);
                downloader.DownloadFile(geofile.Filename, DownloadDirectory);
                Console.Write(" Testing: ");
                Console.WriteLine("{0}", geofile.Test(Path.Combine(DownloadDirectory, geofile.Filename)));
            }

            Console.WriteLine("All done!");
        }

        private static GeoFile[] GetDumps()
        {
            return new[] {
                new GeoFile { Filename = "admin1CodesASCII.txt", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadAdmin1Codes(fn).Count(); }) },
                new GeoFile { Filename = "admin2Codes.txt", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadAdmin2Codes(fn).Count(); }) },
                new GeoFile { Filename = "allCountries.zip", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadExtendedGeoNames(fn).Count(); }) },
                new GeoFile { Filename = "alternateNames.zip", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadAlternateNames(fn).Count(); }) },
                new GeoFile { Filename = "cities1000.zip", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadExtendedGeoNames(fn).Count(); }) },
                new GeoFile { Filename = "cities15000.zip", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadExtendedGeoNames(fn).Count(); }) },
                new GeoFile { Filename = "cities5000.zip", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadExtendedGeoNames(fn).Count(); }) },
                new GeoFile { Filename = "countryInfo.txt", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadCountryInfo(fn).Count(); }) },
                //Featurecodes are downloaded by GetCountryDumps()
                new GeoFile { Filename = "hierarchy.zip", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadHierarchy(fn).Count(); }) },
                new GeoFile { Filename = "iso-languagecodes.txt", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadISOLanguageCodes(fn).Count(); }) },
                new GeoFile { Filename = "no-country.zip", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadExtendedGeoNames(fn).Count(); }) },
                new GeoFile { Filename = "timeZones.txt", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadTimeZones(fn).Count(); }) },
                new GeoFile { Filename = "userTags.zip", Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadUserTags(fn).Count(); }) },
            }.Union(GetCountryDumps()).ToArray();
        }

        private static GeoFile[] GetCountryDumps()
        {
            var w = new WebClient();
            var document = w.DownloadString(BaseUri);

            var countries = new Regex("href=\"([A-Z]{2}.zip)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                .Matches(document)
                .Cast<Match>()
                .Select(m => new GeoFile { Filename = m.Groups[1].Value, Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadExtendedGeoNames(fn).Count(); }) });

            var featurecodes = new Regex("href=\"(featureCodes_[A-Z]{2}.txt)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                .Matches(document)
                .Cast<Match>()
                .Select(m => new GeoFile { Filename = m.Groups[1].Value, Test = (f) => ExecuteTest(f, (fn) => { return GeoFileReader.ReadFeatureCodes(fn).Count(); }) });

            return countries.Union(featurecodes).OrderBy(m => m.Filename).ToArray();
        }

        private static string ExecuteTest(string filename, Func<string, int> test)
        {
            try
            {
                //Haaaack
                var file = filename.Replace(".zip", ".txt");
                
                //Haaaaaaaaaack
                if (file.EndsWith("no-country.txt"))
                    file = file.Replace("no-country.txt", "null.txt");

                return string.Format("{0} records OK", test(file));
            }
            catch (Exception ex)
            {
                return string.Format("FAILED: {0}", ex.Message);
            }
        }
    }

    class GeoFile
    {
        public string Filename { get; set; }
        public Func<string, string> Test { get; set; }

        public GeoFile()
        {
            this.Test = (f) => { throw new NotImplementedException(); };
        }
    }
}
