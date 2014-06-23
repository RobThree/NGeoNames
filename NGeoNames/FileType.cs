using System.IO;
namespace NGeoNames
{
    public enum FileType
    {
        AutoDetect = 0,
        Plain = 1,
        GZip = 2
    }

    internal static class FileUtil
    {
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
