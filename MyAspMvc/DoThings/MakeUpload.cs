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
                isFileExists = File.Exists(Path.Combine(webRootPath, "Upload", file.FileName));
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
                LastModify = Directory.GetLastWriteTime(folderPath),
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
                LastModify = Directory.GetLastWriteTime(filePath),
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
                LastModify = Directory.GetLastWriteTime(folderPath),
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
                LastModify = Directory.GetLastWriteTime(filePath),
                IsFolder = false,
            });
        }
        return fileList;
    }
}

public class ViewLastModifyFiles
{
    public List<MyFileInfo> fileList = new List<MyFileInfo>();

    public List<MyFileInfo> ViewFiles(string webRootPath)
    {
        ListFiles(webRootPath);
        SortFileList(fileList);
        fileList.RemoveRange(10, (fileList.Count - 11));
        return fileList;
    }

    public void ListFiles(string webRootPath)
    {
        string[] filePaths = Directory.GetFiles(webRootPath);
        foreach (string filePath in filePaths)
        {
            fileList.Add(new MyFileInfo
            {
                Name = Path.GetFileName(filePath),
                ParentPath = webRootPath,
                Path = filePath,
                LastModify = Directory.GetLastWriteTime(filePath),
                IsFolder = false,
            });
        }
        ListDirectory(webRootPath);
    }

    public void ListDirectory(string webRootPath) // Если есть в директории папки.
    {
        string[] subDirectoryFolders = Directory.GetDirectories(webRootPath);
        foreach (string subDirectory in subDirectoryFolders)
        {
            ListFiles(subDirectory);
        }
    }

    public void SortFileList(List<MyFileInfo> fileList)
    {
        List<MyFileInfo> tmpList = new List<MyFileInfo>();
        for (int i = 0; i < fileList.Count; i++)
        {
            for (int j = i + 1; j < fileList.Count; j++)
            {
                if (fileList[i].LastModify < fileList[j].LastModify)
                {
                    tmpList.Add(fileList[i]);
                    fileList[i] = fileList[j];
                    fileList[j] = tmpList[0];
                    tmpList.Clear();
                }
            }
        }
    }
}