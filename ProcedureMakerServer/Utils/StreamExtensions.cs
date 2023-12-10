namespace ProcedureMakerServer.Utils;

public static class StreamExtensions
{
    public static MemoryStream CreateReadonlyMemoryStreamFromFile(string path)
    {
        byte[] file = File.ReadAllBytes(path);
        MemoryStream memoryStream = new MemoryStream(file, false);

        return memoryStream;
    }
}

public static class FileExtensions
{
    public static void CreateFileFromString(string content)
    {
        // is xml unicode or ascii
        //byte[] bytes = Encoding.U.GetBytes(content);
    }
}
