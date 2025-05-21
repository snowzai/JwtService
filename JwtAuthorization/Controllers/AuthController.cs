using JwtAuthorization.Models.Configuration;
using JwtAuthorization.Models.Databases;
using JwtAuthorization.Models.DTO.Request;
using JwtAuthorization.Models.DTO.Response;
using JwtAuthorization.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthorization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <summary>
        /// Login登入，驗證帳號密碼後，回傳token
        /// </summary>
        /// <param name="user">User資料</param>
        /// <returns>AuthResult</returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            //驗證Model Binding是否成功
            if (ModelState.IsValid)
            {
                //檢核帳號密碼是否正確
                User isCorrectUser = _jwtService.GetUsers()
                    .Where(x => x.Password == user.Password & x.Account == user.Account)
                    .FirstOrDefault();

                //若查無帳號資訊，則表示帳號密碼錯誤
                if (isCorrectUser == null)
                {
                    //回傳資訊
                    return BadRequest(new UserLoginResponse()
                    {
                        Result = false,
                        Errors = new List<string>(){
                            "請確認帳號密碼是否正確。"
                        }
                    });
                }

                //呼叫GenerateJwtToken方法，建立jwtToken
                AuthResult jwtToken = await _jwtService.GenerateJwtToken(isCorrectUser);

                //回傳AuthResult
                return Ok(jwtToken);
            }
            return BadRequest(new UserLoginResponse()
            {
                Result = false,
                Errors = new List<string>(){
                    "無效的Payload"
                }
            });
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="tokenRequest">TokenRequest參數</param>
        /// <returns>AuthResult</returns>
        [HttpPost]
        [Authorize]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                AuthResult result = await _jwtService.VerifyAndGenerateToken(tokenRequest);

                if (result == null)
                {
                    return BadRequest(new UserLoginResponse()
                    {
                        Errors = new List<string>() {
                            "無效的token"
                        },
                        Result = false
                    });
                }

                return Ok(result);
            }

            return BadRequest(new UserLoginResponse()
            {
                Errors = new List<string>() {
                    "無效的payload"
                },
                Result = false
            });
        }
    }
}
