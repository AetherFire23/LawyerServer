
using AutoMapper;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.AutoMapper;


public class LawyerProfile : Profile
{
    public LawyerProfile()
    {
        // if same type and same name, 
        _ = CreateMap<Lawyer, Lawyer>();
    }
}
