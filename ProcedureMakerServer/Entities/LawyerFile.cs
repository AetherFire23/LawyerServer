using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Enums;

namespace EFCoreBase.Entities;

public class LawyerFile : EntityBase
{
    public Guid LawyerId { get; set; }
    public Lawyer Lawyer { get; set; } = new Lawyer();
    public string CourtNumber { get; set; } = string.Empty;
    public string FileNumber { get; set; } = string.Empty; // cases can have different filenumbers if it comes again many times 
    public CourtType CourtType { get; set; }
}
