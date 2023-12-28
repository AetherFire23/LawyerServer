using EFCoreBase.Entities;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
namespace ProcedureMakerServer.Repository;

public class CasesContextRepository
{
	private readonly CaseRepository _caseRepository;
	private readonly LawyerRepository _lawyerRepository;
	private readonly ProcedureContext _context;
	private readonly ClientRepository _clientRepository;
	private readonly UserRepository _userRepository;
	public CasesContextRepository(
		CaseRepository caseRepository,
		LawyerRepository lawyerRepository,
		ProcedureContext context,
		ClientRepository clientRepository,
		UserRepository userRepository)
	{
		_caseRepository = caseRepository;
		_lawyerRepository = lawyerRepository;
		_context = context;
		_clientRepository = clientRepository;
		_userRepository = userRepository;
	}

	public async Task<CaseContextDto> MapCasesContext(Guid lawyerId)
	{
		var lawyer = await _lawyerRepository.GetEntityById(lawyerId);
		var cases = await MapCasesDtos(lawyer);
		var lawyerDto = await _lawyerRepository.MapLawyerDto(lawyerId);
		var clientDtos = await _clientRepository.MapClientDtos(lawyer.Clients.ToList());
		var userDto = await _userRepository.MapUserDto(lawyer.UserId);

		var context = new CaseContextDto
		{
			Cases = cases,
			Lawyer = lawyerDto,
			Clients = clientDtos,
			User = userDto,
		};

		return context;
	}

	private async Task<List<CaseDto>> MapCasesDtos(Lawyer lawyer)
	{
		var cases = new List<CaseDto>();
		foreach (Case c in lawyer.Cases)
		{
			cases.Add(await _caseRepository.MapCaseDto(c.Id));
		}
		return cases;
	}

	public async Task AddCaseParticipant(Guid caseId, CaseParticipantDto updatedCaseParticipantDto)
	{
		var lcase = await _context.Cases.FirstAsync(c => c.Id == caseId);

		var caseParticipant = new CaseParticipant()
		{
			Id = updatedCaseParticipantDto.GenerateIdIfNull(),
			Case = lcase,
		};

		caseParticipant.CopyFromCourtMember(updatedCaseParticipantDto);

		_context.CaseParticipants.Add(caseParticipant);

		await _context.SaveChangesAsync();
	}

	public async Task UpdateCaseParticipant(CaseParticipantDto updatedCaseParticipant)
	{
		//var caseParticipant = await _context.CaseParticipants.FirstByIdAsync(updatedCaseParticipant.Id);
		var caseParticipant = await _context.CaseParticipants.FirstAsync(x => x.Id == updatedCaseParticipant.Id);

		caseParticipant.CopyFromCourtMember(caseParticipant);
		await _context.SaveChangesAsync();
	}

	public async Task RemoveCaseParticipant(Guid caseParticipantId)
	{
		var caseParticipant = await _context.CaseParticipants.FirstByIdAsync(caseParticipantId);

		_context.CaseParticipants.Remove(caseParticipant);
		await _context.SaveChangesAsync();
	}
}
