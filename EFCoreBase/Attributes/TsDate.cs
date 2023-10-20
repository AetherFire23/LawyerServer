using Reinforced.Typings.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreBase.Attributes;
public class TsDate : TsPropertyAttribute
{
    public TsDate()
    {
        this.Type = "Date";
    }
}
