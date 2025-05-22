using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthorization.Models.Configuration;
using JwtAuthorization.Models.Databases;
using JwtAuthorization.Models.DTO.Request;
using JwtAuthorization.Repositories.Interfaces;
using JwtAuthorization.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthorization.Services.Implement
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public JwtService(IOptionsMonitor<JwtConfig> optionsMonitor, TokenValidationParameters tokenValidationParameters,
            IUserRepository userRepository, IUserTokenRepository userTokenRepository, IUserRoleRepository userRoleRepository)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 產生JWT Token
        /// </summary>
        /// <param name="user">User資料</param>
        /// <returns>AuthResult</returns>
        public async Task<AuthResult> GenerateJwtToken(User user, List<UserRole> userRoles)
        {
            #region 建立JWT Token
            //宣告JwtSecurityTokenHandler，用來建立token
            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

            //appsettings中JwtConfig的Secret值
            byte[] key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Iss, user.Account));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            for (int index = 0; index < userRoles.Count; index++)
            {
                claims.Add(new Claim("roles", ((Role)userRoles[index].RoleId).ToString()));
            }

            //建立JWT securityToken
            var token = new JwtSecurityToken(
                //issuer: _jwtSettings.Issuer,
                //audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtConfig.ExpirationMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            );

            //token序列化為字串
            string jwtToken = jwtTokenHandler.WriteToken(token);
            #endregion

            #region 新增UserToken資料表
            //建立更新物件
            UserToken userToken = new UserToken()
            {
                //帳號
                Account = token.Issuer,
                //Token
                Token = jwtToken,
                //RefreshToken
                RefreshToken = Guid.NewGuid().ToString()
            };

            ////新增至資料表
            //await _context.UserTokens.AddAsync(userToken);
            //await _context.SaveChangesAsync();
            #endregion

            #region 回傳AuthResult
            return new AuthResult()
            {
                Token = jwtToken,
                Result = true,
                RefreshToken = userToken.RefreshToken
            };
            #endregion
        }

        /// <summary>
        /// 驗證Token
        /// </summary>
        /// <param name="tokenRequest">TokenRequest參數</param>
        /// <returns>bool, account</returns>
        public bool VerifyToken(TokenRequest tokenRequest, out string account)
        {
            account = string.Empty;
            //建立JwtSecurityTokenHandler
            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                //驗證參數的Token，回傳SecurityToken
                ClaimsPrincipal tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out SecurityToken validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    //檢核Token的演算法
                    //var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature);

                    if (result == false)
                    {
                        return false;
                    }
                }

                //依參數的RefreshToken，查詢UserToken資料表中的資料
                UserToken storedRefreshToken = _userTokenRepository.GetUserToken(refreshToken: tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return false;
                }

                account = storedRefreshToken.Account;

                //取Token Claims中的Iss(產生token時定義為Account)
                string JwtAccount = tokenInVerification.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Iss).Value;

                //檢核storedRefreshToken與JwtAccount的Account是否一致
                if (storedRefreshToken.Account != JwtAccount)
                {
                    return false;
                }

                //產生Jwt Token
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 驗證Token，並重新產生Token
        /// </summary>
        /// <param name="tokenRequest">TokenRequest參數</param>
        /// <returns>AuthResult</returns>
        public async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            //建立JwtSecurityTokenHandler
            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                this.VerifyToken(tokenRequest, out string tokenAccount);

                //依storedRefreshToken的Account，查詢出DB的User資料
                User dbUser = _userRepository.GetUser(account: tokenAccount);
                List<UserRole> dbUserRole = _userRoleRepository.GetUserRoles(userId: dbUser.Id);

                //產生Jwt Token
                return await GenerateJwtToken(user: dbUser, userRoles: dbUserRole);
            }
            catch (Exception ex)
            {
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>() {
                ex.Message
            }
                };
            }
        }
    }
}
