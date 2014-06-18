using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;
using NGeoNames.Entities;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NGeoNamesTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var fileroot = @"D:\test\geo";

            var x = new GeoFileDownloader();
            
            x.DownloadFile("admin1CodesASCII.txt", fileroot);
            x.DownloadFile("admin2Codes.txt", fileroot);
            x.DownloadFile("countryInfo.txt", fileroot);
            x.DownloadFile("featureCodes_en.txt", fileroot);
            x.DownloadFile("iso-languagecodes.txt", fileroot);
            x.DownloadFile("timeZones.txt", fileroot);

            x.DownloadFile("NL.zip", fileroot);
            x.DownloadFile("allCountries.zip", fileroot);
            x.DownloadFile("alternateNames.zip", fileroot);
            x.DownloadFile("cities1000.zip", fileroot);
            x.DownloadFile("cities15000.zip", fileroot);
            x.DownloadFile("cities5000.zip", fileroot);
            x.DownloadFile("hierarchy.zip", fileroot);
            x.DownloadFile("no-country.zip", fileroot);
            x.DownloadFile("userTags.zip", fileroot);

            //var i = GeoFileReader.ReadAdmin1Codes(Path.Combine(fileroot, "admin1CodesASCII.txt")).ToDictionary(k => k.Code);
            //var j = GeoFileReader.ReadAdmin2Codes(Path.Combine(fileroot, "admin2Codes.txt")).ToDictionary(k => k.Code);
            //var d = GeoFileReader.ReadCountryInfo(Path.Combine(fileroot, "countryinfo.txt")).ToDictionary(k => k.ISO_Alpha2);
            //var e = GeoFileReader.ReadISOLanguageCodes(Path.Combine(fileroot, "iso-languagecodes.txt")).ToDictionary(k => k.ISO_639_3);
            //var f = GeoFileReader.ReadTimeZones(Path.Combine(fileroot, "timeZones.txt")).ToDictionary(k => k.TimeZoneId);

            var c = GeoFileReader.ReadExtendedGeoNames(Path.Combine(fileroot, "NL.txt"));
            ////var c = GeoFileReader.ReadGeoNames(Path.Combine(fileroot, "allCountries.txt")).ToDictionary(k => k.Id);

            //var c = GeoFileReader.ReadExtendedGeoNames(Path.Combine(fileroot, "cities1000.txt")).ToDictionary(k => k.Id);
            //var g = GeoFileReader.ReadFeatureCodes(Path.Combine(fileroot, "featureCodes_en.txt")).ToDictionary(k => k.Class + "." + k.Code);
            //var h = GeoFileReader.ReadBuiltInContinents().ToDictionary(k => k.Code);
            //var l = GeoFileReader.ReadHierarchy(Path.Combine(fileroot, "hierarchy.txt")).ToArray();
            ////var m = GeoFileReader.ReadAlternateNames(Path.Combine(fileroot, "alternateNames.txt")).ToDictionary(k => k.Id);
            //var n = GeoFileReader.ReadUserTags(Path.Combine(fileroot, "userTags.txt")).GroupBy(k => k.GeoNameId).ToDictionary(k => k.Key, k => k.ToArray());
            //var o = GeoFileReader.ReadBuiltInFeatureClasses().ToDictionary(k => k.Class);

            //var t = new KDTree.KDTree<ExtendedGeoName>(c.Values.Where(p => (p.FeatureClass == "P" && p.FeatureCode.StartsWith("PPL")) || (p.FeatureClass == "A" && p.FeatureCode.StartsWith("ADM"))));

            //var search = new ExtendedGeoName { Latitude = 51.55583, Longitude = 5.69028, Name = "Search" };
            //foreach (var r in t.NearestNeighbors(search.GetCoord(), 50))
            //    Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0},{1} {2} ({3:F4}) [{4}]", r.Latitude, r.Longitude, r.Name, r.DistanceTo(search), r.Population));


            var t = new ReverseGeoCode<GeoName>(c.Where(p => (p.FeatureClass == "P" && p.FeatureCode.StartsWith("PPL")) || (p.FeatureClass == "A" && p.FeatureCode.StartsWith("ADM"))));
            var search = new GeoName { Latitude = 51.55583, Longitude = 5.69028, Name = "Search" };
            foreach (var r in t.RadialSearch(search, 10000, 1000))
                Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0},{1} {2} ({3:F4})", r.Latitude, r.Longitude, r.Name, r.DistanceTo(search)));

            foreach (var r in t.NearestNeighbourSearch(search))
                Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0},{1} {2} ({3:F4})", r.Latitude, r.Longitude, r.Name, r.DistanceTo(search)));
        }
    }
}
