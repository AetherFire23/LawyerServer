using ProcedureMakerServer.Enums;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Interfaces;


[TsInterface]
public interface IRequestResult
{
    public RequestResultTypes Result { get;  }
}
