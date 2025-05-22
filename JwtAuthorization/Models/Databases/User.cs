namespace JwtAuthorization.Models.Databases
{
    public class User : ModelBase
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
