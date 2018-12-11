using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FtpService
{
    public class FtpService
    {
        private readonly string _ftpUrl;
        private readonly string _ftpLogin;
        private readonly string _ftpPassword;

        public FtpService(string ftpUrl, string ftpLogin, string ftpPassword)
        {
            CheckaArguments(ftpUrl, ftpLogin, ftpPassword);
            _ftpUrl = ftpUrl;
            _ftpLogin = ftpLogin;
            _ftpPassword = ftpPassword;
        }

        public async Task<string[]> GetFileList()
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

        public async Task<Stream> ReadFile(string fileName)
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

        private string[] GetFileList(string responseString)
        {
            string[] lines = responseString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }

        private void CheckaArguments(string ftpUrl, string ftpLogin, string ftpPassword)
        {
            if (string.IsNullOrEmpty(ftpUrl))
            {
                throw new ArgumentException("Argument is null or empty", nameof(ftpUrl));
            }

            if (string.IsNullOrEmpty(ftpLogin))
            {
                throw new ArgumentException("Argument is null or empty", nameof(ftpLogin));
            }

            if (string.IsNullOrEmpty(ftpPassword))
            {
                throw new ArgumentException("Argument is null or empty", nameof(ftpPassword));
            }
        }
    }
}
