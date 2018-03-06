using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Cache;

namespace NGeoNames
{
    //TODO: Add ASYNC methods/support

    /// <summary>
    /// Provides methods to download files from geonames.org.
    /// </summary>
    /// <remarks>
    /// This class is, essentially, a small wrapper with some extra (specific) functionality for the
    /// <see cref="WebClient"/>-class. When downloading ZIP files from geonames.org it will automatically extract the
    /// archives. It also handles (in a very simple manner) caching of downloaded files to prevent downloading files
    /// more than desired.
    /// </remarks>
    public class GeoFileDownloader
    {
        /// <summary>
        /// Gets the default URI where geonames.org export files can be found.
        /// </summary>
        public static readonly Uri DEFAULTGEOFILEBASEURI = new Uri("http://download.geonames.org/export/dump/", UriKind.Absolute);

        /// <summary>
        /// Gets the default URI where geonames.org postal codes files can be found.
        /// </summary>
        public static readonly Uri DEFAULTPOSTALCODEBASEURI = new Uri("http://download.geonames.org/export/zip/", UriKind.Absolute);

        /// <summary>
        /// Gets the useragent string used to identify when downloading files from geonames.org.
        /// </summary>
        public static readonly string USERAGENT = string.Format("{0} v{1}", typeof(GeoFileDownloader).Assembly.GetName().Name, typeof(GeoFileDownloader).Assembly.GetName().Version.ToString());

        /// <summary>
        /// Gets/sets the base URI to use when downloading files and relative paths are specified.
        /// </summary>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// Gets/sets the application's cache policy for any resources obtained by this GeoFileDownloader instance.
        /// </summary>
        public RequestCachePolicy CachePolicy { get; set; }

        /// <summary>
        /// Gets/sets the network credentials that are sent to the host and used to authenticate the request.
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary>
        /// Gets/sets the proxy used by this GeoFileDownloader object.
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Gets/sets the default 'Time To Live'; specifying how long already downloaded files are deemed 'valid' and
        /// won't require actually downloading again.
        /// </summary>
        public TimeSpan DefaultTTL { get; set; }

        /// <summary>
        /// Initializes a new instance of the GeoFileDownloader class using the specified URI as <see cref="BaseUri"/>.
        /// </summary>
        /// <param name="baseUri">The base URI to use when downloading files and relative paths are specified.</param>
        /// <remarks>
        /// The <see cref="DefaultTTL"/> is 24 hours.
        /// </remarks>
        public GeoFileDownloader(Uri baseUri)
            : this(baseUri, TimeSpan.FromHours(24)) { }

        /// <summary>
        /// Initializes a new instance of the GeoFileDownloader class using the specified URI as <see cref="BaseUri"/>
        /// and specified TTL.
        /// </summary>
        /// <param name="baseUri">The base URI to use when downloading files and relative paths are specified.</param>
        /// <param name="ttl">The <see cref="DefaultTTL"/> to use.</param>
        public GeoFileDownloader(Uri baseUri, TimeSpan ttl)
        {
            BaseUri = baseUri;
            DefaultTTL = ttl;
        }

        /// <summary>
        /// Initializes a new instance of the GeoFileDownloader class for downloading GeoName files.
        /// </summary>
        /// <returns>Returns a default GeoFileDownloader intialized with the default geofiles URI as base URI.</returns>
        /// <remarks>
        /// The <see cref="DefaultTTL"/> is 24 hours.
        /// </remarks>
        public static GeoFileDownloader CreateGeoFileDownloader()
        {
            return new GeoFileDownloader(DEFAULTGEOFILEBASEURI);
        }

        /// <summary>
        /// Initializes a new instance of the GeoFileDownloader class for downloading GeoName files with the specified TTL.
        /// </summary>
        /// <param name="ttl">The <see cref="DefaultTTL"/> to use.</param>
        /// <returns>
        /// Returns a default GeoFileDownloaderintialized with the default geofiles URI as base URI and with specified
        /// <see cref="DefaultTTL">TTL</see>.
        /// </returns>
        public static GeoFileDownloader CreateGeoFileDownloader(TimeSpan ttl)
        {
            return new GeoFileDownloader(DEFAULTGEOFILEBASEURI, ttl);
        }

