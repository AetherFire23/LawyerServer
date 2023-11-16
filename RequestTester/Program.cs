using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Billing;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Initialization;
using ProcedureMakerServer.Models;
using ProcedureMakerServer.Utils;
using RestSharp;

public class Program
{
    public static RestClient Client = new RestClient();
    public static async Task Main(string[] args)
    {
        await Task.Delay(450);
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



        // get accoutn satement
        var statement = Client.SendRequestWithReturn<AccountStatementDto>(Method.Get, $"http://localhost:5099/invoice/getaccountstatement?caseId={caseToModifiy.Id}");


        // add Invoice
        Client.SendRequestWithReturn<IActionResult>(Method.Post, $"http://localhost:5099/invoice/addinvoice?caseId={caseToModifiy.Id.ToString()}");

        var statement2 = Client.SendRequestWithReturn<AccountStatementDto>(Method.Get, $"http://localhost:5099/invoice/getaccountstatement?caseId={caseToModifiy.Id}");

        // add activity 

        ActivityCreation activityCreation = new()
        {
            BillingElementId = statement2.LawyerBillingElements.First().Id,
            HasPersonalizedBillingElement = false,
            HoursWorked = 1,
            InvoiceId = statement2.Invoices.First().Id,
        };


        Client.SendRequestNoReturnWithBody(Method.Post, $"http://localhost:5099/invoice/addactivity", activityCreation);

        var statement3 = Client.SendRequestWithReturn<AccountStatementDto>(Method.Get, $"http://localhost:5099/invoice/getaccountstatement?caseId={caseToModifiy.Id}");

        // add payment

        PaymentCreationRequest payment = new PaymentCreationRequest()
        {
            AmountPaid = 100,
            AmountPaidDate = DateTime.UtcNow,
            InvoiceId = statement3.Invoices.First().Id,
        };

        Client.SendRequestNoReturnWithBody(Method.Post, $"http://localhost:5099/invoice/addpayment", payment);
        var statement4 = Client.SendRequestWithReturn<AccountStatementDto>(Method.Get, $"http://localhost:5099/invoice/getaccountstatement?caseId={caseToModifiy.Id}");


        // add billing element  
        var statement5 = Client.SendRequestWithReturn<AccountStatementDto>(Method.Get, $"http://localhost:5099/invoice/getaccountstatement?caseId={caseToModifiy.Id}");

        BillingElementCreationRequest billingElemetnRequest = new BillingElementCreationRequest()
        {
            AccountStatementId = statement4.Id,
            IsHourlyRate = true,
            ActivityName = "Test",
            Amount = 420,
            IsPersonalizedBillingElement = false,
        };
        Client.SendRequestNoReturnWithBody(Method.Post, $"http://localhost:5099/invoice/addbillingelement", billingElemetnRequest);


        // update the billing element to another one.
        // from JuridicalWork to Test
        var statement6 = Client.SendRequestWithReturn<AccountStatementDto>(Method.Get, $"http://localhost:5099/invoice/getaccountstatement?caseId={caseToModifiy.Id}");


        statement6.Invoices.First().Activities.First().BillingElement.Id = statement6.LawyerBillingElements.First(x => x.ActivityName == "Test").Id;
        Client.SendRequestNoReturnWithBody(Method.Post, "http://localhost:5099/invoice/updateinvoices", statement6.Invoices.First());

        var statement7 = Client.SendRequestWithReturn<AccountStatementDto>(Method.Get, $"http://localhost:5099/invoice/getaccountstatement?caseId={caseToModifiy.Id}");

        //statement4.Invoices.First().InvoiceStatus = InvoiceStatuses.Paid;
        //statement4.Invoices.First().Activities.First().BillingElement.Id = 

        // Client.SendRequestNoReturnWithBody(Method.Post, "http://localhost:5099/invoice/updateinvoices", statement4.Invoices.First());



        // Add personalized billing element
        // add billing element
        // add activity

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