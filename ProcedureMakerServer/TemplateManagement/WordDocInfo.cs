using ProcedureMakerServer.Constants;

namespace ProcedureMakerServer.TemplateManagement;

public class WordDocInfo
{
    public readonly string FileName;
    public readonly string FilePath;

    private readonly Guid _filedId;
    public WordDocInfo()
    {
        _filedId = Guid.NewGuid();
        FileName = _filedId.ToString();
        FilePath = Path.Combine(ConstantPaths.TemporaryFilesPath, $"{FileName}.docx");
    }

    //public void ExecuteOnOpen(Action<WordprocessingDocument> onOpen)
    //{
    //    using(var document = WordprocessingDocument.Open(_path, true))
    //    {
    //        onOpen(document); // i would need to test that though
    //    }
    //}
}
