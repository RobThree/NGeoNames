using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NGeoNames
{
    /// <summary>
    /// Provides methods to download files from geonames.org.
    /// </summary>
    /// <remarks>
    /// This class is, essentially, a small helper with some (specific) functionality for handling GeoNames files. When
    /// downloading ZIP files from geonames.org it will automatically extract the archives. It also handles (in a very
    /// simple manner) caching of downloaded files to prevent downloading files more than necessary.
    /// </remarks>
    public abstract class GeoFileDownloader : IGeoFileDownloader
    {
        /// <summary>
        /// Gets/sets the default 'Time To Live'; specifying how long already downloaded files are deemed 'valid' and
        /// won't require actually downloading again.
        /// </summary>
        public TimeSpan DefaultTTL { get; private set; }

        /// <summary>
        /// Gets the base URI to use when downloading files and relative paths are specified.
        /// </summary>
        public Uri BaseUri => _httpclient.BaseAddress;

        private readonly IGeoNamesClient _httpclient;

        /// <summary>
        /// Initializes a new instance of the GeoFileDownloader class.
        /// </summary>
        /// <param name="httpClient">The <see cref="IGeoNamesClient"/> to use for handling the downloads.</param>
        public GeoFileDownloader(IGeoNamesClient httpClient)
            : this(httpClient, TimeSpan.FromHours(24)) { }

        /// <summary>
        /// Initializes a new instance of the GeoFileDownloader class using the specified <see cref="DefaultTTL"/>.
        /// </summary>
        /// <param name="httpClient">The <see cref="IGeoNamesClient"/> to use for handling the downloads.</param>
        public GeoFileDownloader(IGeoNamesClient httpClient, TimeSpan defaultTTL)
        {
            _httpclient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            DefaultTTL = defaultTTL;
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
        public Task<string[]> DownloadFileAsync(string uri, string destinationpath)
        {
            return DownloadFileAsync(new Uri(uri, UriKind.RelativeOrAbsolute), destinationpath);
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
        public Task<string[]> DownloadFileAsync(Uri uri, string destinationpath)
        {
            return DownloadFileWhenOlderThanAsync(uri, destinationpath, DefaultTTL);
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
        public Task<string[]> DownloadFileWhenOlderThanAsync(string uri, string destinationpath, TimeSpan ttl)
        {
            return DownloadFileWhenOlderThanAsync(new Uri(uri, UriKind.RelativeOrAbsolute), destinationpath, ttl);
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
        public async Task<string[]> DownloadFileWhenOlderThanAsync(Uri uri, string destinationpath, TimeSpan ttl)
        {
            var downloaduri = DetermineDownloadPath(uri);
            destinationpath = DetermineDestinationPath(downloaduri, destinationpath);

            if (IsFileExpired(destinationpath, ttl))
                await _httpclient.DownloadFileAsync(downloaduri, destinationpath).ConfigureAwait(false);

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
                return new Uri(_httpclient.BaseAddress, uri.OriginalString);
            return uri;
        }

        private static string DetermineDestinationPath(Uri uri, string path)
        {
            if (Directory.Exists(path))
                path = Path.Combine(path, Path.GetFileName(uri.AbsolutePath));
            return path;
        }
    }

    public interface IGeoNamesClient
    {
        Uri BaseAddress { get; }
        Task DownloadFileAsync(Uri downloadUri, string destinationPath);
    }

    public abstract class GeoNamesClient : IGeoNamesClient
    {
        /// <summary>
        /// Gets the useragent string used to identify when downloading files from geonames.org.
        /// </summary>
        public static readonly string USERAGENT = $"{typeof(GeoNamesClient).Assembly.GetName().Name} v{typeof(GeoNamesClient).Assembly.GetName().Version}";

        public Uri BaseAddress => _client.BaseAddress;

        private readonly HttpClient _client;

        public GeoNamesClient(HttpClient client, Uri baseAddress)
        {
            _client = client ?? throw new ArgumentNullException(nameof(baseAddress));
            _client.BaseAddress = baseAddress;
            _client.DefaultRequestHeaders.Add("User-Agent", USERAGENT);
        }

        public async Task DownloadFileAsync(Uri downloadUri, string destinationPath)
        {
            using (var response = await _client.GetAsync(downloadUri).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                using (var fs = new FileStream(destinationPath, FileMode.CreateNew))
                    await response.Content.CopyToAsync(fs).ConfigureAwait(false);
            }
        }
    }


    public static class ExtensionMethods
    {
        private class GeoNamesHttpClientBuilder : IHttpClientBuilder
        {
            private static readonly string NAME = typeof(GeoNamesHttpClientBuilder).Assembly.GetName().Name;
            public string Name => NAME;

            public IServiceCollection Services { get; }

            public GeoNamesHttpClientBuilder(IServiceCollection services)
            {
                Services = services ?? throw new ArgumentNullException(nameof(services));
            }
        }

        public static IServiceCollection AddGeoNames(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddHttpClient();
            var defaultHttpClientBuilder = new GeoNamesHttpClientBuilder(services);
            defaultHttpClientBuilder.AddTypedClient<IGeoNamesGeoClient, GeoNamesGeoClient>();
            defaultHttpClientBuilder.AddTypedClient<IGeoNamesPostalClient, GeoNamesPostalClient>();
            services.TryAdd(ServiceDescriptor.Transient(typeof(IGeoFileGeoDownloader), typeof(GeoFileGeoDownloader)));
            services.TryAdd(ServiceDescriptor.Transient(typeof(IGeoFilePostalDownloader), typeof(GeoFilePostalDownloader)));

            return services;
        }

        private static IHttpClientBuilder AddTypedClient<TClient, TImplementation>(this IHttpClientBuilder builder)
            where TClient : class
            where TImplementation : class, TClient
        {
            builder.Services.AddTransient(delegate (IServiceProvider s)
            {
                var httpClient = (HttpClient)s.GetRequiredService<IHttpClientFactory>().CreateClient(builder.Name);
                return (TClient)s.GetRequiredService<ITypedHttpClientFactory<TImplementation>>().CreateClient(httpClient);
            });
            return builder;
        }
    }

    public interface IGeoFileGeoDownloader : IGeoFileDownloader { }

    public class GeoFileGeoDownloader : GeoFileDownloader, IGeoFileGeoDownloader
    {
        public GeoFileGeoDownloader(IGeoNamesGeoClient client)
            : base(client) { }
    }

    public interface IGeoNamesGeoClient : IGeoNamesClient { }

    public class GeoNamesGeoClient : GeoNamesClient, IGeoNamesGeoClient
    {
        public static readonly Uri DEFAULTURI = new Uri("http://download.geonames.org/export/dump/", UriKind.Absolute);

        public GeoNamesGeoClient(HttpClient client)
            : base(client, DEFAULTURI) { }
    }

    public interface IGeoFilePostalDownloader : IGeoFileDownloader { }

    public class GeoFilePostalDownloader : GeoFileDownloader, IGeoFilePostalDownloader
    {
        public GeoFilePostalDownloader(IGeoNamesPostalClient client)
            : base(client) { }
    }

    public interface IGeoNamesPostalClient : IGeoNamesClient { }

    public class GeoNamesPostalClient : GeoNamesClient, IGeoNamesPostalClient
    {
        public static readonly Uri DEFAULTURI = new Uri("http://download.geonames.org/export/zip/", UriKind.Absolute);

        public GeoNamesPostalClient(HttpClient client)
            : base(client, DEFAULTURI) { }
    }
}