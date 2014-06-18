# ![Logo](https://raw.githubusercontent.com/RobThree/NGeoNames/master/icon.png) NGeoNames

Inspired by [OfflineReverseGeocode](https://github.com/AReallyGoodName/OfflineReverseGeocode) found at [this Reddit post](http://www.reddit.com/r/programming/comments/281msj/). Uses [KdTree](https://github.com/codeandcats/KdTree).

This library provides classes for downloading and parsing [files from GeoNames.org](download.geonames.org/export/dump/) and provides (reverse) geocoding methods like NearestNeighbourSearch and RadialSearch on the downloaded dataset(s).

This library is available as [NuGet package](https://www.nuget.org/packages/NGeoNames/).

## Basic usage / example

```c#
var datadirectory = @"D:\test\geo";

//Download file (optional; you can point a GeoFileReader to existing files ofcourse)
var downloader = new GeoFileDownloader();
downloader.DownloadFile("NL.txt", datadirectory);    //Download NL.txt to D:\foo\bar

//Read NL.txt file to memory
var cities = GeoFileReader.ReadExtendedGeoNames(Path.Combine(datadirectory, "NL.txt")).ToArray();   //"Materialize" file to memory by calling ToArray()

//We're going to use Amsterdam as "search-center"
var amsterdam = cities.Where(n => n.Name.Equals("Amsterdam", StringComparison.OrdinalIgnoreCase) && n.FeatureCode.Equals("PPLC")).First();

//Find first 50 items of interest closest to our center
var reversegeocoder = new ReverseGeoCode<ExtendedGeoName>(cities);
var results = reversegeocoder.RadialSearch(amsterdam, 250);  //Locate 250 geo-items near the center of Amsterdam
//Print the results
foreach (var r in results)
    Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}, {1} {2} ({3:F4}Km)", r.Latitude, r.Longitude, r.Name, r.DistanceTo(amsterdam)));
```
