using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyAspMvc.DoThings;
using MyAspMvc.Models;

namespace MyAspMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Upload file
        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            string webRootPath = this._webHostEnvironment.WebRootPath;
            MakeSomething makeSomething = new MakeSomething();
            if (!makeSomething.IsFileExists(file, webRootPath))
            {
                await makeSomething.UploadFile(file, webRootPath);
                TempData["msg"] = $"{file.FileName} successfully upload.";                
            }
            else
            {
                TempData["msg"] = $"{file.FileName} is exists in this folder.";
            }
            
            return View();
        }
        #endregion

        #region List files
        public IActionResult FileList()
        {
            string webRootPath = this._webHostEnvironment.WebRootPath;
            MakeSomething makeSomething = new MakeSomething();
            List<MyFileInfo>  fileList = makeSomething.ViewFilesFolders(webRootPath);

            return View(fileList);
        }

        public FileResult FileDownload(string fileName)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Upload", fileName);
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }
        #endregion
    }
}