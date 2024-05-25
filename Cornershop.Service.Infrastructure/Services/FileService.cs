namespace Cornershop.Service.Infrastructure.Services;

public static class FileService
{
    public static string UploadImageFile(string path, string file)
    {
        var typeString = file.Split(',')[0];
        var type = typeString.Contains("png") ? ".png" : ".jpg";
        var imageBytes = Convert.FromBase64String(file.Split(',')[1]);
        var filePath = Path.Combine(path, Guid.NewGuid() + type);
        File.WriteAllBytes(filePath, imageBytes);
        return filePath;
    }
}