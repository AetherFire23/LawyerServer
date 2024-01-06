using Microsoft.AspNetCore.Mvc.Testing;
using ProcedureMakerServer;
using Xunit.Abstractions;

namespace IntegrationTest;

public class BasicTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    private readonly Swagpoints _caller;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _caller = new Swagpoints("/", _client);
    }

    [Fact]
    public async Task TestApplication()
    {
        // REGISTER & LOGIN 
        await Task.Delay(3500);
        var registerResult = await RegisterTest();
        var logResult = await LoginTest(registerResult);

        await PrepareAndTestAuthenticationHeaders(logResult.Token);
        await CaseManagement();

        // NOTIFY 
        await SendNotificationPdfOnlyAndGetProofOfNotificationBack();

        // INVOICE 
        var invoiceTester = new InvoiceTester(_caller);

        await invoiceTester.TestCreateInvoices();
        await invoiceTester.TestUpdateInvoice();
        await invoiceTester.RemoveInvoice();

        await invoiceTester.TestCreateActivities();
        await invoiceTester.UpdateActivity();
        await invoiceTester.RemoveActivity();

        await invoiceTester.AddInvoicePayments();
        await invoiceTester.ModifyInvoicePayments();
        await invoiceTester.RemoveInvoicePayment();

        // TRUST 
        await invoiceTester.AddTrustFunds();
        await invoiceTester.ModifyTrustFundPayment();
        await invoiceTester.RemoveTrustFund();


        await DownloadInvoice();

        var lastState = await _caller.GetcasescontextAsync();
    }
    private async Task CaseManagement()
    {
        await UpdateLawyer();
        await CreateInitialClient();
        await CreateInitialCase();
        await UpdateCase();
        await AddCaseParticipants();
        await RemoveCaseParticipant();
        await UpdateCaseParticipant();
    }
    private async Task<RegisterRequest> RegisterTest()
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
    private async Task<LoginResult> LoginTest(RegisterRequest requested)
    {
        var loginRequest = new LoginRequest
        {
            Username = requested.Username,
            Password = requested.Password,
        };
        var loginResult = await _caller.CredentialsloginAsync(loginRequest);
        return loginResult;
    }
    private async Task PrepareAndTestAuthenticationHeaders(string token)
    {
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        await _caller.AuthorizedrequestblablaAsync();
    }
    private async Task UpdateLawyer()
    {
        var caseContext = await _caller.GetcasescontextAsync();

        caseContext.Lawyer.Country = "sieks";
        caseContext.Lawyer.DateOfBirth = DateTime.Now;
        caseContext.Lawyer.Address = "MyAdressIsGreat";

        await _caller.UpdatelawyerAsync(caseContext.Lawyer);
    }
    private async Task CreateInitialClient()
    {
        var clientDto = new ClientDto()
        {
            Id = Guid.Empty,
            NotificationEmail = "richerf3212@gmail.com",
            FirstName = "Piotr",
            LastName = "Alexandrovich",
            Country = "Poland",

        };
        await _caller.AddclientAsync();
    }
    private async Task CreateInitialCase()
    {
        var caseContext = await _caller.GetcasescontextAsync();
        var firstclient = caseContext.Clients.First();

        await _caller.CreatenewcaseAsync(firstclient.Id);
    }
    private async Task UpdateCase()
    {
        var caseContext = await _caller.GetcasescontextAsync();

        var modifiedCase = caseContext.GetFirstCase();
        modifiedCase.CaseNumber = "4444";

        await _caller.SavecaseAsync(modifiedCase);
    }
    private async Task AddCaseParticipants()
    {
        var caseContext = await _caller.GetcasescontextAsync();
        var lcase = caseContext.GetFirstCase();

        await _caller.CreatecaseparticipantAsync(lcase.Id);
        await _caller.CreatecaseparticipantAsync(lcase.Id);
        await _caller.CreatecaseparticipantAsync(lcase.Id);
    }

    private async Task UpdateCaseParticipant()
    {
        var caseContext = await _caller.GetcasescontextAsync();
        var participants = caseContext.GetFirstCase().Participants;
        var participant1 = new CaseParticipantDto()
        {
            WorkPhoneNumber = "4444",
            FirstName = "Mother",
            LastName = "Of Piotr",
            Address = "333, ddd Avenue",
            NotificationEmail = "richerf3212@gmail.com",
            CourtRole = CourtRoles.Defender,
            MustNotify = true,
        };

        var participant2 = new CaseParticipantDto()
        {
            WorkPhoneNumber = "4444",
            FirstName = "Father",
            LastName = "Of Piotr",
            Address = "333, ddd Avenue",
            NotificationEmail = "richerf3212@gmail.com",
            CourtRole = CourtRoles.Plaintiff,
            MustNotify = true,
        };

        var participant3 = new CaseParticipantDto()
        {
            WorkPhoneNumber = "4444",
            FirstName = "Father3",
            LastName = "Of Piotr",
            Address = "333, ddd Avenue",
            NotificationEmail = "richerf3212@gmail.com",
            CourtRole = CourtRoles.Intimated,
            MustNotify = true,
        };

        var modifiedParticipants = new List<CaseParticipantDto>() { participant1, participant2, participant3 };
        for (int i = 0; i < participants.Count; i++)
        {
            var currParticipant = participants.ElementAt(i);
            var modified = modifiedParticipants.ElementAt(i);
            modified.Id = currParticipant.Id;
            await _caller.UpdatecaseparticipantAsync(modified);
        }
    }

    private async Task RemoveCaseParticipant()
    {
        var caseContext = await _caller.GetcasescontextAsync();

        Guid participantId = caseContext.GetFirstCase().Participants.First().Id;

        await _caller.RemovecaseparticipantAsync(participantId);
    }

    private async Task SendNotificationPdfOnlyAndGetProofOfNotificationBack()
    {
        if (true) return;

        var caseContext = await _caller.GetcasescontextAsync();
        var lcase = caseContext.GetFirstInvoice();

        string path = "FunFiles/ville_et_la_prison.pdf";

        var fileBytes = File.ReadAllBytes(path);
        using var fileStream = File.Open(path, FileMode.Open);
        var fileParameter = new FileParameter(fileStream);
        var f = await _caller.NotifypdfAsync(lcase.Id, "Ville et Prison", fileParameter);

        var t = f.Stream.ReadToEnd();

        File.WriteAllBytes("test.pdf", t);
    }

    // TEST INVOICE
    private async Task DownloadInvoice()
    {
        var ctx = await _caller.GetcasescontextAsync();
        var res = await _caller.GetinvoiceAsync(ctx.Clients.First().Cases.First().Invoices.First().Id);
    }
}

public static class StreamExtensions
{
    public static byte[] ReadToEnd(this Stream stream)
    {
        var bytes = new List<byte>();

        int readValue = 0;
        bool isLastByte = readValue == -1;
        while (!isLastByte) // stream returns -1 when it is the last byte
        {
            readValue = stream.ReadByte();
            bytes.Add((byte)readValue);
        }

        return [.. bytes];
    }
}