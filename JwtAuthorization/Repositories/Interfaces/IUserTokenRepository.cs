using JwtAuthorization.Models.Databases;

namespace JwtAuthorization.Repositories.Interfaces
{
    public interface IUserTokenRepository
    {
        UserToken GetUserToken(string refreshToken);
    }
}
