namespace IntegrationTest;

public class InvoiceTester
{
    private readonly Swagpoints _caller;

    public InvoiceTester(Swagpoints caller)
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
            Quantity = 3,
            CostInDollars = 2,
            IsDisburse = true,
            IsTaxable = false,
        };
        var activity4DisburseTaxable = new ActivityDto
        {
            Description = "My Activity4",
            Quantity = 4,
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
        var activities = lcase.Invoices.First().Activities;

        foreach ((var modified, var old) in defaultActivities.Zip(activities))
        {
            modified.Id = old.Id;
            await _caller.UpdateactivityAsync(modified);
        };
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

        var t = (Enumerable.Range(0, 4).ToList()
            .Select(x => _caller.AddinvoicepaymentAsync(invoice.Id)));

        await Task.WhenAll(t);

    }

    public async Task ModifyInvoicePayments()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();

        var payment = new InvoicePaymentDto
        {
            AmoundPaidDate = DateTime.Now,
            AmountPaid = 3,
            IsPaymentComingFromTrust = false,
            Method = "Argent comptant",
        };
        var trustPayment = new InvoicePaymentDto
        {
            AmoundPaidDate = DateTime.Now,
            AmountPaid = 3,
            IsPaymentComingFromTrust = true,
            Method = "Should not show up",
        };
        var defaultActivities = new List<InvoicePaymentDto>
        {
            payment, trustPayment,
        };

        // 4 paments rn
        foreach ((var modified, var old) in defaultActivities.Zip(lcaseCtx.Clients.First().Cases.First().Invoices.First().Payments))
        {
            modified.Id = old.Id;
            await _caller.UpdateinvoicepaymentAsync(modified);
        }
    }

    public async Task RemoveInvoicePayment()
    {
        //var lcaseCtx = await _caller.GetcasescontextAsync();
        //var payment = lcaseCtx.GetFirstCase().Invoices.First().Payments.First();

        //await _caller.RemoveinvoicepaymentAsync(payment.Id);
    }

    public async Task AddTrustFunds()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var clientId = lcaseCtx.Clients.First().Id;

        await _caller.AddfundstotrustAsync(clientId);
        await _caller.AddfundstotrustAsync(clientId);
    }

    public async Task ModifyTrustFundPayment()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var trustPayments = lcaseCtx.Clients.First().TrustClientCard.Payments;

        var trustFundDto = new TrustPaymentDto
        {
            Amount = 1,
        };
        var trustFundDto2 = new TrustPaymentDto
        {
            Amount = 1,
        };
        var modifiedPayments = new List<TrustPaymentDto> { trustFundDto, trustFundDto2 };

        foreach ((var modified, var old) in modifiedPayments.Zip(trustPayments))
        {
            modified.Id = old.Id;
            await _caller.UpdatetrustpaymentAsync(modified);
        }
    }

    // Remove trust fund I gues
    public async Task RemoveTrustFund()
    {
        var lcaseCtx = await _caller.GetcasescontextAsync();
        var trustPayment = lcaseCtx.Clients.First().TrustClientCard.Payments.First();

        await _caller.RemovetrustpaymentAsync(trustPayment.Id);
    }
}
