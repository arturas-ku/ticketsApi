using Microsoft.EntityFrameworkCore;
using SupportAPI.Data.Dtos.Tickets;
using SupportAPI.Data.Entities;
using SupportAPI.Helpers;

namespace SupportAPI.Data.Repositories
{
    public interface ITicketsRepository
    {
        Task CreateAsync(Ticket ticket);
        Task DeleteAsync(Ticket ticket);
        Task<Ticket?> GetAsync(int projectId, int ticketId);
        Task<PagedList<Ticket>> GetAllAsync(int projectId, TicketSearchParameters searchParameters);
        Task<PagedList<Ticket>> GetAllAsync(TicketSearchParameters searchParameters);
        Task PutAsync(Ticket ticket);
    }

    public class TicketsRepository : ITicketsRepository
    {
        private readonly DbContext _context;

        public TicketsRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Ticket>> GetAllAsync(TicketSearchParameters searchParameters)
        {
            var queryable = _context.Tickets.AsQueryable().OrderBy(o => o.CreationDate);

            return await PagedList<Ticket>
                .CreateAsync(queryable, searchParameters.PageNumber, searchParameters.PageSize);
        }

        public async Task<PagedList<Ticket>> GetAllAsync(int projectId, TicketSearchParameters searchParameters)
        {
            var queryable = _context.Tickets
                .Where(o => o.ProjectId == projectId)
                .AsQueryable()
                .OrderBy(o => o.CreationDate);

            return await PagedList<Ticket>
                .CreateAsync(queryable, searchParameters.PageNumber, searchParameters.PageSize);
        }

        public async Task<Ticket?> GetAsync(int projectId, int ticketId)
        {
            return await _context.Tickets.FirstOrDefaultAsync(o => o.Id == ticketId && o.ProjectId == projectId);
        }

        public async Task CreateAsync(Ticket ticket)
        {
            ticket.CreationDate = DateTime.UtcNow;
            ticket.UpdateDate = DateTime.UtcNow;

            _context.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task PutAsync(Ticket ticket)
        {
            ticket.UpdateDate = DateTime.UtcNow;

            _context.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Ticket ticket)
        {
            _context.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