        /// <summary>
        /// Initializes a new instance of the GeoFileDownloader class for downloading postal code files.
        /// </summary>
        /// <returns>Returns a default GeoFileDownloader intialized with the default postalcode URI as base URI.</returns>
        /// <remarks>
        /// The <see cref="DefaultTTL"/> is 24 hours.
        /// </remarks>
        public static GeoFileDownloader CreatePostalcodeDownloader()
        {
            return new GeoFileDownloader(DEFAULTPOSTALCODEBASEURI);
        }

        /// <summary>
        /// Initializes a new instance of the GeoFileDownloader class for downloading postal code files with the specified TTL.
        /// </summary>
        /// <param name="ttl">The <see cref="DefaultTTL"/> to use.</param>
        /// <returns>
        /// Returns a default GeoFileDownloaderintialized with the default postalcode URI as base URI and with specified
        /// <see cref="DefaultTTL">TTL</see>.
        /// </returns>
        /// <remarks>
        /// The <see cref="DefaultTTL"/> is 24 hours.
        /// </remarks>
        public static GeoFileDownloader CreatePostalcodeDownloader(TimeSpan ttl)
        {
            return new GeoFileDownloader(DEFAULTPOSTALCODEBASEURI, ttl);
        }

        /// <summary>
        /// Downloads the specified file to the destination path using the <see cref="DefaultTTL"/> to determine if an
        /// existing version is still valid.
        /// </summary>
        /// <param name="uri">
        /// The URI (either relative like "US.txt" or absolute like "http://site.tld/foo/bar/US.txt") of the file to download.
        /// </param>
        /// <param name="destinationpath">
        /// The path where the file should be stored; this can be either a directory (e.g. d:\foo\bar\ where the original
        /// filename will be used) or a filename (e.g. d:\foo\bar\myfile.txt where the original filename is ignored).
        /// </param>
        /// <returns>Returns the path(s) to the file(s) downloaded.</returns>
        /// <remarks>
        /// When a ZIP archive is downloaded the archive will automatically be extracted; this is why this method returns
        /// a string-array: there may be more than one file in the archive. This method uses the <see cref="DefaultTTL"/>.
        /// </remarks>
        public string[] DownloadFile(string uri, string destinationpath)
        {
            return DownloadFile(new Uri(uri, UriKind.RelativeOrAbsolute), destinationpath);
        }

        /// <summary>
        /// Downloads the specified file to the destination path using the <see cref="DefaultTTL"/> to determine if an
        /// existing version is still valid.
        /// </summary>
        /// <param name="uri">
        /// The URI (either relative like "US.txt" or absolute like "http://site.tld/foo/bar/US.txt") of the file to download.
        /// </param>
        /// <param name="destinationpath">
        /// The path where the file should be stored; this can be either a directory (e.g. d:\foo\bar\ where the original
        /// filename will be used) or a filename (e.g. d:\foo\bar\myfile.txt where the original filename is ignored).
        /// </param>
        /// <returns>Returns the path(s) to the file(s) downloaded.</returns>
        /// <remarks>
        /// When a ZIP archive is downloaded the archive will automatically be extracted; this is why this method returns
        /// a string-array: there may be more than one file in the archive. This method uses the <see cref="DefaultTTL"/>.
        /// </remarks>
        public string[] DownloadFile(Uri uri, string destinationpath)
        {
            return DownloadFileWhenOlderThan(uri, destinationpath, DefaultTTL);
        }

        /// <summary>
        /// Downloads the specified file to the destination path using the specified TTL.
        /// </summary>
        /// <param name="uri">
        /// The URI (either relative like "US.txt" or absolute like "http://site.tld/foo/bar/US.txt") of the file to download.
        /// </param>
        /// <param name="destinationpath">
        /// The path where the file should be stored; this can be either a directory (e.g. d:\foo\bar\ where the original
        /// filename will be used) or a filename (e.g. d:\foo\bar\myfile.txt where the original filename is ignored).
        /// </param>
        /// <param name="ttl">
        /// The TTL (Time To Live) for the file downloaded; when the TTL is expired the file will be downloaded again, if
        /// not the file won't be downloaded again. To determine if the TTL has expired the file's createdate and current
        /// time are used.
        /// </param>
        /// <returns>Returns the path(s) to the file(s) downloaded.</returns>
        /// <remarks>
        /// When a ZIP archive is downloaded the archive will automatically be extracted; this is why this method returns
        /// a string-array: there may be more than one file in the archive.
        /// </remarks>
        public string[] DownloadFileWhenOlderThan(string uri, string destinationpath, TimeSpan ttl)
        {
            return DownloadFileWhenOlderThan(new Uri(uri, UriKind.RelativeOrAbsolute), destinationpath, ttl);
        }

