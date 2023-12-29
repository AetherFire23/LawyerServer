namespace ProcedureShared.FunExtensions;

public static class PathExtensions
{
	public static string CreateTempFilePath(string extension)
	{
		string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		string randomFileName = $"{Guid.NewGuid()}.{extension}";
		string path = Path.Combine(appdata, randomFileName);
		return path;
	}
}