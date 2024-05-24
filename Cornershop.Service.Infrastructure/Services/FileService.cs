namespace Cornershop.Service.Infrastructure.Services;

public static class FileService
{
    public static string UploadImageFile(string path, string file)
    {
        var savePath = Path.Combine(path, "UploadedImages");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        
        var typeString = file.Split(',')[0];
        var type = typeString.Contains("png") ? ".png" : ".jpg";
        var imageBytes = Convert.FromBase64String(file.Split(',')[1]);
        var filePath = Path.Combine(savePath, $"image_{Guid.NewGuid()}" + type);
        File.WriteAllBytes(savePath, imageBytes);
        return filePath;
    }
}