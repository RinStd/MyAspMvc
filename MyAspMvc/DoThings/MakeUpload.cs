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

    public List<MyFileInfo> ViewFilesFolders(string webRootPath)
    {
        string[] filePaths = Directory.GetFiles(Path.Combine(webRootPath, "Upload"));
        string[] folderPaths = Directory.GetDirectories(Path.Combine(webRootPath, "Upload"));
        List<MyFileInfo> fileList = new List<MyFileInfo>();
        foreach (string folderPath in folderPaths)
        {
            fileList.Add(new MyFileInfo
            {
                Name = Path.GetFileName(folderPath),
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
                Path = filePath,
                LastModify = Directory.GetCreationTime(filePath),
                IsFolder = false,
            });
        }
        return fileList;
    }
}
