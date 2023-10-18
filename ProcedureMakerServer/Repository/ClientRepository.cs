using AutoMapper;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Repository;

public class ClientRepository : ProcedureCrudBase<Client>, IClientRepository
{
    public ClientRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task ModifyClient(Client client)
    {
        var entity = await GetEntityById(client.Id);

        entity.FirstName = client.FirstName;
        //Mapper.Map(client, entity);

        await Context.SaveChangesAsync();
    }
}
