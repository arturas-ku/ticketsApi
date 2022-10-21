using System.ComponentModel.DataAnnotations;

namespace SupportAPI.Data.Dtos.Tickets
{
    public class TicketsDto
    {
        public record TicketDto(int Id, string Description, DateTime CreationDate, DateTime UpdateDate, int ProjectId, int TicketTypeId);
        public record CreateTicketDto([Required] string Description, [Required] int TicketTypeId);
        public record UpdateTicketDto([Required] string Description, [Required] int ProjectId, [Required] int TicketTypeId);
    }
}