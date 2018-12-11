using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StorageService
{
    public class StorageService
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudBlobClient cloudBlobClient;
        private readonly CloudBlobContainer cloudBlobContainer;

        public StorageService(string blobConnectionString, string containerName )
        {
          if(!CloudStorageAccount.TryParse(blobConnectionString, out storageAccount))
            {
                throw new StorageException("A valid Storage account conection string is not provided");
            }
            cloudBlobClient = storageAccount.CreateCloudBlobClient();
            cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
           
        }

        public async Task<bool> IsContainerExist()
        {
            return await cloudBlobContainer.ExistsAsync();
        }


        public async Task<bool> IsDirectoryExist(string directoryName)
        {
            var containerItems = await GetContainerItems();
            foreach (var item in containerItems)
            {
                if (item is CloudBlobDirectory)
                {
                    var dir = (CloudBlobDirectory)item;
                    var dirName = dir.Prefix.Substring(0, dir.Prefix.Length - 1);
                    if (dirName == directoryName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<IReadOnlyCollection<IListBlobItem>> GetContainerItems()
        {
            BlobContinuationToken blobContinuationToken = null;
            var items = new List<IListBlobItem>();
            do
            {
                var results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                items.AddRange(results.Results);               
            } while (blobContinuationToken != null);
            return items;
        }


        

        public async Task ProcessAsync()
        {
            CloudStorageAccount storageAccount = null;
            CloudBlobContainer cloudBlobContainer = null;

            string sourceFile = null;
            string destinationFile = null;

                  // Check whether the connection string can be parsed.
           
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    // Create a container called 'quickstartblobs' and append a GUID value to it to make the name unique. 
                    cloudBlobContainer = cloudBlobClient.GetContainerReference("quickstartblobs" + Guid.NewGuid().ToString());
                    await cloudBlobContainer.CreateAsync();
                    Console.WriteLine("Created container '{0}'", cloudBlobContainer.Name);
                    Console.WriteLine();

                    // Set the permissions so the blobs are public. 
                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Off
                    };
                    await cloudBlobContainer.SetPermissionsAsync(permissions);

                    // Create a file in your local MyDocuments folder to upload to a blob.
                    string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string localFileName = "QuickStart_" + Guid.NewGuid().ToString() + ".txt";
                    sourceFile = Path.Combine(localPath, localFileName);
                    // Write text to the file.
                    File.WriteAllText(sourceFile, "Hello, World!");

                    Console.WriteLine("Temp file = {0}", sourceFile);
                    Console.WriteLine("Uploading to Blob storage as blob '{0}'", localFileName);
                    Console.WriteLine();

                    // Get a reference to the blob address, then upload the file to the blob.
                    // Use the value of localFileName for the blob name.
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);
                    await cloudBlockBlob.UploadFromFileAsync(sourceFile);

                    // List the blobs in the container.
                    Console.WriteLine("Listing blobs in container.");
                    BlobContinuationToken blobContinuationToken = null;
                    do
                    {
                        var results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                        // Get the value of the continuation token returned by the listing call.
                        blobContinuationToken = results.ContinuationToken;
                        foreach (IListBlobItem item in results.Results)
                        {
                            Console.WriteLine(item.Uri);
                        }
                    } while (blobContinuationToken != null); // Loop while the continuation token is not null.
                    Console.WriteLine();

                    // Download the blob to a local file, using the reference created earlier. 
                    // Append the string "_DOWNLOADED" before the .txt extension so that you can see both files in MyDocuments.
                    destinationFile = sourceFile.Replace(".txt", "_DOWNLOADED.txt");
                    Console.WriteLine("Downloading blob to {0}", destinationFile);
                    Console.WriteLine();
                    await cloudBlockBlob.DownloadToFileAsync(destinationFile, FileMode.Create);
              
                    Console.WriteLine("Press any key to delete the sample files and example container.");
                    Console.ReadLine();
                    // Clean up resources. This includes the container and the two temp files.
                    Console.WriteLine("Deleting the container and any blobs it contains");
                    if (cloudBlobContainer != null)
                    {
                        await cloudBlobContainer.DeleteIfExistsAsync();
                    }
                    Console.WriteLine("Deleting the local source file and local downloaded files");
                    Console.WriteLine();
                    File.Delete(sourceFile);
                    File.Delete(destinationFile);                
           
        }
    }
}
