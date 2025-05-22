using JwtAuthorization.Models.Databases;
using JwtAuthorization.Repositories.Interfaces;

namespace JwtAuthorization.Repositories.Implement
{
    public class UserTokenRepository : IUserTokenRepository
    {
        public UserTokenRepository()
        {

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
                    Account = "fakeacc",
                    Token = "",
                    RefreshToken = ""
                }
            };

            return userTokens;
        }
    }
}
