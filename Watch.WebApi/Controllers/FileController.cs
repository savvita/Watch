using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watch.DataAccess;
using Watch.DataAccess.UI;
using Watch.DataAccess.UI.Models;
using Watch.Domain.Models;
using Watch.Domain.Roles;
using Watch.Files;
using Watch.WebApi.Helpers;

namespace Watch.WebApi.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFileController _controller;

        public FileController(IFileController controller, WatchDbContext context, IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = new DbContext(context, userManager, roleManager);
            _configuration = configuration;
            _controller = controller;
        }


        [HttpPost("")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<Result<List<string>>> Create(IFormFileCollection uploads)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            List<string> urls = new List<string>();

            foreach(var uploadedFile in Request.Form.Files)
            {
                if (uploadedFile.Length > FileHelper.MaxSize || !FileHelper.IsImage(uploadedFile))
                {
                    continue;
                }
                string fileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;

                try
                {
                    var file = await _controller.UploadFileAsync(uploadedFile.OpenReadStream(), fileName);
                    urls.Add(file);
                }
                catch
                {
                    continue;
                }

            }

            return new Result<List<string>>
            {
                Value = urls,
                Hits = urls.Count,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpGet("file")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Get([FromQuery]string file)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            //TODO delete comments
            //var connectionString = _configuration.GetConnectionString("BlobConnection");

            //string containerName = "files";
            //var blob = new AzureBlob(connectionString!, containerName);

            //var stream = await blob.DownloadFileAsync(file);

            //if(stream == null)
            //{
            //    throw new FileNotFoundException();
            //}

            var stream = await _controller.DownloadFileAsync(file);
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

            var images = await _context.Images.GetAsync();
            var slides = await _context.Slides.GetAsync();
            var res = (await _controller.GetFileListAsync()).Where(x => images.FirstOrDefault(i => i.Value == x) == null && slides.FirstOrDefault(i => i.ImageUrl == x) == null);

            return new Result<List<string>>
            {
                Value = res != null ? res.ToList() : new List<string>(),
                Hits = res != null ? res.Count() : 0,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }

        [HttpDelete("")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Result<bool>> Delete([FromBody] List<string> files)
        {
            await _context.Users.CheckUserAsync(User.Identity);

            int count = 0;

            files.ForEach(async file =>
            {
                if (await _controller.DeleteFileAsync(Path.GetFileName(file)) == true)
                {
                    count++;
                }
            });

            return new Result<bool>
            {
                Value = files.Count == count,
                Hits = count,
                Token = await JwtHelper.GetTokenAsync(_context, User, _configuration)
            };
        }
    }
}
