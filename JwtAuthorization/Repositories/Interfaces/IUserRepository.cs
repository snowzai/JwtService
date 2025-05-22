using JwtAuthorization.Models.Databases;

namespace JwtAuthorization.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetUsers();

        User GetUser(string account);

        User GetUser(string account, string password);
    }
}
