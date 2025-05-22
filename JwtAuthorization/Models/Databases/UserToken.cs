namespace JwtAuthorization.Models.Databases
{
    public class UserToken : ModelBase
    {
        public long Id { get; set; }

        public string Account { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
