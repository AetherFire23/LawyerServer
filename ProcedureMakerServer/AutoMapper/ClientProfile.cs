using AutoMapper;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.AutoMapper;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, Client>()
            .ForMember(x => x.Id, y => y.Ignore());
    }
}
