using MyAspMvc.Models;

namespace MyAspMvc.DoThings;

public class MakeSomething
{
    public async Task<bool> UploadFile(IFormFile file, string webRootPath)
    {
        bool isUploaded = false;
        string? path = null;
        
        try
        {
            if (file != null)
            {
                path = Path.GetFullPath(Path.Combine(webRootPath, "Upload"));
                using (var filestream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(filestream);
                }
                isUploaded = true;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return isUploaded;
    }

    public bool IsFileExists(IFormFile file, string webRootPath)
    {
        bool isFileExists = false;
        
        try
        {
            if (file != null)
            {
                isFileExists = File.Exists(Path.Combine(webRootPath, "Upload" ,file.FileName));
            }
        }
        catch (Exception)
        {
            throw;
        }

        return isFileExists;
    }

    public List<MyFileInfo> ViewFiles(string webRootPath)
    {
        string[] filePaths = Directory.GetFiles(Path.Combine(webRootPath, "Upload"));
        string[] folderPaths = Directory.GetDirectories(Path.Combine(webRootPath, "Upload"));
        List<MyFileInfo> fileList = new List<MyFileInfo>();
        foreach (string folderPath in folderPaths)
        {
            fileList.Add(new MyFileInfo
            {
                Name = Path.GetFileName(folderPath),
                ParentPath = webRootPath,
                Path = folderPath,
                LastModify = Directory.GetCreationTime(folderPath),
                IsFolder = true,
            });
        }
        foreach (string filePath in filePaths)
        {
            fileList.Add(new MyFileInfo
            {
                Name = Path.GetFileName(filePath),
                ParentPath = webRootPath,
                Path = filePath,
                LastModify = Directory.GetCreationTime(filePath),
                IsFolder = false,
            });
        }
        return fileList;
    }

    // ***** ***** ***** ***** ***** do things in current folder ***** ***** ***** ***** *****

    public async Task<bool> UploadFileInFolder(IFormFile file, string inFolder)
    {
        bool isUploaded = false;

        try
        {
            if (file != null)
            {
                using (var filestream = new FileStream(Path.Combine(inFolder, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(filestream);
                }
                isUploaded = true;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return isUploaded;
    }

    public bool IsFileExistsInFolder(IFormFile file, string inFolder)
    {
        bool isFileExists = false;

        try
        {
            if (file != null)
            {
                isFileExists = File.Exists(Path.Combine(inFolder, file.FileName));
            }
        }
        catch (Exception)
        {
            throw;
        }

        return isFileExists;
    }

    public List<MyFileInfo> ViewFilesInFolder(string inFolder)
    {
        string[] filePaths = Directory.GetFiles(inFolder);
        string[] folderPaths = Directory.GetDirectories(inFolder);
        List<MyFileInfo> fileList = new List<MyFileInfo>();
        foreach (string folderPath in folderPaths)
        {
            fileList.Add(new MyFileInfo
            {
                Name = Path.GetFileName(folderPath),
                ParentPath = inFolder,
                Path = folderPath,
                LastModify = Directory.GetCreationTime(folderPath),
                IsFolder = true,
            });
        }
        foreach (string filePath in filePaths)
        {
            fileList.Add(new MyFileInfo
            {
                Name = Path.GetFileName(filePath),
                ParentPath = inFolder,
                Path = filePath,
                LastModify = Directory.GetCreationTime(filePath),
                IsFolder = false,
            });
        }
        return fileList;
    }
}
