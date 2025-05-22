using JwtAuthorization.Models.Databases;

namespace JwtAuthorization.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        List<UserRole> GetUserRoles();

        List<UserRole> GetUserRoles(long userId);
    }
}
