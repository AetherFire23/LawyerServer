using DotLiquid.Util;
using Newtonsoft.Json;
using RequestTester;
using RestSharp;
using System.Runtime.CompilerServices;
// https://next-auth.js.org/providers/google
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0

public class Program
{
    private static HttpClient _client = new HttpClient();
    private static NSwagCaller _caller;

    public static async Task Main(string[] args)
    {
        // Register & Login
        await Task.Delay(3500);
        _caller = new NSwagCaller("http://localhost:5099/", _client);

        var registerResult = await RegisterTest();
        var logResult = await LoginTest(registerResult);

        await PrepareAndTestAuthenticationHeaders(logResult.Token);

        await CaseManagement();

        // NOTIFY 

        await SendNotificationPdfOnlyAndGetProofOfNotificationBack();
    }

    private static async Task CaseManagement()
    {
        // UpdateLawyer
        await UpdateLawyer();

        // create new client
        await CreateInitialClient();

        // create new case for client
        await CreateInitialCase();

        // update information for case
        await UpdateCase();

        // add CaseParticipant
        await AddCaseParticipant();

        await UpdateCaseParticipant();
    }

    private static async Task<RegisterRequest> RegisterTest()
    {
        var registerRequest = new RegisterRequest()
        {
            Username = "FredLeChaud",
            Password = "MotDePasse",
            Role = RoleTypes.Normal,
        };

        await _caller.RegisterAsync(registerRequest);

        return registerRequest;
    }

    private static async Task<LoginResult> LoginTest(RegisterRequest requested)
    {
        var loginRequest = new LoginRequest
        {
            Username = requested.Username,
            Password = requested.Password,
        };
        var loginResult = await _caller.CredentialsloginAsync(loginRequest);
        return loginResult;
    }
    private static async Task PrepareAndTestAuthenticationHeaders(string token)
    {
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        await _caller.AuthorizedrequestblablaAsync();
    }

    private static async Task UpdateLawyer()
    {
        var caseContext = await _caller.GetcasescontextAsync();

        caseContext.Lawyer.Country = "sieks";
        caseContext.Lawyer.DateOfBirth = DateTime.Now;
        caseContext.Lawyer.Address = "MyAdressIsGreat";

        await _caller.UpdatelawyerAsync(caseContext.Lawyer);
    }

    private static async Task CreateInitialClient()
    {
        var clientDto = new ClientDto()
        {
            Id = Guid.Empty,
            NotificationEmail = "piotrullo@polish.com",
            FirstName = "Piotr",
            LastName = "Alexandrovich",
            Country = "Poland",
        };
        await _caller.AddclientAsync(clientDto);
    }

    private static async Task CreateInitialCase()
    {
        var caseContext = await _caller.GetcasescontextAsync();
        var firstclient = caseContext.Lawyer.Clients.First();

        var caseCreation = new CaseCreationInfo()
        {
            CaseNumber = "20 001",
            ClientId = firstclient.Id,
        };

        await _caller.CreatenewcaseAsync(caseCreation);
    }
    private static async Task UpdateCase()
    {
        var caseContext = await _caller.GetcasescontextAsync();

        var modifiedCase = caseContext.Cases.First();
        modifiedCase.CaseNumber = "4444";

        await _caller.SavecaseAsync(modifiedCase);
    }

    private static async Task AddCaseParticipant()
    {
        var caseContext = await _caller.GetcasescontextAsync();
        var lcase = caseContext.Cases.First();
        var participant = new CaseParticipantDto()
        {
            WorkPhoneNumber = "sex",
            FirstName = "Mother",
            LastName = "Of Pitor",
            Address = "333, Bitchass Avenue",
        };

        await _caller.CreatecaseparticipantAsync(lcase.Id, participant);
    }

    private static async Task UpdateCaseParticipant()
    {
        var caseContext = await _caller.GetcasescontextAsync();
        var participant = caseContext.Cases.First().Participants.First();
        participant.Address = "Piotrek Lord";

        await _caller.UpdatecaseparticipantAsync(participant);
    }

    private static async Task SendNotificationPdfOnlyAndGetProofOfNotificationBack()
    {
        var caseContext = await _caller.GetcasescontextAsync();
        var lcase = caseContext.Cases.First();


        string path = "FunFiles/ville_et_la_prison.pdf";
        using var fileStream = File.Open(path, FileMode.Open);
        var fileParameter = new RequestTester.FileParameter(fileStream);
        await _caller.NotifypdfAsync(lcase.Id, "Ville et Prison", fileParameter);
    }
}
