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

        #region Default
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****

        #region Cerate folder
        [HttpPost]
        public IActionResult FolderCreateInFolder(string nameFolder, string currentFolder)
        {
            MakeSomething makeSomething = new MakeSomething();
            if (!makeSomething.IsFolderExists(nameFolder, currentFolder))
            {
                makeSomething.FolderCreate(nameFolder, currentFolder);
                TempData["msg"] = $"{nameFolder} successfully create.";
            }
            else
            {
                TempData["msg"] = $"{nameFolder} is exists in this folder.";
            }

            return RedirectToAction("FileListInFolder", "Home", new { currentFolder = currentFolder });
        }
        #endregion // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****

        #region Upload files
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

        [HttpPost]
        public async Task<IActionResult> FileUploadInFolder(IFormFile file, string currentFolder)
        {
            MakeSomething makeSomething = new MakeSomething();
            if (!makeSomething.IsFileExistsInFolder(file, currentFolder))
            {
                await makeSomething.UploadFileInFolder(file, currentFolder);
                TempData["msg"] = $"{file.FileName} successfully upload.";
            }
            else
            {
                TempData["msg"] = $"{file.FileName} is exists in this folder.";
            }

            return RedirectToAction("FileListInFolder", "Home", new { currentFolder = currentFolder });
        }
        #endregion // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****

        #region View list files
        public IActionResult FileList()
        {
            string webRootPath = this._webHostEnvironment.WebRootPath;
            MakeSomething makeSomething = new MakeSomething();
            List<MyFileInfo>  fileList = makeSomething.ViewFiles(webRootPath);

            return View(fileList);
        }

        public IActionResult FileListInFolder(string currentFolder)
        {
            MakeSomething makeSomething = new MakeSomething();
            List<MyFileInfo> fileList = makeSomething.ViewFilesInFolder(currentFolder);
            
            TempData["adress"] = currentFolder;

            return View(fileList);
        }

        public IActionResult ViewLastModifyFilesInAllFolders()
        {
            string webRootPath = this._webHostEnvironment.WebRootPath;
            ViewLastModifyFiles viewLastModifyFiles = new ViewLastModifyFiles();
            List<MyFileInfo> fileList = viewLastModifyFiles.ViewFiles(Path.Combine(webRootPath, "Upload"));
            return View(fileList);
        }
        #endregion // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****

        #region Download files
        public FileResult FileDownload(string fileName)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Upload", fileName);
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }

        public FileResult FileDownloadInFolder(string fileName, string currentFolder)
        {
            string path = Path.Combine(currentFolder, fileName);
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", fileName);
        }
        #endregion // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****

        #region Delete files
        public IActionResult FileDeleteInFolder(string fileName, string currentFolder)
        {
            System.IO.File.Delete(Path.Combine(currentFolder, fileName));

            return RedirectToAction("FileListInFolder", "Home", new { currentFolder = currentFolder });
        }
        #endregion // ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** *****
    }
}