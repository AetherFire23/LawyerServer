using EFCoreBase.Entities;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Entities;
using ProcedureShared.Dtos;
namespace ProcedureMakerServer.Repository;

public class CaseContextRepository
{
	private readonly CaseRepository _caseRepository;
	private readonly LawyerRepository _lawyerRepository;
	private readonly ProcedureContext _context;
	private readonly UserRepository _userRepository;
	private readonly ClientRepository _clientRepository;
	public CaseContextRepository(
		CaseRepository caseRepository,
		LawyerRepository lawyerRepository,
		ProcedureContext context,
		ClientRepository clientRepository,
		UserRepository userRepository)
	{
		_caseRepository = caseRepository;
		_lawyerRepository = lawyerRepository;
		_context = context;
		_userRepository = userRepository;
		_clientRepository = clientRepository;
	}

	public async Task<CaseContextDto> MapCasesContext(Guid lawyerId)
	{
		var lawyer = await _lawyerRepository.GetEntityById(lawyerId);
		var lawyerDto = await _lawyerRepository.MapLawyerDto(lawyerId);
		var clientDtos = await _clientRepository.MapClientDtos(lawyer.Clients.ToList());
		var userDto = await _userRepository.MapUserDto(lawyer.UserId);

		var context = new CaseContextDto
		{
			Lawyer = lawyerDto,
			Clients = clientDtos,
			User = userDto,

		};

		return context;
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
