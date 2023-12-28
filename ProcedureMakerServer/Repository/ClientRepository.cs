using AutoMapper;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Repository.ProcedureRepo;
using ProcedureMakerServer.Trusts;
namespace ProcedureMakerServer.Repository;

public class ClientRepository : ProcedureCrudBase<Client>
{
    private readonly CaseRepository _caseRepository;
    private readonly TrustRepository _trustRepository;
    public ClientRepository(ProcedureContext context, IMapper mapper, CaseRepository caseRepository, TrustRepository trustRepository) : base(context, mapper)
    {
        _caseRepository = caseRepository;
        _trustRepository = trustRepository;
    }

    public async Task CreateClient(Guid lawyerId, ClientDto clientDto)
    {
        var lawyer = await Context.Lawyers.FirstByIdAsync(lawyerId);

        var client = new Client
        {
            Lawyer = lawyer,
        };

        client.CopyFromCourtMember(clientDto);

        Context.Clients.Add(client);
        await Context.SaveChangesAsync();

        var createdTrust = await CreateTrust(client.Id);

        client.TrustClientCard = createdTrust;
        await Context.SaveChangesAsync();


        var regottenClient = await Context.Clients.FirstAsync(x => x.Id == client.Id);
    }

    public async Task UpdateClientInfo(ClientDto client)
    {
        Client entity = await GetEntityById(client.Id);

        entity.CopyFromCourtMember(client);

        await Context.SaveChangesAsync();
    }

    public async Task RemoveClient(Guid clientId)
    {
        var client = await Context.Clients.FirstByIdAsync(clientId);

        Context.Clients.Remove(client);
        await Context.SaveChangesAsync();
    }

    public async Task<List<ClientDto>> MapClientDtos(List<Client> clients)
    {
        var clientDtos = new List<ClientDto>();

        foreach (var client in clients)
        {
            var clientDto = await MapClientDto(client.Id);
            clientDtos.Add(clientDto);
        }
        return clientDtos;
    }
    public async Task<ClientDto> MapClientDto(Guid clientId)
    {
        var client = await Context.Clients
            .Include(x => x.Cases)
            .FirstAsync(x => x.Id == clientId);


        var caseDtos = await _caseRepository.MapCaseDtos(client.Cases.Select(x => x.Id));
        var trustClientCardDto = await _trustRepository.ConstrustTrustClientCard(clientId);
        var clientDto = new ClientDto
        {
            Cases = caseDtos,
            TrustClientCard = trustClientCardDto,
        };
        clientDto.CopyFromCourtMember(client);

        return clientDto;
    }

    /// <summary> trust is created when client is created. Returns the created trust as tracked  </summary>
    private async Task<TrustClientCard> CreateTrust(Guid clientId)
    {
        var client = await Context.Clients.FirstOrDefaultAsync(x => x.Id == clientId);
        var trustCard = new TrustClientCard
        {
            Client = client,
        };

        Context.TrustClientCards.Add(trustCard);
        await Context.SaveChangesAsync();

        return trustCard;
    }
}
