using System.ComponentModel.DataAnnotations;

namespace JwtAuthorization.Models.DTO.Request
{
    public class TokenRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
