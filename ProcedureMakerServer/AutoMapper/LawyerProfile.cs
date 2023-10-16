
using AutoMapper;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.AutoMapper;


public class LawyerProfile : Profile
{
    public LawyerProfile()
    {
        // if same type and same name, 
        CreateMap<Lawyer, Lawyer>()
            .ForMember(x => x.Id, y => y.Ignore());
    }
}
