namespace HotelManagementSystem.Model.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string CreatedBy { get; set; }
    }
}
