using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Billing;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Initialization;
using ProcedureMakerServer.Models;
using RestSharp;

public class Program
{
    public static RestClient Client = new RestClient();
    public static void Main(string[] args)
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

        var loginResBody = JsonConvert.DeserializeObject<LoginResult>(loginResponse.Content);


        Guid lawyerId = loginResBody.UserDto.LawyerId;

        // create a case (and invoice)

        var caseCreationInfo = new CaseCreationInfo()
        {
            LawyerId = lawyerId,
            CaseNumber = "01",
            ClientFirstName = "Roger",
            ClientLastName = "Durand",

        };


        var s = Client.SendRequestWithBodyAndReturn<GetCaseResponse, CaseCreationInfo>(Method.Post, $"http://localhost:5099/case/createnewcase", caseCreationInfo);


        // get cases
        var caseContext = Client.SendRequestWithReturn<CasesContext>(Method.Get, $"http://localhost:5099/case/getcasescontext?lawyerId={lawyerId}");



        // then update a case info


        var caseToModifiy = caseContext.Cases.First();
        caseToModifiy.CourtAffairNumber = "GrosCube";
        Client.SendRequestNoReturnWithBody(Method.Put, $"http://localhost:5099/case/savecontextdto", caseToModifiy);


        var regottenContext = Client.SendRequestWithReturn<CasesContext>(Method.Get, $"http://localhost:5099/case/getcasescontext?lawyerId={lawyerId}");



        // get accoutn satement
        var statement = Client.SendRequestWithReturn<AccountStatement>(Method.Get, $"http://localhost:5099/invoice/getaccountstatement?caseId={caseToModifiy.Id}");


        // update the account statement

        Client.SendRequestNoReturnWithBody(Method.Post, "http://localhost:5099/invoice/updateinvoices", statement);


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