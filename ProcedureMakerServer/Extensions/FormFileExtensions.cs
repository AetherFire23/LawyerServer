namespace ProcedureMakerServer.Extensions;

public static class FormFileExtensions
{
    /// <summary> Do not include the dot </summary>
    public static bool IsFileOfType(this IFormFile formFile, string fileExtension)
    {
        int indexOfFileExtensionBegin = formFile.ContentDisposition.IndexOf('.');

        var formExtension = new string(formFile.ContentDisposition
            .SkipWhile(x => x != '.')
            .ToArray());

        bool isContained = formExtension.Contains(fileExtension);
        return isContained;
    }

    public static async Task CreateFileTo(this IFormFile formFile, string filePath)
    {
        bool directoryExists = Path.Exists(Path.GetDirectoryName(filePath));
        if (!directoryExists) throw new Exception("Directory dont exist lol!");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await formFile.CopyToAsync(stream);
        }
    }
}
