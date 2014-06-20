# ![Logo](https://raw.githubusercontent.com/RobThree/NGeoNames/master/icon.png) NGeoNames

Inspired by [OfflineReverseGeocode](https://github.com/AReallyGoodName/OfflineReverseGeocode) found at [this Reddit post](http://www.reddit.com/r/programming/comments/281msj/). You may also be interested in [GeoSharp](https://github.com/Necrolis/GeoSharp). Uses [KdTree](https://github.com/codeandcats/KdTree). Built and tested on .Net 4.5.

This library provides classes for downloading and parsing [files from GeoNames.org](download.geonames.org/export/dump/) and provides (reverse) geocoding methods like `NearestNeighbourSearch()` and `RadialSearch()` on the downloaded dataset(s).

This library is available as [NuGet package](https://www.nuget.org/packages/NGeoNames/).

## Basic usage / example / "quick start"

```c#
var datadir = @"D:\test\geo\";

// Download file (optional; you can point a GeoFileReader to existing files ofcourse)
var downloader = new GeoFileDownloader();
downloader.DownloadFile("NL.txt", datadir);    // Download NL.txt to D:\test\geo\

// Read NL.txt file to memory
var cities = GeoFileReader.ReadExtendedGeoNames(Path.Combine(datadir, "NL.txt")).ToArray();   
// Note: we "Materialize" the file to memory by calling ToArray()

// We're going to use Amsterdam as "search-center"
var amsterdam = cities.Where(n => 
        n.Name.Equals("Amsterdam", StringComparison.OrdinalIgnoreCase) 
        && n.FeatureCode.Equals("PPLC")
    ).First();

// Find first 50 items of interest closest to our center
var reversegeocoder = new ReverseGeoCode<ExtendedGeoName>(cities);
// Locate 250 geo-items near the center of Amsterdam
var results = reversegeocoder.RadialSearch(amsterdam, 250);  
// Print the results
foreach (var r in results) {
    Console.WriteLine(
        string.Format(
            CultureInfo.InvariantCulture, "{0}, {1} {2} ({3:F4}Km)", 
            r.Latitude, r.Longitude, r.Name, r.DistanceTo(amsterdam)
        )
    );
}
```

## Overview

The library provides for the following main operations:

1. [Downloading / retrieving data from geonames.org](#downloading) (Optional)
2. [Reading / parsing geonames.org data](#parsing)
3. [Utilizing geonames.org data](#utilizing)

The library consists mainly of parsers and entities (in the `NGeoNames.Entities` and `NGeoNames.Parsers` namespaces) and a `GeoFileReader` to read and parse files from geonames.org, a `GeoFileDownloader` to retrieve files from geonames.org and a `ReverseGeoCode<T>` class to do the heavy lifting of the reverse geocoding.

Because some "geoname files" can be very large (like `allcountries.txt`) we have a `GeoName` entity which is a simplified version (and baseclass) of an `ExtendedGeoName`. The `GeoName` class contains a unique id which can be used to resolve the `ExtendedGeoName` easily for more information when required. It is, however, recommended to use `<countrycode>.txt` (e.g. `GB.txt`) or `cities1000.txt` for example to reduce the dataset to a smaller size.

Also worth noting is that the readers return an `IEnumerable<SomeEntity>`; make sure that you materialize these enumerables to a list, array or other datastructure (using `.ToList()`, `.ToArray()`, `.ToDictionary` etc.) if you access it more than once to avoid file I/O to the underlying file each time you access the data.

### <a name="downloading"></a>Downloading / retrieving data from geonames.org (Optional)

To download files from geonames.org you can use the `GeoFileDownloader` class which is, in essence, a wrapper for a basic [`WebClient`](http://msdn.microsoft.com/en-us/library/system.net.webclient.aspx). The simplest form is:

```c#
// Downloads (and extracts) NL.zip from geonames.org to D:\my\geodata\directory
new GeoFileDownloader()
    .DownloadFile("NL.zip", @"D:\my\geodata\directory");
```

You can specify the BaseUrl in the `GeoFileDownloader` constructor or just pass an absolute url to the `DownloadFile()` method if you want to use another location than the default `http://download.geonames.org/export/dump/`. The class has properties to set a (HTTP) `CachePolicy`, `Proxy` and `Credentials` to use when downloading the file. The filedownloader, by default, downloads a file only if the destination file doesn't exist *or* when the destination file has "expired" (by default 24 hours). It uses the files CreationDate to determine when the file was downloaded and if a newer version should be downloaded. The "TTL", how long a file will be 'valid', can be set using the `DefaultTTL` property of the `GeoFileDownloader` class. You can also use the `DownloadFileWhenOlderThan()` method which allows you to explicitly set a TTL. When a filename is specified (e.g. `d:\folder\foo.txt`) the file will be named accordingly.

Zipfiles are automatically extracted in the destinationfolder; the original zipfile is preserved because the `GeoFileDownloader` needs to know which files are supposed to be in the zipfile and thus in the destinationdirectory in their extracted form.

### <a name="parsing"></a>Reading / parsing geonames.org data

Once files are downloaded using the `GeoFileDownloader`, *or* by using your own custom/specific implementation, the files can be accessed using the `GeoFileReader` class. This class contains a number of static "convenience methods" like `ReadGeoNames()` and it's "sibling" `ReadExtendedGeoNames()`. but also `ReadCountryInfo()`, `ReadAlternateNames()` etc. There is a "convenience method" for each entity.

```c#
// Open file "cities1000.txt" and retrieve only cities in the US
var cities_in_us = GeoFileReader.ReadExtendedGeoNames(@"D:\my\geodata\cities1000.txt")
        .Where(p => p.CountryCode.Equals("US", StringComparison.OrdinalIgnoreCase))
        .OrderBy(p => p.Name);
```

Again, **please note** that `Read<Something>` methods return an `IEnumerable<T>`. Whenever you want to access the data more than once you will probably want to call `.ToArray()` or similar to materialize the data into memory. The `GeoFileReader` class has two static method (`ReadBuiltInContinents()` and `ReadBuiltInFeatureClasses()`) that can be used to use built-in values for continents and [feature codes](http://www.geonames.org/export/codes.html) which are not provided by geonames.org as downloadable files. You can, however, craft your own files for this purpose and use the `ReadContinents()` and `ReadFeatureClasses()` if you want to specify your own values / update built-in values (should `NGeoNames`'s values be outdated for example).

You can also add your own entities and, as long as you provide a parser for it, use the `GeoFileReader` class to read/parse files for these entities as well:

```c#
var data = new GeoFileReader().ReadRecords<MyEntity>("d:\foo\bar.txt", new MyEntityParser());
```

As long as your parser implements `IParser<MyEntity>` you're good to go. A parser can skip a fixed number of lines in a file (for example a 'header' record), skip comments (for example lines starting with `#`) and you can even specify the encoding to use etc. Examples and more information can be found in the unittests.

Another thing to note is that the `GeoFileReader` will try to "autodetect" if the file is a plain text file (`.txt` extension) or a GZipped file (`.gz` extension). Support for GZip was added to keep the footprint of the files lower when desired. This will, however, trade-off I/O speed and CPU load for space. The `ReadRecords<T>()` method has an overload wher you can explicitly specify the type of the file (should you want to use your own file-extensions like `.dat` for example).

> Support for compressing downloaded files using the `GeoFileDownloader` on the fly is planned for a later version; for now you will have to GZip the files manually.

The `GeoFileReader` also supports the use of [`Stream`](http://msdn.microsoft.com/en-us/library/system.io.stream.aspx)s so you can provide data from a MemoryStream for example or any other source that can be wrapped in a stream.

As you'll probably realize by now, the `GeoFileReader` class *combined* with [LINQ](http://msdn.microsoft.com/en-us/library/bb397926.aspx) allows for very powerful querying, filtering and sorting of the data.

> I am playing with the idea of creating a `GeoFileWriter<T>` class (and corresponding IWriter<T> interface and classes implementing them for each entity) so you can \*write\* "geo files". This would allow you to, for example, easily combine several country files (e.g. "BE.txt", "NL.txt", "LU.txt" for [Benelux](http://en.wikipedia.org/wiki/Benelux)) into one file or pass "allCountries.txt" through a (series of) (LINQ) filter(s) etc. to create your own "persisted" views on the data.

### <a name="utilizing"></a>Utilizing geonames.org data

The 'heart' of the library is the `ReverseGeoCode<T>` class. When you supply it with either `IEnumerable<GeoNames>` or `IEnumerable<ExtendedGeoNames>` it can be used to do a `RadialSearch()` or `NearestNeighbourSearch()`. Supplying the class with data can be done by either passing it to the class constructor or by using the `Add()` or `AddRange()` methods. You may want to call the `Balance()` method to balance the internal KD-tree, however; this is done automatically when the data is supplied via the constructor.

```c#
// Create our ReverseGeoCode class and supply it with data
var r = new ReverseGeoCode<ExtendedGeoName>(
        GeoFileReader.ReadExtendedGeoNames(@"D:\foo\cities1000.txt")
    );
            
// Create a point from a lat/long pair from which we want to conduct our search(es) (center)
var new_york = r.CreateFromLatLong(40.7056308, -73.9780035);

// Find 10 nearest
r.NearestNeighbourSearch(new_york, 10);
```

Note that the library is based on the [**International System of Units (SI)**](http://en.wikipedia.org/wiki/International_System_of_Units); units of distance for example are specified in **meters**. If you want to use imperial system (e.g. miles, nautical miles, yards, foot and whathaveyou's) you need to convert to meters.

The `GeoName` class (and, by extension, the `ExtendedGeoName` class) has a `DistanceTo()` method which can be used to determine the exact distance betweem two points.

Both the `NearestNeighbourSearch()` and `RadialSearch()` methods have some overloads that accept lat/long pairs as *doubles* as well.

## Project status

<img src="http://riii.nl/womm" width="200" height="200" align="left"> The project will be updated from time-to-time when required. I am happy to accept pull-requests; if you're interested in contributing to this library please contact me. More, better and extensive documentation ('[XMLDoc](http://msdn.microsoft.com/en-us/library/vstudio/b2s063f7(v=vs.100).aspx)') is currently in the works.

[![Build status](https://ci.appveyor.com/api/projects/status/mkmbxvm1w0mxaifv)](https://ci.appveyor.com/project/RobIII/ngeonames)
