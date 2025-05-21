namespace JwtAuthorization.Models.DTO.Request
{
    public class UserLoginRequest
    {
        public string Account { get; set; } = string.Empty;

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
