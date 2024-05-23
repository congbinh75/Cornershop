namespace Cornershop.Service.Infrastructure.Services;

public static class FileService
{
    public static string UploadFile(string path, string file)
    {
        var savePath = Path.Combine(path, "UploadedImages");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        var imageBytes = Convert.FromBase64String(file.Split(',')[1]);
        var filePath = Path.Combine(savePath, $"image_{Guid.NewGuid()}.jpg");
        File.WriteAllBytes(savePath, imageBytes);
        return filePath;
    }
}