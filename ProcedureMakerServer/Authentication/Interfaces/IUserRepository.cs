namespace ProcedureMakerServer.Authentication.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserById(Guid id);
    Task<User> GetUserByName(string name);
}
