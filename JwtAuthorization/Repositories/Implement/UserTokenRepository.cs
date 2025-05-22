using JwtAuthorization.Models.Databases;
using JwtAuthorization.Repositories.Interfaces;

namespace JwtAuthorization.Repositories.Implement
{
    public class UserTokenRepository : IUserTokenRepository
    {
        public UserTokenRepository()
        {

        }

        public List<UserToken> GetUserToken()
        {
            return AllUserToken();
        }

        public UserToken GetUserToken(string refreshToken)
        {
            return AllUserToken().FirstOrDefault(ut => ut.RefreshToken == refreshToken);
        }

        //fake data
        public List<UserToken> AllUserToken()
        {
            List<UserToken> userTokens = new List<UserToken> {
                new UserToken
                {
                    Id = 1,
                    Account = "snowchoy",
                    Token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzbm93Y2hveSIsImVtYWlsIjoic25vd2xlb25nLndAZ21haWwuY29tIiwicm9sZXMiOlsiVXNlciIsIkFkbWluIl0sImV4cCI6MTc0Nzg4ODg4N30.JrIDfCYzFwRchBFyNC9Mg14o7y84SnSA8JP7uBeMVawMp2YW7TwQciUzG9JQxAFEv8ViB0c85cb6iewC6OvD3Q",
                    RefreshToken = "391c1667-c4b4-4c5f-9abe-c680814d3427"
                },
                new UserToken
                {
                    Id = 2,
                    Account = "abcdefg",
                    Token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJhYmNkZWZnIiwiZW1haWwiOiJzbm93bGVvbmcud0BnbWFpbC5jb20iLCJyb2xlcyI6IlVzZXIiLCJleHAiOjE3NDc4ODkzOTV9.8yZJnJlmlkfaSnpB40QrdVZaTo1PG1dXnb2kOPxxpyhl2ZOARqzH2reqAxnvq-_CyV0RVjls1drIQsdRoZEFWw",
                    RefreshToken = "f87d6e3e-7298-40e1-8550-e08c0ecddd6c"
                }
            };

            return userTokens;
        }
    }
}
