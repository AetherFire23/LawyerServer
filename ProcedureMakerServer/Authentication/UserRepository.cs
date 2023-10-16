using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Repository.ProcedureRepo;

namespace ProcedureMakerServer.Authentication;

public class UserRepository : ProcedureEntityRepoBase<User>, IUserRepository
{
    public UserRepository(IMapper mapper, ProcedureContext context) : base(mapper, context)
    {

    }

    public async Task<User> GetUserById(Guid id)
    {
        var user = await Set.FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception("User Id can not be null");
        return user;
    }

    public async Task<User> GetUserByName(string name)
    {
        var user = await Set.FirstOrDefaultAsync(u => u.Name == name) ?? throw new Exception("User Id can not be null");
        return user;
    }
}
