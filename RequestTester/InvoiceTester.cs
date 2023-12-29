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
			Description = "My Activity2",
			Quantity = 2,
			CostInDollars = 2,
			IsDisburse = true,
			IsTaxable = false,
		};
		var activity4DisburseTaxable = new ActivityDto
		{
			Description = "My Activity2",
			Quantity = 2,
			CostInDollars = 2,
			IsDisburse = true,
			IsTaxable = true,
		};

		await _caller.CreateactivityAsync(invoice.Id, activity);
		await _caller.CreateactivityAsync(invoice.Id, activity2);
		await _caller.CreateactivityAsync(invoice.Id, activity3DisburseNonTaxable);
		await _caller.CreateactivityAsync(invoice.Id, activity4DisburseTaxable);
	}

	public async Task UpdateActivity()
	{
		var lcaseCtx = await _caller.GetcasescontextAsync();

		var lcase = lcaseCtx.GetFirstCase();
		var activity = lcase.Invoices.First().Activities.First();
		activity.Quantity = 420;

		await _caller.UpdateactivityAsync(activity);
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

		var invoicePaymentDto1 = new InvoicePaymentDto()
		{
			AmountPaid = 3,
			IsPaymentComingFromTrust = false,
			AmoundPaidDate = DateTime.Now,
		};
		var invoicePaymentDto2 = new InvoicePaymentDto()
		{
			AmountPaid = 3,
			IsPaymentComingFromTrust = false,
			AmoundPaidDate = DateTime.Now,
		};
		var invoicePaymentDtoWithTrustDisburse = new InvoicePaymentDto()
		{
			AmountPaid = 3,
			IsPaymentComingFromTrust = true,
			AmoundPaidDate = DateTime.Now,
		};
		var invoicePaymentDtoWithTrustDisburse2 = new InvoicePaymentDto()
		{
			AmountPaid = 3,
			IsPaymentComingFromTrust = true,
			AmoundPaidDate = DateTime.Now,
		};



		await _caller.AddinvoicepaymentAsync(invoice.Id, invoicePaymentDto1);
		await _caller.AddinvoicepaymentAsync(invoice.Id, invoicePaymentDto2);
		await _caller.AddinvoicepaymentAsync(invoice.Id, invoicePaymentDtoWithTrustDisburse);
		await _caller.AddinvoicepaymentAsync(invoice.Id, invoicePaymentDtoWithTrustDisburse2);

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
