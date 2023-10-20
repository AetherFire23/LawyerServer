using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Attributes;

public class TsGuid : TsPropertyAttribute
{
    public TsGuid()
    {
        this.StrongType = typeof(string);
    }
}
