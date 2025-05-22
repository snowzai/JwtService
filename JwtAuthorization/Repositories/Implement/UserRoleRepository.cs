using JwtAuthorization.Models.Databases;
using JwtAuthorization.Repositories.Interfaces;

namespace JwtAuthorization.Repositories.Implement
{
    public class UserRoleRepository : IUserRoleRepository
    {
        public UserRoleRepository()
        {

        }

        public List<UserRole> GetUserRoles(long userId)
        {
            return AllRoles().Where(r => r.UserId == userId).ToList();
        }

        //fake data
        private List<UserRole> AllRoles()
        {
            List<UserRole> roles = new List<UserRole>
            {
                new UserRole
                {
                    Id = 1,
                    UserId = 1,
                    RoleId = 1,
                    IsAdmin = false,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedUserId = 1
                },
                new UserRole
                {
                    Id = 2,
                    UserId = 1,
                    RoleId = 2,
                    IsAdmin = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedUserId = 1
                }
            };

            return roles;
        }
    }
}
