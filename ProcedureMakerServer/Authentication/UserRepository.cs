using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Authentication;

public class UserRepository : ProcedureEntityRepoBase<User>, IUserRepository
{
    public UserRepository(IMapper mapper, ProcedureContext context) : base(mapper, context)
    {

    }

    public async Task<User> GetUserById(Guid id)
    {
        var user = await Set
            .FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<User> GetUserByName(string name)
    {
        var user = await Set
            .Include(p => p.UserRoles)
            .ThenInclude(p => p.Role)
            .FirstOrDefaultAsync(u => u.Name == name);

        return user;
    }

    public async Task SaveUser(UserDto userDto)
    {

    }

    public async Task<UserDto> MapUserDto(Guid id)
    {
        User? user = await Set.FirstAsync(x => x.Id == id);
        Lawyer lawyer = await Context.Lawyers.FirstAsync(x=> x.UserId == id);
        var userDto = new UserDto()
        {
            Name = user.Name,
            Roles = user.Roles,
            Lawyer = lawyer,
        };

        return userDto;
    }
}
