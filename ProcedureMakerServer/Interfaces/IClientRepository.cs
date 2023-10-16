using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Interfaces;

internal interface IClientRepository : IProcedureRepositoryBase<Client>
{
    Task ModifyClient(Client client);
}