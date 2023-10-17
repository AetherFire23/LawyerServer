

using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Initialization;

public class Seeder
{
    public static List<Lawyer> SampleLawyers = new()
    {
        new Lawyer() { FirstName = "fred", LastName = "richer", Email = "richerf@master.com", Id = Ids.FredId,},
        new Lawyer() { FirstName = "john", LastName = "granger", Email = "grangerf@master.com", Id = Ids.JohnId,},
    };
}

public class Ids
{
    public static Guid FredId = new Guid("c931cc96-4e87-49f6-8037-111041bcc774");
    public static Guid JohnId = new Guid("3284ac9d-81b2-4fc0-82fb-47855df39814");
}