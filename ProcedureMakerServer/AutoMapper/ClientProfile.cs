using AutoMapper;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.AutoMapper;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        _ = CreateMap<Client, Client>();
    }
}
