using System.IO;
namespace NGeoNames
{
    /// <summary>
    /// Specifies constants that define known (or 'supported') file types.
    /// </summary>
    public enum FileType
    {
        /// <summary>Auto detect the file type; uses the file's extension to determine filetype.</summary>
        AutoDetect = 0,
        /// <summary>The file is a plain textfile.</summary>
        Plain = 1,
        /// <summary>The file is a GZipped textfile.</summary>
        GZip = 2
    }

    /// <summary>
    /// Internal utility class.
    /// </summary>
    internal static class FileUtil
    {
        /// <summary>
        /// Returns the <see cref="FileType"/> of a file by looking at the file-extension.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>The <see cref="FileType"/> of the file</returns>
        public static FileType GetFileTypeFromExtension(string path)
        {
            var fi = new FileInfo(path);
            switch (fi.Extension.ToLowerInvariant())
            {
                case ".txt":
                    return FileType.Plain;
                case ".gz":
                    return FileType.GZip;
            }
            throw new System.NotSupportedException("Unable to detect filetype from file extension");
        }
    }
}
