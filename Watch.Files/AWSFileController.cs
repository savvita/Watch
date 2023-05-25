using Amazon.S3;
using Amazon.S3.Model;

namespace Watch.Files
{
    public class AWSFileController : IFileController
    {
        private readonly string _bucketName;
        private readonly AmazonS3Client _s3Client;
        private readonly string _rootPath;
        public AWSFileController(string accessKey, string secretKey, Amazon.RegionEndpoint region, string bucketName, string rootPath)
        {
            _bucketName = bucketName;
            _s3Client = new AmazonS3Client(accessKey, secretKey, region);
            _rootPath = rootPath;
        }
        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            try
            {
                var response = await _s3Client.DeleteObjectAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Stream?> DownloadFileAsync(string fileName)
        {
            MemoryStream? ms = null;
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            using GetObjectResponse response = await _s3Client.GetObjectAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                using (ms = new MemoryStream())
                {
                    await response.ResponseStream.CopyToAsync(ms);
                }
            }

            if (ms == null)
            {
                throw new FileNotFoundException();
            }

            return ms;     
        }

        public async Task<List<string>> GetFileListAsync()
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName
            };

            try
            {
                var response = await _s3Client.ListObjectsV2Async(request);
                return response.S3Objects.Select(x => Path.Combine(_rootPath, x.Key)).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<string> UploadFileAsync(Stream stream, string filename)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = filename,
                InputStream = stream,
                CannedACL = S3CannedACL.PublicRead
            };

            await _s3Client.PutObjectAsync(request);

            return Path.Combine(_rootPath, filename);
        }
    }
}
