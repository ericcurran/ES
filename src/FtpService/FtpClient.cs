using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FtpService
{
    public class FtpClient
    {
        private readonly string _ftpUrl;
        private readonly string _ftpLogin;
        private readonly string _ftpPassword;
        private readonly string _processedDir;
        private readonly ILogger<FtpClient> _logger;

        public FtpClient(string ftpUrl, string ftpLogin, string ftpPassword, string processedDir, ILogger<FtpClient> logger)
        {
            _ftpUrl = ftpUrl;
            _ftpLogin = ftpLogin;
            _ftpPassword = ftpPassword;
            _processedDir = processedDir;
            _logger = logger;
        }
       

        public async Task<string[]> GetFileList()
        {
            _logger.LogInformation("Getting file list from ftp....");
            try
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
                _logger.LogInformation("Connected to ftp.");
                return GetFileList(responseString);
            }
            catch (Exception e)
            {
                _logger.LogError("Error during downloading zip file list from FTP");
                _logger.LogError(e.Message);
                throw e;
            }            
        }

        public async Task<Stream> ReadFile(string fileName)
        {
            try
            {
                _logger.LogInformation($"File {fileName} downloading started.");
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{_ftpUrl}/{fileName}");
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                request.Credentials = new NetworkCredential(_ftpLogin, _ftpPassword);

                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();

                Stream responseStream = response.GetResponseStream();
                MemoryStream ms = new MemoryStream();
                await responseStream.CopyToAsync(ms);

                response.Close();
                _logger.LogInformation($"File {fileName} downloading finished successfully.");
                return ms;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error during downloading {fileName} from ftp. file is not downloaded.");
                _logger.LogError(e.Message);
                return null;
            }
        }

        public async Task MoveFileToProcessed(string fileName)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{_ftpUrl}/{fileName}");
                request.Method = WebRequestMethods.Ftp.Rename;
                request.Credentials = new NetworkCredential(_ftpLogin, _ftpPassword);
                request.RenameTo = $"{_processedDir}/{fileName}";
                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                response.Close();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error during moving {fileName} to processed folder on ftp foldrt.");
                _logger.LogError($"File {fileName} not moved to processed folder.");
                _logger.LogError(e.Message);
                
            }
        }

        private string[] GetFileList(string responseString)
        {
            string[] lines = responseString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }     
    }
}
