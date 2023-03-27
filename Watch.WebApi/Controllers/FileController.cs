using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;
using Watch.WebApi.Helpers;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;

        public FileController(WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
        }


        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<string>>> Create(IFormFileCollection uploads)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            List<string> urls = new List<string>();

            ////===local
            //if (!Directory.Exists(ConfigurationManager.FileRoot))
            //{
            //    Directory.CreateDirectory(ConfigurationManager.FileRoot);
            //}

            //foreach (var uploadedFile in Request.Form.Files)
            //{
            //    string fileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;

            //    string path = Path.Combine(ConfigurationManager.FileRoot, fileName);
            //    using (var fileStream = new FileStream(path, FileMode.Create))
            //    {
            //        await uploadedFile.CopyToAsync(fileStream);
            //    }
            //    urls.Add(Path.Combine(ConfigurationManager.FileRoot, fileName));
            //}
            ////====

            ////Azure + поменять путь ConfigurationManager.FileRoot
            //var connectionString = _configuration.GetConnectionString("BlobConnection");

            //string containerName = "files";
            //var serviceClient = new BlobServiceClient(connectionString);
            //var containerClient = serviceClient.GetBlobContainerClient(containerName);

            //foreach (var uploadedFile in Request.Form.Files)
            //{
            //    if(uploadedFile.Length > 100000 || !FileHelper.IsImage(uploadedFile))
            //    {
            //        continue;
            //    }
            //    string fileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;

            //    var blobClient = containerClient.GetBlobClient(fileName);
            //    await blobClient.UploadAsync(uploadedFile.OpenReadStream(), true);
            //    urls.Add(Path.Combine(ConfigurationManager.FileRoot, fileName));
            //}

            var connectionString = _configuration.GetConnectionString("BlobConnection");

            string containerName = "files";

            var blob = new AzureBlob(connectionString!, containerName);

            foreach (var uploadedFile in Request.Form.Files)
            {
                if (uploadedFile.Length > 100000 || !FileHelper.IsImage(uploadedFile))
                {
                    continue;
                }
                urls.Add(await blob.UploadFileAsync(uploadedFile));
            }

            return new Result<List<string>>
            {
                Value = urls,
                Hits = urls.Count,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpGet("file")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Get([FromQuery]string file)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var connectionString = _configuration.GetConnectionString("BlobConnection");

            string containerName = "files";
            var blob = new AzureBlob(connectionString!, containerName);

            var stream = await blob.DownloadFileAsync(file);

            if(stream == null)
            {
                throw new FileNotFoundException();
            }

            return File(stream, "application/octet-stream", file);
        }

        [HttpGet("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<List<string>>> Get()
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var connectionString = _configuration.GetConnectionString("BlobConnection");

            string containerName = "files";
            var blob = new AzureBlob(connectionString!, containerName);

            var images = await _context.Images.GetAsync();

            var res = (await blob.GetBlobListAsync()).Where(x => images.FirstOrDefault(i => i.Value == x) == null);

            return new Result<List<string>>
            {
                Value = res != null ? res.ToList() : new List<string>(),
                Hits = res != null ? res.Count() : 0,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

        [HttpDelete("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<bool>> Delete([FromBody] List<string> files)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            var connectionString = _configuration.GetConnectionString("BlobConnection");

            string containerName = "files";
            var blob = new AzureBlob(connectionString!, containerName);

            int count = 0;

            files.ForEach(async file =>
            {
                if (await blob.DeleteFileAsync(file) == true)
                {
                    count++;
                }
            });

            return new Result<bool>
            {
                Value = files.Count == count,
                Hits = count,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }
    }
}
