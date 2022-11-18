using Microsoft.AspNetCore.Identity;
using SupportAPI.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace SupportAPI.Data.Entities
{
    public class TicketComment : BaseEntity, IUserOwnedResource
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
