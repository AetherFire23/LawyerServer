using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using ProcedureMakerServer.Enums;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Entities.BaseEntities;

[TsClass]
public abstract class CourtMemberBase : PersonBase
{
    // if string.empty, means that the target is not notifiabvle
    public string NotificationEmail { get; set; } = string.Empty;

    [TsIgnore]
    public string FullName => $"{FirstName} {LastName}";
    public CourtRoles CourtRole { get; set; }

    public bool IsNotifiable => NotificationEmail != string.Empty;



    public string GetGenderedCourtRoleName()
    {
        switch (CourtRole)
        {
            case CourtRoles.Plaintiff:
                {
                    return this.Gender == Genders.Male ? "Demandeur" : "Demanderesse";
                }
            case CourtRoles.Defender:
                {
                    return this.Gender == Genders.Female ? "Défendeur" : "Défenderesse";
                }
            default: return string.Empty;
        }
    }
}
