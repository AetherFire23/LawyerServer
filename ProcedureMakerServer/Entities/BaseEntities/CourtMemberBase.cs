using ProcedureMakerServer.Enums;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Entities.BaseEntities;

[TsClass]
public abstract class CourtMemberBase : PersonBase
{
    public CourtRoles CourtRole { get; set; }

}
