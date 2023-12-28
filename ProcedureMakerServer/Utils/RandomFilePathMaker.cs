using ProcedureMakerServer.Constants;

namespace ProcedureMakerServer.Utils;

public class RandomFilePathMaker
{
	// Do not include . in extension
	public static string GenerateRandomFilePath(string extension)
	{
		if (extension.Contains('.')) throw new Exception();

		string randomFilePath = $@"{ConstantPaths.TemporaryFilesPath}{Guid.NewGuid()}.{extension}";

		return randomFilePath;
	}
}
