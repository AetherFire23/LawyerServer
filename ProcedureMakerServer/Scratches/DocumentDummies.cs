using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Enums;

namespace ProcedureMakerServer.Scratches;

public static class DocumentDummies
{
    private static CaseDto CaseDto { get; set; } = new()
    {
        CaseNumber = "20 015",
        Client = FirstClient,
        CourtAffairNumber = "200 000 04 04 04",
        ManagerLawyer = ExampleLawyer,
        Participants = new List<CasePart>()
        {
            Opposite,
        },
    };
    private static readonly Lawyer ExampleLawyer = new()
    {
        FirstName = "Roger",
        LastName = "TheLawyer",
        MobilePhoneNumber = "450-344-4444",
        Email = "RogerLawyer@hotmail.fr",
        NotificationEmail = "RogerMadysson@live.fr",
        Fax = "400-400-4444",
        WorkPhoneNumber = "999-999-9999"

    };

    private static readonly CasePart Opposite = new()
    {
        FirstName = "Madame",
        LastName = "Chose",
        NotificationEmail = "supertest@lolzida.com",
        CourtRole = CourtRoles.Defender,
    };

    private static readonly Client FirstClient = new()
    {
        FirstName = "Roger",
        LastName = "Durand",
        MobilePhoneNumber = "444-444-4444",
        PostalCode = "J2X 222",
        CourtRole = CourtRoles.Plaintiff,
    };

    public static CaseDto CreateCaseDto()
    {
        var dto = CaseDto;

        dto.ManagerLawyer = ExampleLawyer;
        dto.Client = FirstClient;
        dto.Participants = new List<CasePart> { Opposite };

        return dto;
    }
}
