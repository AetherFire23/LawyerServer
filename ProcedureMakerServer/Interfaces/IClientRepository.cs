using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Interfaces;

public interface IClientRepository : IProcedureCrudRepositoryBase<Client>
{
	Task UpdateClient(Client client);
}