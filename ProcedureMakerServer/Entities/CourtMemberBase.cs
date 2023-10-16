using ProcedureMakerServer.Enums;

namespace ProcedureMakerServer.Entities;

public abstract class CourtMemberBase : PersonBase
{
    public CourtRoles CourtRole { get; set; } = CourtRoles.PutInCause;

}
