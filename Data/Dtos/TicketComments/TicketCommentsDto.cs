using System.ComponentModel.DataAnnotations;

namespace SupportAPI.Data.Dtos.TicketComments
{
    public class TicketCommentsDto
    {
        public record TicketCommentDto(int Id, string Content, DateTime CreationDate, DateTime UpdateDate);
        public record CreateTicketCommentDto([Required] string Content);
        public record UpdateTicketCommentDto([Required] string Content);
    }
}
