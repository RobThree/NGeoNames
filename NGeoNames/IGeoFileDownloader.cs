using System;
using System.Threading.Tasks;

namespace NGeoNames
{
    public interface IGeoFileDownloader
    {
        Uri BaseUri { get; }
        TimeSpan DefaultTTL { get; }

        Task<string[]> DownloadFileAsync(string uri, string destinationpath);
        Task<string[]> DownloadFileAsync(Uri uri, string destinationpath);
        Task<string[]> DownloadFileWhenOlderThanAsync(string uri, string destinationpath, TimeSpan ttl);
        Task<string[]> DownloadFileWhenOlderThanAsync(Uri uri, string destinationpath, TimeSpan ttl);
    }
}