using JwtAuthorization.Models.Databases;

namespace JwtAuthorization.Repositories.Interfaces
{
    public interface IUserTokenRepository
    {
        List<UserToken> GetUserToken();

        UserToken GetUserToken(string refreshToken);
    }
}
