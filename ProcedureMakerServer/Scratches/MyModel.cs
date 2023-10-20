using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Scratches;

[TsClass]
public class MyModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public MyOtherClass Caca { get; set; }
}
