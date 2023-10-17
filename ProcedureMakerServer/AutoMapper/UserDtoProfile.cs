using AutoMapper;
using ProcedureMakerServer.Authentication;

namespace ProcedureMakerServer.AutoMapper;

public class UserDtoProfile : Profile
{
    public UserDtoProfile()
    {
        CreateMap<User, UserDto>();
    }
}