        /// <summary>
        /// Downloads the specified file to the destination path using the specified TTL.
        /// </summary>
        /// <param name="uri">
        /// The URI (either relative like "US.txt" or absolute like "http://site.tld/foo/bar/US.txt") of the file to download.
        /// </param>
        /// <param name="destinationpath">
        /// The path where the file should be stored; this can be either a directory (e.g. d:\foo\bar\ where the original
        /// filename will be used) or a filename (e.g. d:\foo\bar\myfile.txt where the original filename is ignored).
        /// </param>
        /// <param name="ttl">
        /// The TTL (Time To Live) for the file downloaded; when the TTL is expired the file will be downloaded again, if
        /// not the file won't be downloaded again. To determine if the TTL has expired the file's createdate and current
        /// time are used.
        /// </param>
        /// <returns>Returns the path(s) to the file(s) downloaded.</returns>
        /// <remarks>
        /// When a ZIP archive is downloaded the archive will automatically be extracted; this is why this method returns
        /// a string-array: there may be more than one file in the archive.
        /// </remarks>
        public string[] DownloadFileWhenOlderThan(Uri uri, string destinationpath, TimeSpan ttl)
        {
            var downloaduri = DetermineDownloadPath(uri);
            destinationpath = DetermineDestinationPath(downloaduri, destinationpath);

            if (IsFileExpired(destinationpath, ttl))
            {
                using (var w = new WebClient())
                {
                    w.CachePolicy = CachePolicy;
                    w.Credentials = Credentials;
                    w.Proxy = Proxy;
                    w.Headers.Add(HttpRequestHeader.UserAgent, USERAGENT);
                    w.DownloadFile(downloaduri, destinationpath);
                }
            }

            if (Path.GetExtension(destinationpath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                return UnzipFiles(destinationpath, ttl);
            return new[] { destinationpath };
        }

        /// <summary>
        /// Determines if a file is 'expired' by it's TTL.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="ttl">The TTL for the file.</param>
        /// <returns>Returns true when the file has expired it's TTL *or* the file doesn't exist; false otherwise.</returns>
        private static bool IsFileExpired(string path, TimeSpan ttl)
        {
            return (!File.Exists(path) || (DateTime.UtcNow - new FileInfo(path).LastWriteTimeUtc) > ttl);
        }

        /// <summary>
        /// Extracts a ZIP archive and returns the extracted files as a string array.
        /// </summary>
        /// <param name="path">The path to extract to.</param>
        /// <param name="ttl">The TTL for the extracted files.</param>
        /// <returns>A string array of extracted files.</returns>
        /// <remarks>Removes "readme"'s (these aren't extracted).</remarks>
        private static string[] UnzipFiles(string path, TimeSpan ttl)
        {
            var files = new List<string>();
            using (var f = File.OpenRead(path))
            {
                using (var z = new ZipArchive(f, ZipArchiveMode.Read))
                {
                    foreach (var c in z.Entries.Where(n => !n.Name.StartsWith("readme", StringComparison.OrdinalIgnoreCase)))
                    {
                        var dest = Path.Combine(Path.GetDirectoryName(path), c.Name);
                        if (IsFileExpired(dest, ttl))
                        {
                            using (var e = File.Create(dest))
                                c.Open().CopyTo(e);
                        }
                        files.Add(dest);
                    }
                }
            }
            return files.ToArray();
        }

        /// <summary>
        /// Combines relative path with <see cref="BaseUri"/> to create an absolute path for downloading files.
        /// </summary>
        /// <param name="uri">The URI to combine with <see cref="BaseUri"/> when it is relative.</param>
        /// <returns>
        /// Returns an absolute URI; this will be the passed in URI when it is/was absolute, otherwise it will be the
        /// <see cref="BaseUri"/> + passed in URI combined.
        /// </returns>
        private Uri DetermineDownloadPath(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                return new Uri(BaseUri, uri.OriginalString);
            return uri;
        }

        private static string DetermineDestinationPath(Uri uri, string path)
        {
            if (Directory.Exists(path))
                path = Path.Combine(path, Path.GetFileName(uri.AbsolutePath));
            return path;
        }
    }
}
