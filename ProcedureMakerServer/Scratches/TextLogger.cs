namespace ProcedureMakerServer.Scratches;

public class TextLogger
{
	public static void LogToFile(string text)
	{
		File.WriteAllText("log.txt", $"{Environment.NewLine}");
		File.WriteAllText("log.txt", text);
	}
}
