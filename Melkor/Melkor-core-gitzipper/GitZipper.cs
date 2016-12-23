using Ionic.Zip;
using System;
using System.IO;
using System.Net;

namespace Melkor_core_gitzipper
{
    /// <summary>
    ///     <p>Autor : Kucis</p>
    /// </summary>
    public class GitZipper
    {
        private readonly string _extension = "/archive/master.zip";
        private readonly string _url;
        private string _directoryLocation;
        private string _downloadFile;
        private readonly string _userGuid;

        public GitZipper(string url, string userGuid)
        {
            this._url = url;
            this._userGuid = userGuid;
        }

        public void GitDownload(string directoryLocation)
        {
            _directoryLocation = $@"{directoryLocation}\{_userGuid}\";
            _downloadFile = $@"{_directoryLocation}\{_userGuid}master.zip";
            
            Directory.CreateDirectory(_directoryLocation);
           
            string downloadLink = _url + _extension;
            using (WebClient client = new WebClient())
            {
                Console.WriteLine($"Downloading {downloadLink}");
                client.DownloadFile(new Uri(downloadLink), _downloadFile);
            }
        }

        public static void CleanUp(string directoryLocation)
        {
            string[] files = Directory.GetFiles(directoryLocation);
            string[] dirs = Directory.GetDirectories(directoryLocation);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                CleanUp(dir);
            }

            Directory.Delete(directoryLocation, false);
        }

        /// <summary>
        /// <p>Unzips the downloaded zip</p>
        /// Edited (14.12.2016) - @M3talen : Azure support.
        /// </summary>
        public void GitUnzip()
        {
            Console.WriteLine($"Unziping {_downloadFile} to {_directoryLocation}");
            using (ZipFile zip = ZipFile.Read(_downloadFile))
            {
                zip.ExtractAll(_directoryLocation, ExtractExistingFileAction.OverwriteSilently);
            }

            File.Delete(_downloadFile);
        }

    }
}
