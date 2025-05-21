namespace JwtAuthorization.Models.Configuration
{
    public class JwtConfig
    {
        public string Secret { get; set; }

        public int ExpirationMinutes { get; set; }
    }
}
