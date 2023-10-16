//using AutoMapper;
//using EFCoreBase.Interfaces;
//using JWTAuth.Entities;
//using Microsoft.EntityFrameworkCore;
//using ProcedureMakerServer.Authentication;

//namespace ProcedureMakerServer.Repository;

//public class UserRepository : ProcedureRepositoryBase<User>, IUserRepository
//{
//    public UserRepository(ProcedureContext context, IMapper mapper) : base(context, mapper)
//    {
//    }

//    public async Task<User> GetUserById(Guid id)
//    {
//        var user = await Set.FirstAsync(x => x.Id == id);
        
//        return user;
//    }
//}
