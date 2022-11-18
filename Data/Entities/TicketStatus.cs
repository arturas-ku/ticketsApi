using Microsoft.AspNetCore.Identity;
using SupportAPI.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace SupportAPI.Data.Entities
{
    public class TicketStatus : BaseEntity, IUserOwnedResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
