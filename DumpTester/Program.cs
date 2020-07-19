using Microsoft.Extensions.DependencyInjection;
using NGeoNames;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DumpTester
{
    /// <summary>
    /// Quick'n'Dirty program to download all dumpfiles and iterate over all of them to ensure parsing works for
    /// all files. Use at own risk!
    /// </summary>
    /// <remarks>
    /// This code is full of hacks / shortcuts / sloppy work; it's NOT intended to be used in production. This project
    /// is a simple "brute force" approach to test all of the files (by forcing the parsers to parse each record) made 
    /// available by geonames.org.
    /// </remarks>
    class Program
    {
        private static readonly string Dump_DownloadDirectory = ConfigurationManager.AppSettings["dump_downloaddirectory"];
        private static readonly string Postal_DownloadDirectory = ConfigurationManager.AppSettings["postal_downloaddirectory"];

        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddGeoNames();
            var serviceProvider = services.BuildServiceProvider();

            //Test GeoName dumps
            var dumpdownloader = serviceProvider.GetRequiredService<IGeoFileGeoDownloader>();
            var dumpfiles = await GetDumps(dumpdownloader).ConfigureAwait(false);

            Directory.CreateDirectory(Dump_DownloadDirectory);

            await Task.WhenAll(
                dumpfiles.Select(async f =>
                {
                    Console.WriteLine("Download: {0}", f.Filename);
                    var result = await dumpdownloader.DownloadFileAsync(f.Filename, Dump_DownloadDirectory).ConfigureAwait(false);
                    Console.WriteLine("Testing {0}: {1}", f.Filename, await f.Test(Path.Combine(Dump_DownloadDirectory, f.Filename)).ConfigureAwait(false));
                    return result;
                })
            ).ConfigureAwait(false);

            //Test Postalcode dumps
            var postalcodedownloader = serviceProvider.GetRequiredService<IGeoFilePostalDownloader>();
            var postalcodefiles = await GetCountryPostalcodes(postalcodedownloader).ConfigureAwait(false);

            Directory.CreateDirectory(Postal_DownloadDirectory);

            await Task.WhenAll(
                postalcodefiles.Select(async f =>
                {
                    Console.WriteLine("Download: {0}", f.Filename);
                    var result = await postalcodedownloader.DownloadFileAsync(f.Filename, Postal_DownloadDirectory).ConfigureAwait(false);
                    Console.WriteLine("Testing {0}: {1}", f.Filename, await f.Test(Path.Combine(Postal_DownloadDirectory, f.Filename)).ConfigureAwait(false));
                    return result;
                })
            ).ConfigureAwait(false);

            //Console.WriteLine("Testing ASCII fields");
            //await DumpASCIILies(Dump_DownloadDirectory).ConfigureAwait(false);

            Console.WriteLine("All done!");
        }

        private static async Task<GeoFile[]> GetCountryPostalcodes(IGeoFilePostalDownloader downloader)
        {
            var w = new WebClient();
            var document = w.DownloadString(downloader.BaseUri);


            var countries = new Regex("href=\"([A-Z]{2}.zip)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                .Matches(document)
                .Cast<Match>()
                .Select(m => new GeoFile { Filename = m.Groups[1].Value, Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadPostalcodes(fn).CountAsync()).ConfigureAwait(false) });

            return new[] {
                new GeoFile { Filename = "allCountries.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadPostalcodes(fn).CountAsync()).ConfigureAwait(false) }
            }.Union(countries.OrderBy(m => m.Filename)).ToArray();
        }

        private static async Task<GeoFile[]> GetDumps(IGeoFileGeoDownloader downloader)
        {
            return new[] {
                new GeoFile { Filename = "admin1CodesASCII.txt", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadAdmin1Codes(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "admin2Codes.txt", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadAdmin2Codes(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "allCountries.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadExtendedGeoNames(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "alternateNames.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadAlternateNames(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "alternateNamesV2.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadAlternateNamesV2(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "cities500.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadExtendedGeoNames(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "cities1000.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadExtendedGeoNames(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "cities15000.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadExtendedGeoNames(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "cities5000.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadExtendedGeoNames(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "countryInfo.txt", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadCountryInfo(fn).CountAsync()).ConfigureAwait(false) },
                //Featurecodes are downloaded by GetCountryDumps()
                new GeoFile { Filename = "hierarchy.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadHierarchy(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "iso-languagecodes.txt", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadISOLanguageCodes(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "no-country.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadExtendedGeoNames(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "timeZones.txt", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadTimeZones(fn).CountAsync()).ConfigureAwait(false) },
                new GeoFile { Filename = "userTags.zip", Test = async f => await ExecuteTest(f, fn => GeoFileReader.ReadUserTags(fn).CountAsync()).ConfigureAwait(false) },
            }.Union(await GetCountryDumps(downloader)).ToArray();
        }

        private static async Task<GeoFile[]> GetCountryDumps(IGeoFileGeoDownloader downloader)
        {
            var w = new WebClient();
            var document = w.DownloadString(downloader.BaseUri);

            var countries = new Regex("href=\"([A-Z]{2}.zip)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                .Matches(document)
                .Cast<Match>()
                .Select(m => new GeoFile { Filename = m.Groups[1].Value, Test = (f) => ExecuteTest(f, fn => GeoFileReader.ReadExtendedGeoNames(fn).CountAsync()) });

            var featurecodes = new Regex("href=\"(featureCodes_[A-Z]{2}.txt)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                .Matches(document)
                .Cast<Match>()
                .Select(m => new GeoFile { Filename = m.Groups[1].Value, Test = (f) => ExecuteTest(f, fn => GeoFileReader.ReadFeatureCodes(fn).CountAsync()) });

            return countries.Union(featurecodes).OrderBy(m => m.Filename).ToArray();
        }

        private static async ValueTask<string> ExecuteTest(string filename, Func<string, ValueTask<int>> test)
        {
            try
            {
                //Haaaack
                var file = filename.Replace(".zip", ".txt");

                //Haaaaaaaaaack
                //if (file.EndsWith("no-country.txt"))
                //    file = file.Replace("no-country.txt", "null.txt");

                return string.Format("{0} records OK", await test(file));
            }
            catch (Exception ex)
            {
                return string.Format("FAILED: {0}", ex.Message);
            }
        }

        //private static async Task DumpASCIILies(string logpath)
        //{
        //    using (var lw = File.CreateText(Path.Combine(logpath, "_asciilies.log")))
        //    {

        //        //Test for fields that claim to contain ASCII only but contain non-ASCII data anyways
        //        var nonasciifilter = new Regex("[^\x20-\x7F]", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        //        var geofilefilter = new Regex("^[A-Z]{2}.txt$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        //        lw.WriteLine("The following files contain entries that claim to contain ASCII only but contain non-ASCII data anyways:");

        //        var extgeofiles = new[] { "allCountries", "cities1000", "cities5000", "cities15000", "no-country" }
        //            .Select(f => Path.Combine(Dump_DownloadDirectory, f + ".txt"))
        //            .Union(Directory.GetFiles(Dump_DownloadDirectory, "*.txt")
        //                .Where(f => geofilefilter.IsMatch(Path.GetFileName(f)))
        //        );

        //        var lies = extgeofiles.AsParallel()
        //            .SelectMany(async f => (await GeoFileReader.ReadExtendedGeoNames(f).ToArrayAsync()).AsParallel()
        //                .Where(e => nonasciifilter.IsMatch(e.NameASCII))
        //                .Select(i => new NonASCIIEntry { FileName = f, Id = i.Id, Value = i.NameASCII })
        //            ).Union(
        //                (await GeoFileReader.ReadAdmin1Codes(Path.Combine(Dump_DownloadDirectory, "admin1CodesASCII.txt")).ToArrayAsync()).AsParallel()
        //                    .Where(c => nonasciifilter.IsMatch(c.NameASCII))
        //                    .Select(i => new NonASCIIEntry { FileName = "admin1CodesASCII.txt", Id = i.GeoNameId, Value = i.NameASCII })
        //            ).Union(
        //                (await GeoFileReader.ReadAdmin2Codes(Path.Combine(Dump_DownloadDirectory, "admin2Codes.txt")).ToArrayAsync()).AsParallel()
        //                    .Where(c => nonasciifilter.IsMatch(c.NameASCII))
        //                    .Select(i => new NonASCIIEntry { FileName = "admin2Codes.txt", Id = i.GeoNameId, Value = i.NameASCII })
        //            );

        //        foreach (var l in lies.OrderBy(l => l.FileName).ThenBy(l => l.Value))
        //        {
        //            lw.WriteLine(string.Join("\t", Path.GetFileName(l.FileName), l.Id, l.Value));
        //        };

        //    }
        //}
    }

    class GeoFile
    {
        public string Filename { get; set; }
        public Func<string, ValueTask<string>> Test { get; set; }

        public GeoFile()
        {
            //this.Test = (f) => { throw new NotImplementedException(); };
        }
    }

    class NonASCIIEntry
    {
        public string FileName { get; set; }
        public string Value { get; set; }
        public int Id { get; set; }
    }
}