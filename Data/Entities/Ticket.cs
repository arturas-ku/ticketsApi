using Microsoft.AspNetCore.Identity;
using SupportAPI.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace SupportAPI.Data.Entities
{
    public class Ticket : BaseEntity, IUserOwnedResource
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }
        public int TicketStatusId { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public ICollection<TicketComment> TicketComments { get; set; }

        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
