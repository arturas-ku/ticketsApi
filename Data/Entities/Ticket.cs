﻿namespace SupportAPI.Data.Entities
{
    public class Ticket : BaseEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }
        public ICollection<TicketComment> TicketComments { get; set; }
    }
}