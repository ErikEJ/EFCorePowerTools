﻿using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace EFCorePowerTools.Extensions
{
    public static class ZipArchiveExtensions
    {
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }

            DirectoryInfo di = Directory.CreateDirectory(destinationDirectoryName);
            string destinationDirectoryFullPath = di.FullName;

            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.FullName));

                if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                {
                    throw new IOException("Trying to extract file outside of destination directory. See this link for more info: https://snyk.io/research/zip-slip-vulnerability");
                }

                if (string.IsNullOrEmpty(file.Name))
                {// Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }

                RetryFileWrite(file, completeFileName);
            }
        }

        private static void RetryFileWrite(ZipArchiveEntry entry, string completeFileName)
        {
            for (int i = 1; i <= 4; ++i)
            {
                try
                {
#pragma warning disable SCS0018 // See line 25 above
                    entry.ExtractToFile(completeFileName, true);
#pragma warning restore SCS0018
                    break;
                }
                catch (IOException) when (i <= 3)
                {
                    Thread.Sleep(500);
                }
            }
        }
    }
}
