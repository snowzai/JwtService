namespace JwtAuthorization.Models.Databases
{
    public class UserRole : ModelBase
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public int RoleId { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }
    }

    public enum Role
    {
        User = 1,

        Admin = 2,

        SuperUser = 99
    }
}
