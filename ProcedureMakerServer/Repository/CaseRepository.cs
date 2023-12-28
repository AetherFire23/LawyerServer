using AutoMapper;
using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Repository;

public class CaseRepository : ProcedureRepositoryContextBase
{
	public CaseRepository(IMapper mapper, ProcedureContext context) : base(mapper, context)
	{

	}

	public async Task<Case> GetCase(Guid id)
	{
		Case lcase = await Context.Cases
			.Include(c => c.CaseParticipants)
			.Include(c => c.Client)
			.Include(c => c.ManagerLawyer)
			.Include(x => x.AccountStatement)
				.ThenInclude(x => x.Invoices)
			.FirstAsync(c => c.Id == id);

		return lcase;
	}
	public async Task<List<string>> GetEmailsToNotify(Guid caseId)
	{
		var notifiableInCase = await Context.CaseParticipants
			.Where(x => x.CaseId == caseId)
			.Where(c => c.MustNotify)
			.Where(c => c.NotificationEmail != "")
			.Select(c => c.NotificationEmail)
			.ToListAsync();

		return notifiableInCase;
	}
}
