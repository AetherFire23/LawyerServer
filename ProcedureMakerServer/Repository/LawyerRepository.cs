using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Repository.ProcedureRepo;
namespace ProcedureMakerServer.Repository;

public class LawyerRepository : ProcedureCrudBase<Lawyer>
{
    public LawyerRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public override async Task<Lawyer> GetEntityById(Guid id)
    {
        Lawyer? lawyer = await Set
            .Include(l => l.DefaultHourlyElement)
            .Include(l => l.Cases)
            .Include(l => l.User)
            .Include(l => l.Clients)
                .ThenInclude(c => c.TrustClientCard)
            .FirstOrDefaultAsync(l => l.Id == id);

        return lawyer;
    }

    public async Task<LawyerDto> MapLawyerDto(Guid lawyerId)
    {
        var lawyer = await GetEntityById(lawyerId);
        var lawyerGlobalBillingElements = await GetGlobalBillingElementDtosForLawyer(lawyerId);
        var defaultBillingElementDto = lawyer.DefaultHourlyElement?.ToDto();
        var lawyerDto = new LawyerDto
        {
            BillingElements = lawyerGlobalBillingElements,
            DefaultHourlyElement = defaultBillingElementDto,
            CourtLockerNumber = lawyer.CourtLockerNumber,
            BillsEmittedCount = lawyer.BillsEmittedCount,
        };

        lawyerDto.CopyFromCourtMember(lawyer);

        return lawyerDto;
    }

    public async Task<List<BillingElementDto>> GetGlobalBillingElementDtosForLawyer(Guid lawyerId)
    {
        var billingElements = await Context.BillingElements
            .Where(x => !x.IsInvoiceSpecific)
            .Where(x => x.ManagerLawyerId == lawyerId)
            .Select(x => x.ToDto())
            .ToListAsync();

        return billingElements;
    }

    public async Task<Lawyer> GetLawyerFromUserId(Guid userId)
    {
        Lawyer lawyer = await this.Set
            .Include(l => l.Cases)
            .FirstAsync(l => l.UserId == userId);

        return lawyer;
    }

    public async Task UpdateLawyer(LawyerDto updatedLawyer)
    {
        Lawyer entity = await GetEntityById(updatedLawyer.Id);

        entity.CopyFromCourtMember(updatedLawyer);

        entity.CourtLockerNumber = updatedLawyer.CourtLockerNumber;

        await Context.SaveChangesAsync();
    }
}
