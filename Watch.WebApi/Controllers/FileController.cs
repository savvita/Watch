using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;

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
            List<string> urls = new List<string>();

            //===local
            if (!Directory.Exists(ConfigurationManager.FileRoot))
            {
                Directory.CreateDirectory(ConfigurationManager.FileRoot);
            }

            foreach (var uploadedFile in Request.Form.Files)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;

                string path = Path.Combine(ConfigurationManager.FileRoot, fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                urls.Add(Path.Combine(ConfigurationManager.FileRoot, fileName));
            }
            //====

            ////Azure + поменять путь ConfigurationManager.FileRoot

            //var connectionString = "DefaultEndpointsProtocol=https;AccountName=savvita;AccountKey=7bwdykfVsPLQqhNoU6m2oyrxMVlqJo2nkXMy/7pilTvhxqSas4Tg4iglXzDYVByzfj9RJcMcXWht+AStW0+lWw==;EndpointSuffix=core.windows.net";
            //string containerName = "files";
            //var serviceClient = new BlobServiceClient(connectionString);
            //var containerClient = serviceClient.GetBlobContainerClient(containerName);

            //foreach (var uploadedFile in Request.Form.Files)
            //{
            //    string fileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;

            //    var blobClient = containerClient.GetBlobClient(fileName);
            //    await blobClient.UploadAsync(uploadedFile.OpenReadStream(), true); 
            //    urls.Add(Path.Combine(ConfigurationManager.FileRoot, fileName));
            //}

            return new Result<List<string>>
            {
                Value = urls,
                Hits = 20,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtHelper.GetToken(User.Claims, _configuration))
            };
        }

    }
}
