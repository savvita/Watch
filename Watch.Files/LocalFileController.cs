namespace Watch.Files
{
    public class LocalFileController : IFileController
    {
        private readonly string _rootPath;
        private readonly string _sitePath;

        public LocalFileController(string rootPath, string sitePath)
        {
            _rootPath = rootPath;
            _sitePath = sitePath;
        }
        public async Task<bool> DeleteFileAsync(string fileName)
        {
            string path = Path.Combine(_rootPath, fileName);
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<Stream?> DownloadFileAsync(string fileName)
        {
            string path = Path.Combine(_rootPath, fileName);
            if(File.Exists(path))
            {
                return File.OpenRead(path);
            }

            return null;
        }

        public async Task<List<string>> GetFileListAsync()
        {
            var files = new List<string>();
            if (Directory.Exists(_rootPath))
            {
                files.AddRange(Directory.GetFiles(_rootPath).Select(x => Path.Combine(_sitePath, Path.GetFileName(x))));
            }


            return files;
        }

        public async Task<string> UploadFileAsync(Stream stream, string filename)
        {
            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }


            string path = Path.Combine(_rootPath, filename);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }

            return Path.Combine(_sitePath, filename);
        }
    }
}
