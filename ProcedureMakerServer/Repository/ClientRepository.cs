using AutoMapper;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;
using System.Runtime.InteropServices;

namespace ProcedureMakerServer.Repository;

public class ClientRepository : ProcedureCrudBase<Client>, IClientRepository
{
    public ClientRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task ModifyClient(Client client)
    {
        var entity = await GetEntityById(client.Id);

        _mapper.Map(client, entity);

        await Context.SaveChangesAsync();
    }
}
