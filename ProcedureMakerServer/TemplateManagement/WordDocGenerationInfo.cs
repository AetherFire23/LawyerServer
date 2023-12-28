using ProcedureMakerServer.Constants;
namespace ProcedureMakerServer.TemplateManagement;

public class WordDocGenerationInfo
{
	public readonly string RandomlyGenerated;
	public readonly string FilePath;
	private readonly Guid _filedId;
	public WordDocGenerationInfo()
	{
		_filedId = Guid.NewGuid();
		RandomlyGenerated = _filedId.ToString();
		FilePath = Path.Combine(ConstantPaths.TemporaryFilesPath, $"{RandomlyGenerated}.docx");
	}

	//public void ExecuteOnOpen(Action<WordprocessingDocument> onOpen)
	//{
	//    using(var document = WordprocessingDocument.Open(_path, true))
	//    {
	//        onOpen(document); // i would need to test that though
	//    }
	//}
}
