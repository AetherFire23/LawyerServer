using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Scratches;

public static class DocumentDummies
{
    public static Lawyer ExampleLawyer = new Lawyer()
    {
        FirstName = "Fred",
        LastName = "Master",
        MobilePhoneNumber = "450-344-4444"
    };
    public static Client Client = new Client()
    {
         PostalCode = "J2X 222"
    };
    public static CaseDto CaseDto { get; set; } = new CaseDto()
    {
        CaseNumber = "20 015",
        Client = Client,
        CourtAffairNumber = "200 000 04 04 04",
        ManagerLawyer = ExampleLawyer,
    };
}
