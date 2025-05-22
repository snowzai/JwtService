namespace JwtAuthorization.Models.Databases
{
    public class ModelBase
    {
        public DateTime CreatedAt { get; set; }

        public long CreatedUserId { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public long? LastUpdatedUserId { get; set; }
    }
}
