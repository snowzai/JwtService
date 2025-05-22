using System.Collections.Generic;
using JwtAuthorization.Models.Databases;
using JwtAuthorization.Repositories.Interfaces;

namespace JwtAuthorization.Repositories.Implement
{
    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {

        }

        public List<User> GetUsers()
        {
            return AllUsers();
        }

        public User GetUser(string account)
        {
            return AllUsers().FirstOrDefault(u => u.Account == account);
        }

        public User GetUser(string account, string password)
        {
            return AllUsers().FirstOrDefault(u => u.Account == account && u.Password == password);
        }

        private List<User> AllUsers()
        {
            //fake data
            List<User> users = new List<User> {
                new User
                {
                    Id = 1,
                    Account = "snowchoy",
                    Password = "pass123",
                    Name = "Snow",
                    Email = "snowleong.w@gmail.com",
                    CreatedAt = DateTime.Parse("2025-05-21"),
                    CreatedUserId = 0
                },
                new User
                {
                    Id = 2,
                    Account = "abcdefg",
                    Password = "pass123",
                    Name = "Snow abcdefg",
                    Email = "snowleong.w@gmail.com",
                    CreatedAt = DateTime.Parse("2025-05-22"),
                    CreatedUserId = 1
                }
            };

            return users;
        }
    }
}
