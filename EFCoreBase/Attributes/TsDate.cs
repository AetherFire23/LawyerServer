using Reinforced.Typings.Attributes;

namespace EFCoreBase.Attributes;
public class TsDate : TsPropertyAttribute
{
    public TsDate()
    {
        this.Type = "Date";
    }
}
