
namespace Watch.Files
{
    public interface IFileController
    {
        Task<bool> DeleteFileAsync(string fileName);

        Task<string> UploadFileAsync(Stream stream, string filename);

        Task<Stream?> DownloadFileAsync(string fileName);

        Task<List<string>> GetFileListAsync();
    }
}
