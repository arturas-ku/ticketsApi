using Microsoft.EntityFrameworkCore;
using SupportAPI.Data.Entities;

namespace SupportAPI.Data.Repositories
{
    public interface ITicketStatusesRepository
    {
        Task<TicketStatus?> GetAsync(int projectId);
    }

    public class TicketStatusesRepository : ITicketStatusesRepository
    {
        private readonly SupportDbContext _context;

        public TicketStatusesRepository(SupportDbContext context)
        {
            _context = context;
        }

        public async Task<TicketStatus?> GetAsync(int ticketStatusId)
        {
            return await _context.TicketStatuses.FirstOrDefaultAsync(t => t.Id == ticketStatusId);
        }
    }
}
