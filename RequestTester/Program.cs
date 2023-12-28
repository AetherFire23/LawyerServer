using RequestTester;
using System.Diagnostics;
// https://next-auth.js.org/providers/google
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0

// THis is emulating a client.
// So no project references please.
public class Program
{
	private static HttpClient _client = new HttpClient();
	private static NSwagCaller _caller = new NSwagCaller("http://localhost:5099/", _client);
	public static async Task Main(string[] args)
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
		await invoiceTester.ModifyInvoicePayment();
		await invoiceTester.RemoveInvoicePayment();

		// TRUST 
		await invoiceTester.AddTrustFunds();
		await invoiceTester.ModifyTrustFundPayment();
		await invoiceTester.RemoveTrustFund();


		await DownloadInvoice();

		var lastState = await _caller.GetcasescontextAsync();
	}

	private static async Task CaseManagement()
	{
		await UpdateLawyer();
		await CreateInitialClient();
		await CreateInitialCase();
		await UpdateCase();
		await AddCaseParticipants();
		await RemoveCaseParticipant();
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
			NotificationEmail = "richerf3212@gmail.com",
			FirstName = "Piotr",
			LastName = "Alexandrovich",
			Country = "Poland",

		};
		await _caller.AddclientAsync(clientDto);
	}
	private static async Task CreateInitialCase()
	{
		var caseContext = await _caller.GetcasescontextAsync();
		var firstclient = caseContext.Clients.First();

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

		var modifiedCase = caseContext.GetFirstCase();
		modifiedCase.CaseNumber = "4444";

		await _caller.SavecaseAsync(modifiedCase);
	}
	private static async Task AddCaseParticipants()
	{
		var caseContext = await _caller.GetcasescontextAsync();
		var lcase = caseContext.GetFirstCase();
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

		await _caller.CreatecaseparticipantAsync(lcase.Id, participant1);
		await _caller.CreatecaseparticipantAsync(lcase.Id, participant2);
		await _caller.CreatecaseparticipantAsync(lcase.Id, participant3);
	}

	private static async Task UpdateCaseParticipant()
	{
		var caseContext = await _caller.GetcasescontextAsync();
		var participant = caseContext.GetFirstCase().Participants.First();
		participant.Address = "Piotrek Lord";

		await _caller.UpdatecaseparticipantAsync(participant);
	}

	private static async Task RemoveCaseParticipant()
	{
		var caseContext = await _caller.GetcasescontextAsync();

		Guid participantId = caseContext.GetFirstCase().Participants.First(x => x.CourtRole == CourtRoles.Intimated).Id;

		await _caller.RemovecaseparticipantAsync(participantId);
	}

	private static async Task SendNotificationPdfOnlyAndGetProofOfNotificationBack()
	{
		if (true) return;

		var caseContext = await _caller.GetcasescontextAsync();
		var lcase = caseContext.GetFirstInvoice();

		string path = "FunFiles/ville_et_la_prison.pdf";

		var fileBytes = File.ReadAllBytes(path);
		using var fileStream = File.Open(path, FileMode.Open);
		var fileParameter = new RequestTester.FileParameter(fileStream);
		var f = await _caller.NotifypdfAsync(lcase.Id, "Ville et Prison", fileParameter);

		var t = f.Stream.ReadToEnd();

		File.WriteAllBytes("test.pdf", t);
	}

	// TEST INVOICE

	private static async Task DownloadInvoice()
	{
		var ctx = await _caller.GetcasescontextAsync();

		await _caller.GetinvoiceAsync(ctx.Clients.First().Cases.First().Invoices.First().Id);

	}
}

public static class StreamExtensions
{
	public static byte[] ReadToEnd(this Stream stream)
	{
		var bytes = new List<byte>();

		int lastResult = 0;
		while (lastResult != -1) // stream returns -1 when it is the last byte
		{
			lastResult = stream.ReadByte();
			bytes.Add((byte)lastResult);
		}

		return bytes.ToArray();
	}
}