
using Newtonsoft.Json;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Billing;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Models;
using ProcedureMakerServer.Trusts;
using RestSharp;
using DotLiquid;
using System.Text.RegularExpressions;
// https://next-auth.js.org/providers/google
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0

public class Program
{
    public static RestClient Client = new RestClient();
    public static async Task Main(string[] args)
    {
        await Task.Delay(450);

        //Test the dotliquid thingy:

        var f = File.ReadAllText("test.html");
        var p = Template.Parse(f);


       // les else
        string r = p.Render(Hash.FromAnonymousObject(new {user = new {name = "opps"}}));
        string d = p.Render();


        // les loops

        string looptest = p.Render(Hash.FromAnonymousObject(new { stuff = new List<string>{"allo", "bonsoir"}}));

        // la questions a 100%: jpeux tu faire des component aek ca 








        //LoginResult? loginResBody;
        //Guid lawyerId;
        //RegisterPhase(out loginResBody, out lawyerId);
        //CaseDto caseToModifiy = CreatingCasePhase(loginResBody, lawyerId);
        //TestTrustDto(caseToModifiy);
        // TestInvoicePhase(caseToModifiy);
    }

    private static void TestTrustDto(CaseDto caseToModifiy)
    {
        // manipulate the trust
        var dto = Client.SendRequestWithReturn<TrustDto>(Method.Get, $"http://localhost:5099/invoice/gettrustdto?clientId={caseToModifiy.Client.Id}");

        // modify the trusts content
        dto.Disburses.Add(new TrustDisburseDto()
        {
            Amount = 100,
            Date = DateTime.UtcNow,
        });
        dto.Disburses.Add(new TrustDisburseDto()
        {
            Amount = 200,
            Date = DateTime.UtcNow,
        });
        dto.Payments.Add(new TrustPaymentDto()
        {
            Amount = 100,
            Date = DateTime.UtcNow,
        });

        // save it
        Client.SendRequestNoReturnWithBody(Method.Post, $"http://localhost:5099/invoice/savetrustdto", dto);


        // remove some stuff then save
        dto.Disburses.RemoveAt(0);
        Client.SendRequestNoReturnWithBody(Method.Post, $"http://localhost:5099/invoice/savetrustdto", dto);

        var dto2 = Client.SendRequestWithReturn<TrustDto>(Method.Get, $"http://localhost:5099/invoice/gettrustdto?clientId={caseToModifiy.Client.Id}");
        // modify stuff


        dto2.Payments.First().Amount = 1000;

        // save it 
        Client.SendRequestNoReturnWithBody(Method.Post, $"http://localhost:5099/invoice/savetrustdto", dto2);


        var checkDtoBack = Client.SendRequestWithReturn<TrustDto>(Method.Get, $"http://localhost:5099/invoice/gettrustdto?clientId={caseToModifiy.Client.Id}");
    }

    private static void TestInvoicePhase(CaseDto caseToModifiy)
    {
        var accountStatementDto = Client.SendRequestWithReturn<AccountStatementDto>(Method.Get, $"http://localhost:5099/invoice/getaccountdto");

        // create invoice

        // add activity to invoice

        // add payment to invoice


        var invoice = accountStatementDto.Invoices.First();

        // Client.SendRequestNoReturnWithBody(Method.Post, $"http://localhost:5099/invoice/updateinvoiceactivites", accountStatementDto.i);
    }

    private static CaseDto CreatingCasePhase(LoginResult? loginResBody, Guid lawyerId)
    {
        var caseCreationInfo = new CaseCreationInfo()
        {
            LawyerId = lawyerId,
            CaseNumber = "01",
            ClientFirstName = "Roger",
            ClientLastName = "Durand",
        };

        GetCaseResponse getCaseResposne = Client.SendRequestWithBodyAndReturn<GetCaseResponse, CaseCreationInfo>(Method.Post, $"http://localhost:5099/case/createnewcase", caseCreationInfo);

        Client.AddDefaultHeader("Authorization", $"Bearer {loginResBody.Token}");
        var caseContextabcd = Client.SendRequestWithReturn<CasesContext>(Method.Get, $"http://localhost:5099/case/getcasescontext");

        // get cases
        var caseContext = Client.SendRequestWithReturn<CasesContext>(Method.Get, $"http://localhost:5099/case/getcasescontext?lawyerId={lawyerId}");

        // then update a case info

        var caseToModifiy = caseContext.Cases.First();
        caseToModifiy.CourtAffairNumber = "GrosCube";
        Client.SendRequestNoReturnWithBody(Method.Put, $"http://localhost:5099/case/savecontextdto", caseToModifiy);

        var regottenContext = Client.SendRequestWithReturn<CasesContext>(Method.Get, $"http://localhost:5099/case/getcasescontext?lawyerId={lawyerId}");
        return caseToModifiy;
    }

    private static void RegisterPhase(out LoginResult? loginResBody, out Guid lawyerId)
    {
        // register
        var registerRequest = new RegisterRequest()
        {
            Password = "password",
            Role = RoleTypes.Normal,
            Username = "username",
        };
        var req = new RestRequest("http://localhost:5099/User/register")
            .AddJsonBody(registerRequest);


        var res = Client.Post(req);



        var login = new LoginRequest()
        {
            Password = "password",
            Username = "username",
        };

        var loginReq = new RestRequest("http://localhost:5099/user/credentialslogin")
            .AddJsonBody(login);

        var loginResponse = Client.Put(loginReq);

        loginResBody = JsonConvert.DeserializeObject<LoginResult>(loginResponse.Content);

        lawyerId = loginResBody.UserDto.LawyerId;
    }
}


public static class RestSharpExtensions
{
    public static T GetBody<T>(this RestResponse self)
    {
        return JsonConvert.DeserializeObject<T>(self.Content);
    }

    public static TReturn SendRequestWithReturn<TReturn>(this RestClient self, Method method, string uri)
    {

        var req = new RestRequest(uri);
        req.Method = method;
        var res = self.Execute(req);


        Console.WriteLine(res.StatusCode);
        return res.GetBody<TReturn>();
    }
    public static TReturn SendRequestWithBodyAndReturn<TReturn, TBody>(this RestClient self, Method method, string uri, TBody body) where TBody : class
    {
        var req = new RestRequest(uri);
        req.Method = method;
        req.AddJsonBody(body);
        var res = self.Execute(req);
        Console.WriteLine(res.ErrorMessage);
        return res.GetBody<TReturn>();
    }

    public static void SendRequestNoReturnWithBody<TBody>(this RestClient self, Method method, string uri, TBody body) where TBody : class
    {
        var req = new RestRequest(uri);
        req.Method = method;
        req.AddJsonBody(body);
        var res = self.Execute(req);
        Console.WriteLine(res.ErrorMessage);
    }
}
