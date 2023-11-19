using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Repository.ProcedureRepo;
namespace ProcedureMakerServer.Trusts;

public partial class TrustRepository : ProcedureRepositoryContextBase
{
    public TrustRepository(IMapper mapper, ProcedureContext context) : base(mapper, context)
    {

    }

    public async Task SaveTrustDto(TrustDto dto)
    {
        Trust trustEntity = await Context.Trusts.FirstAsync(x => x.Id == dto.Id);
        await SaveTrustDisburses(dto, trustEntity);
        await SaveTrustPayments(dto, trustEntity);
    }

    public async Task SaveTrustPayments(TrustDto dto, Trust trustEntity)
    {
        foreach (TrustPaymentDto payments in dto.Payments)
        {
            TrustPayment payment = await Context.TrustPayments.FirstAsync(x => x.Id == payments.Id);
            payment.CopyFromDto(payments, trustEntity);
        }
    }

    public async Task SaveTrustDisburses(TrustDto trustDto, Trust trustEntity)
    {
        foreach (var disburseDto in trustDto.Disburses)
        {
            TrustDisburse trustDisburseEntity = await Context.TrustDisburses.FirstAsync(x => x.Id == disburseDto.Id);
            trustDisburseEntity.CopyFromDto(disburseDto, trustEntity);
        }
    }
}
