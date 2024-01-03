namespace RequestTester;

internal class InvoiceTester
{
    private readonly NSwagCaller _caller;

    public InvoiceTester(NSwagCaller caller)
    {
        _caller = caller;
    }

    public async Task TestCreateInvoices()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();

        var lcase = lcaseCtx.GetFirstCase();

        await _caller.CreateinvoiceAsync(lcase.Id);
        await _caller.CreateinvoiceAsync(lcase.Id);
    }

    public async Task TestUpdateInvoice()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var invoice = lcaseCtx.GetFirstCase().Invoices.First();

        invoice.InvoiceStatus = InvoiceStatuses.Paid;

        await _caller.UpdateinvoiceAsync(invoice);
    }

    public async Task RemoveInvoice()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var invoice = lcaseCtx.GetFirstCase().Invoices.First();


        await _caller.ArchiveinvoiceAsync(invoice.Id);
    }

    public async Task DeleteInvoice()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var invoice = lcaseCtx.GetFirstCase().Invoices.First();

        invoice.InvoiceStatus = InvoiceStatuses.Paid;

        await _caller.UpdateinvoiceAsync(invoice);
    }

    public async Task TestCreateActivities()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();

        var lcase = lcaseCtx.GetFirstCase();
        var invoice = lcase.Invoices.First();



        await _caller.CreateactivityAsync(invoice.Id);
        await _caller.CreateactivityAsync(invoice.Id);
        await _caller.CreateactivityAsync(invoice.Id);
        await _caller.CreateactivityAsync(invoice.Id);
    }

    public async Task UpdateActivity()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();

        // for each activity, update to some default value
        var activity = new ActivityDto
        {
            Description = "My Activity1",
            Quantity = 1,
            CostInDollars = 1,
            IsTaxable = true,
        };
        var activity2 = new ActivityDto
        {
            Description = "My Activity2",
            Quantity = 2,
            CostInDollars = 2,
            IsTaxable = true,
        };
        var activity3DisburseNonTaxable = new ActivityDto
        {
            Description = "My Activity3",
            Quantity = 2,
            CostInDollars = 2,
            IsDisburse = true,
            IsTaxable = false,
        };
        var activity4DisburseTaxable = new ActivityDto
        {
            Description = "My Activity4",
            Quantity = 2,
            CostInDollars = 2,
            IsDisburse = true,
            IsTaxable = true,
        };
        var defaultActivities = new List<ActivityDto>
        {
            activity,
            activity2,
            activity3DisburseNonTaxable,
            activity4DisburseTaxable,
        };

        var lcase = lcaseCtx.GetFirstCase();

        // update the activities that were iniially created.
        // implies that I created 4 activities. 
        for (int i = 0; i < lcase.Invoices.First().Activities.Count; i++)
        {
            var def = lcase.Invoices.First().Activities.ElementAt(i);
            var modifyTo = defaultActivities.ElementAt(i);

            modifyTo.Id = def.Id;
            await _caller.UpdateactivityAsync(modifyTo);
        }
    }

    public async Task RemoveActivity()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var activityId = lcaseCtx.GetFirstCase().Invoices.First().Activities.First().Id;
        await _caller.RemoveactivityAsync(activityId);
    }
    public async Task AddInvoicePayments()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var invoice = lcaseCtx.GetFirstInvoice();

        await _caller.AddinvoicepaymentAsync(invoice.Id);
        await _caller.AddinvoicepaymentAsync(invoice.Id);
        await _caller.AddinvoicepaymentAsync(invoice.Id);
        await _caller.AddinvoicepaymentAsync(invoice.Id);
        var lcaseCtx2 = await _caller.GetcasescontextAsync();
    }

    public async Task ModifyInvoicePayment()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();

        var invoicePayment = lcaseCtx.GetFirstInvoice().Payments.First();

        invoicePayment.AmountPaid = 3;

        await _caller.UpdateinvoicepaymentAsync(invoicePayment);
    }

    public async Task RemoveInvoicePayment()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var payment = lcaseCtx.GetFirstCase().Invoices.First().Payments.First();

        await _caller.RemoveinvoicepaymentAsync(payment.Id);
    }

    public async Task AddTrustFunds()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var clientId = lcaseCtx.Clients.First().Id;

        var trustFundDto = new TrustPaymentDto
        {
            Amount = 1,
        };

        var trustFundDto2 = new TrustPaymentDto
        {
            Amount = 1,

        };

        await _caller.AddfundstotrustAsync(clientId, trustFundDto);

        await _caller.AddfundstotrustAsync(clientId, trustFundDto2);
        var lcaseCtx2 = await _caller.GetcasescontextAsync();
    }

    public async Task ModifyTrustFundPayment()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var trustPayment = lcaseCtx.Clients.First().TrustClientCard.Payments.First();
        trustPayment.Amount = 420;

        await _caller.UpdatetrustpaymentAsync(trustPayment);
        await _caller.GetcasescontextAsync();
    }

    // Remove trust fund I gues
    public async Task RemoveTrustFund()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var trustPayment = lcaseCtx.Clients.First().TrustClientCard.Payments.First();

        await _caller.RemovetrustpaymentAsync(trustPayment.Id);
    }
}
