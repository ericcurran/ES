using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FtpService
{
    public static class FtpMonitoringService
    {
        private static readonly string _ftpUrl       = Environment.GetEnvironmentVariable("FtpUrl");
        private static readonly string _ftpLogin     = Environment.GetEnvironmentVariable("FtpLogin");
        private static readonly string _ftpPassword  = Environment.GetEnvironmentVariable("FtpPasword");
        private static readonly string _processedDir = Environment.GetEnvironmentVariable("ProcessedDir");

        public static async Task<string[]> GetFileList()
        {
         
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpUrl);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(_ftpLogin, _ftpPassword);

            FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();

            Stream responseStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(responseStream);

            string responseString = await reader.ReadToEndAsync();

            reader.Close();
            response.Close();

            return GetFileList(responseString);
        }

        public static async Task<Stream> ReadFile(string fileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{_ftpUrl}/{fileName}");
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            request.Credentials = new NetworkCredential(_ftpLogin, _ftpPassword);

            FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();

            Stream responseStream = response.GetResponseStream();
            MemoryStream ms = new MemoryStream();
            await responseStream.CopyToAsync(ms);

            response.Close();

            return ms;
        }

        public static async Task MoveFileToProcessed(string fileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{_ftpUrl}/{fileName}");
            request.Method = WebRequestMethods.Ftp.Rename;
            request.Credentials = new NetworkCredential(_ftpLogin, _ftpPassword);
            request.RenameTo = $"{_processedDir}/{fileName}";
            FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
            response.Close();
        }

        private static string[] GetFileList(string responseString)
        {
            string[] lines = responseString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }     
    }
}
