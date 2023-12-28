using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestTester;

public static class CaseContextDtoExtensions
{
    public static InvoiceDto GetFirstInvoice(this CaseContextDto ctx)
    {
        var invoice = ctx.Cases.First().Invoices.First();
        return invoice;
    }
}
