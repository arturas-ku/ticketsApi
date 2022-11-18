using System.ComponentModel.DataAnnotations;

namespace SupportAPI.Data.Dtos.Tickets
{
    public class TicketsDto
    {
        public record TicketDto(int Id, string Description, DateTime CreationDate, DateTime UpdateDate, int ProjectId, int TicketTypeId, int TicketStatusId);
        public record CreateTicketDto([Required, MaxLength(500)] string Description, [Required] int TicketTypeId);
        public record UpdateTicketDto([Required, MaxLength(500)] string Description, [Required] int ProjectId, [Required] int TicketStatusId, [Required] int TicketTypeId);
    }
}