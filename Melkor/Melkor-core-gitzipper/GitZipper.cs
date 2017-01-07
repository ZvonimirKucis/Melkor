using System;
using System.IO;
using System.Net;
using Ionic.Zip;

namespace Melkor_core_gitzipper
{
    /// <summary>
    ///     <p>Autor : Kucis</p>
    /// </summary>
    public class GitZipper
    {
        private readonly string _extension = "/archive/master.zip";
        private readonly string _url;
        private readonly string _userGuid;
        private string _directoryLocation;
        private string _downloadFile;

        public GitZipper(string url, string userGuid)
        {
            _url = url;
            _userGuid = userGuid;
        }

        public void GitDownload(string directoryLocation)
        {
            _directoryLocation = $@"{directoryLocation}\{_userGuid}\";
            _downloadFile = $@"{_directoryLocation}\{_userGuid}master.zip";

            Directory.CreateDirectory(_directoryLocation);

            var downloadLink = _url + _extension;
            using (var client = new WebClient())
            {
                Console.WriteLine($"Downloading {downloadLink}");
                client.DownloadFile(new Uri(downloadLink), _downloadFile);
            }
        }

        public static void CleanUp(string directoryLocation)
        {
            var files = Directory.GetFiles(directoryLocation);
            var dirs = Directory.GetDirectories(directoryLocation);

            foreach (var file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (var dir in dirs)
                CleanUp(dir);

            Directory.Delete(directoryLocation);
        }

        /// <summary>
        ///     <p>Unzips the downloaded zip</p>
        ///     Edited (14.12.2016) - @M3talen : Azure support.
        /// </summary>
        public void GitUnzip()
        {
            Console.WriteLine($"Unziping {_downloadFile} to {_directoryLocation}");
            using (var zip = ZipFile.Read(_downloadFile))
            {
                zip.ExtractAll(_directoryLocation, ExtractExistingFileAction.OverwriteSilently);
            }

            File.Delete(_downloadFile);
        }
    }
}