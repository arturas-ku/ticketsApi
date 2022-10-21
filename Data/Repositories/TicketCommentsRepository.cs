using Microsoft.EntityFrameworkCore;
using SupportAPI.Data.Dtos.Projects;
using SupportAPI.Data.Dtos.TicketComments;
using SupportAPI.Data.Entities;
using SupportAPI.Helpers;

namespace SupportAPI.Data.Repositories
{
    public interface ITicketCommentsRepository
    {
        Task CreateAsync(TicketComment ticketComment);
        Task DeleteAsync(TicketComment ticketComment);
        Task<TicketComment?> GetAsync(int ticketCommentId, int ticketId);
        Task<PagedList<TicketComment>> GetAllAsync(int ticketId, TicketCommentsSearchParameters searchParameters);
        Task PutAsync(TicketComment ticketComment);
    }

    public class TicketCommentsRepository : ITicketCommentsRepository
    {
        private readonly SupportDbContext _context;

        public TicketCommentsRepository(SupportDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<TicketComment>> GetAllAsync(int ticketId, TicketCommentsSearchParameters searchParameters)
        {
            var queryable = _context.TicketComments
                .Where(o => o.TicketId == ticketId)
                .AsQueryable()
                .OrderBy(o => o.CreationDate);

            return await PagedList<TicketComment>
                .CreateAsync(queryable, searchParameters.PageNumber, searchParameters.PageSize);
        }

        public async Task<TicketComment?> GetAsync(int ticketCommentId, int ticketId)
        {
            return await _context.TicketComments
                .FirstOrDefaultAsync(o => o.Id == ticketCommentId && o.TicketId == ticketId);
        }

        public async Task CreateAsync(TicketComment ticketComment)
        {
            ticketComment.CreationDate = DateTime.UtcNow;
            ticketComment.UpdateDate = DateTime.UtcNow;

            _context.Add(ticketComment);
            await _context.SaveChangesAsync();
        }

        public async Task PutAsync(TicketComment ticketComment)
        {
            ticketComment.UpdateDate = DateTime.UtcNow;

            _context.Update(ticketComment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TicketComment ticketComment)
        {
            _context.Remove(ticketComment);
            await _context.SaveChangesAsync();
        }
    }
}
