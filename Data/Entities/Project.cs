namespace SupportAPI.Data.Entities
{
    public class Project : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}