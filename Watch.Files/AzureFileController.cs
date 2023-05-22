using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Watch.Files
{
    public class AzureFileController : IFileController
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly string _rootPath;

        public AzureFileController(string connectionString, string containerName, string rootPath)
        {
            _connectionString = connectionString;
            _containerName = containerName;
            _rootPath = rootPath;
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            try
            {
                var serviceClient = new BlobServiceClient(_connectionString);
                var containerClient = serviceClient.GetBlobContainerClient(_containerName);

                if (containerClient == null)
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

        public async Task<List<string>> GetFileListAsync()
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

        public async Task<string> UploadFileAsync(Stream stream, string filename)
        {
            try
            {
                var serviceClient = new BlobServiceClient(_connectionString);
                var containerClient = serviceClient.GetBlobContainerClient(_containerName);

                var blobClient = containerClient.GetBlobClient(filename);
                await blobClient.UploadAsync(stream, true);
                return Path.Combine(this._rootPath, filename);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
