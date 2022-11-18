using System.ComponentModel.DataAnnotations;

namespace SupportAPI.Data.Dtos.TicketComments
{
    public class TicketCommentsDto
    {
        public record TicketCommentDto(int Id, string Content, DateTime CreationDate, DateTime UpdateDate, int TicketId);
        public record CreateTicketCommentDto([Required, MaxLength(500)] string Content);
        public record UpdateTicketCommentDto([Required, MaxLength(500)] string Content);
    }
}
