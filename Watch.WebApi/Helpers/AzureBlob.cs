using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Watch.WebApi.Helpers
{
    public class AzureBlob
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public AzureBlob(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            try
            {
                var serviceClient = new BlobServiceClient(_connectionString);
                var containerClient = serviceClient.GetBlobContainerClient(_containerName);

                if(containerClient == null)
                {
                    return false;
                }

                var obj = Path.GetFileName(fileName);

                var blobClient = containerClient.GetBlobClient(obj);
                return await blobClient.DeleteIfExistsAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            try
            {
                var serviceClient = new BlobServiceClient(_connectionString);
                var containerClient = serviceClient.GetBlobContainerClient(_containerName);

                string fileName = Guid.NewGuid().ToString() + "_" + file.FileName.Trim();

                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(file.OpenReadStream(), true);
                return Path.Combine(ConfigurationManager.FileRoot, fileName);
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<Stream?> DownloadFileAsync(string fileName)
        {
            try
            {
                var serviceClient = new BlobServiceClient(_connectionString);
                var containerClient = serviceClient.GetBlobContainerClient(_containerName);

                var blobClient = containerClient.GetBlobClient(fileName);

                Stream? result = new MemoryStream();
                await blobClient.DownloadToAsync(result);

                return result;

            }
            catch
            {
                return null;
            }
        }

        public async Task<List<string>> GetBlobListAsync()
        {
            // Get Reference to Blob Container
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(_containerName);

            // Fetch info about files in the container
            // Note: Loop with BlobContinuationToken to fetch results in pages. Pass null as currentToken to fetch all results.
            BlobResultSegment resultSegment = await container.ListBlobsSegmentedAsync(currentToken: null);
            IEnumerable<IListBlobItem> blobItems = resultSegment.Results;

            // Extract the URI of the files into a new list
            List<string> fileUris = new List<string>();
            foreach (var blobItem in blobItems)
            {
                fileUris.Add(blobItem.StorageUri.PrimaryUri.ToString());
            }
            return fileUris;
        }
    }
}
