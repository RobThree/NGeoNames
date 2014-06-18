using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Reflection;

namespace NGeoNames
{
    //TODO: Add ASYNC methods/support
    public class GeoFileDownloader
    {
        public static readonly Uri DEFAULTBASEURI = new Uri("http://download.geonames.org/export/dump/", UriKind.Absolute);

        public static readonly string USERAGENT = string.Format("{0} v{1}", Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly().GetName().Version.ToString());

        public Uri BaseUri { get; set; }
        public RequestCachePolicy CachePolicy { get; set; }
        public ICredentials Credentials { get; set; }
        public IWebProxy Proxy { get; set; }
        public TimeSpan DefaultTTL { get; set; }

        public GeoFileDownloader()
            : this(DEFAULTBASEURI) { }

        public GeoFileDownloader(Uri baseUri)
        {
            this.BaseUri = baseUri;
            this.DefaultTTL = TimeSpan.FromHours(24);
        }


        public string[] DownloadFile(string uri, string destinationpath)
        {
            return DownloadFile(new Uri(uri, UriKind.RelativeOrAbsolute), destinationpath);
        }

        public string[] DownloadFile(Uri uri, string destinationpath)
        {
            return DownloadFileWhenOlderThan(uri, destinationpath, this.DefaultTTL);
        }

        public string[] DownloadFileWhenOlderThan(string uri, string destinationpath, TimeSpan ttl)
        {
            return this.DownloadFileWhenOlderThan(new Uri(uri, UriKind.RelativeOrAbsolute), destinationpath, ttl);
        }

        public string[] DownloadFileWhenOlderThan(Uri uri, string destinationpath, TimeSpan ttl)
        {
            var downloaduri = DetermineDownloadPath(uri);
            destinationpath = DetermineDestinationPath(downloaduri, destinationpath);

            if (IsFileExpired(destinationpath, ttl))
            {
                using (var w = new WebClient())
                {
                    w.CachePolicy = this.CachePolicy;
                    w.Credentials = this.Credentials;
                    w.Proxy = this.Proxy;
                    w.Headers.Add(HttpRequestHeader.UserAgent, USERAGENT);
                    w.DownloadFile(downloaduri, destinationpath);
                }
            }

            if (Path.GetExtension(destinationpath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                return UnzipFiles(destinationpath, ttl);
            return new[] { destinationpath };
        }

        private static bool IsFileExpired(string path, TimeSpan ttl)
        {
            return (!File.Exists(path) || (DateTime.UtcNow - new FileInfo(path).CreationTimeUtc) > ttl);
        }

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
                            using (var e = File.OpenWrite(dest))
                                c.Open().CopyTo(e);
                        }
                        files.Add(dest);
                    }
                }
            }
            return files.ToArray();
        }

        private Uri DetermineDownloadPath(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                return new Uri(this.BaseUri, uri.OriginalString);
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
