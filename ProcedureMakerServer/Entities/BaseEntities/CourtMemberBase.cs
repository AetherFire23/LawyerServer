using ProcedureMakerServer.Enums;

namespace ProcedureMakerServer.Entities.BaseEntities;

public abstract class CourtMemberBase : PersonBase
{
    public CourtRoles CourtRole { get; set; }

}
