using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Enums;


[TsEnum]
public enum CourtRoles
{
    Intimated,
    PutInCause,
    Plaintiff,
    Defender,
    PlaintiffLawyer,
    DefenderLawyer
}
