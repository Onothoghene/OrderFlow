using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Helper
{
    public class Utilities
    {
        #region Date Converter

        public static string ConvertDateToString(DateTime model)
        {
            long res = (long)(model - new DateTime(1970, 1, 1)).TotalMilliseconds;
            string _model = res.ToString();
            return _model;
        }

        public static long ConvertDateToLong(DateTime model)
        {
            long res = (long)(model - new DateTime(1970, 1, 1)).TotalMilliseconds;
            long _model = res;
            return _model;
        }

        public static DateTime ConvertToDateTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }
        public static DateTime ConvertToDateTime(string unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = long.Parse(unixTime);
            return epoch.AddMilliseconds(date);
        }

        public static string FileDirectoryUniqueName(string realName)
        {
            if (!string.IsNullOrEmpty(realName))
            {
                if (realName.Contains('.'))
                {
                    realName = RemoveExtensionFromFileName(realName);
                }
                return string.Concat(realName, "_", DateTime.UtcNow.Ticks);
            }
            return null;
        }
        #endregion

        #region File Extractors and Converters
        public static string GetFileExtensionFromFileName(string fileName)
        {
            string fileExtension = "";
            try
            {
                if (!string.IsNullOrEmpty(fileName) && fileName.Contains('.'))
                {
                    var splittedFileArray = fileName.Split('.');
                    fileExtension = splittedFileArray[splittedFileArray.Length - 1];
                }
            }
            catch (Exception)
            {
            }
            return fileExtension;
        }

        public static string RemoveExtensionFromFileName(string fileName)
        {
            string fileExtension = "";
            try
            {
                if (!string.IsNullOrEmpty(fileName) && fileName.Contains('.'))
                {
                    var splittedFileArray = fileName.Split('.');
                    for (int i = 0; i < (splittedFileArray.Length - 1); i++)
                    {
                        fileExtension += splittedFileArray[i];
                    }
                }
            }
            catch (Exception)
            {
            }
            return fileExtension;
        }

        public static string CalculateFileSize(string filePath)
        {
            string fileSizeFormat = "";
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    long length = new System.IO.FileInfo(filePath).Length;
                    Utilities utilities = new Utilities();
                    fileSizeFormat = utilities.FileSizeFormatter(length);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return fileSizeFormat;
        }

        public static string CalculateFileSize(long length)
        {
            string fileSizeFormat = "";
            try
            {
                if (length > 0)
                {
                    Utilities utilities = new Utilities();
                    fileSizeFormat = utilities.FileSizeFormatter(length);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return fileSizeFormat;
        }

        private readonly string[] FileSizeSuffixes =
                  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        private string FileSizeFormatter(long value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + FileSizeFormatter(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, FileSizeSuffixes[i]);
        }
        #endregion

        //public static string Convert()
        //{
        //    // byte[] fileBytes = getFileBytesFromDB();
        //    var tmpFile = Path.GetTempFileName();
        //    File.WriteAllBytes(tmpFile, fileBytes);
        //}
    }
}
