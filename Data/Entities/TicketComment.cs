namespace SupportAPI.Data.Entities
{
    public class TicketComment : BaseEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
