using Microsoft.AspNetCore.Hosting;
using System.Diagnostics.Metrics;

namespace ArshaWebApp.Extensions;
public static class  FileExtension
{
    public static bool CheckType(this IFormFile file,string type)
    {
        return file.ContentType.Contains(type);
    }
    public static bool CheckSize( this IFormFile file, double kb)
    {
        return file.Length /1024 > kb;
    }
    public static async Task<string> UploadAsync(this IFormFile file, params string[] folders)
    {
        string newFileName = Guid.NewGuid().ToString() + file.FileName;
        string pathFolder = Path.Combine(folders);
        string path=Path.Combine(pathFolder, newFileName);

        using (FileStream fileStream = new FileStream(path, FileMode.CreateNew))
        {
            await file.CopyToAsync(fileStream);
        }
        return newFileName;
    }
}
