﻿using System;

namespace Carbon.Media
{
    public static class FileFormat
    {
        public static string Normalize(string format)
        {
            if (format is null)
                throw new ArgumentNullException(nameof(format));

            if (format.Length == 0)
                throw new ArgumentException("Must not be empty", nameof(format));

            if (format[0] == '.')
            {
                format = format.Substring(1);
            }

            // Ensure the format is in lowercase
            // NOTE: This does not allocate (if already lowercase) on .NET CORE 2.1
            format = format.ToLower();
            
            switch (format)
            {
                case "aif"  : return "aiff";
                case "fpix" : return "fpx";
                case "jpe"  : return "jpeg";
                case "jpg"  : return "jpeg";
                case "tif"  : return "tiff";
                case "mpg"  : return "mpeg";
                case "wave" : return "wav";

                default     : return format;
            }
        }

        public static string FromPath(string path)
        {
            int lastDotIndex = path.LastIndexOf('.');

            return Normalize(path.Substring(lastDotIndex + 1));
        }
    }
}