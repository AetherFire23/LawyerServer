namespace RequestTester;

public static class CaseContextDtoExtensions
{
	public static InvoiceDto GetFirstInvoice(this CaseContextDto ctx)
	{
		var invoice = ctx.Clients.First().Cases.First().Invoices.First();
		return invoice;
	}

	public static CaseDto GetFirstCase(this CaseContextDto ctx)
	{
		var firstCase = ctx.Clients.First().Cases.First();
		return firstCase;
	}
}
