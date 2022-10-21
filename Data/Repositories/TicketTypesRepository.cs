using Microsoft.EntityFrameworkCore;
using SupportAPI.Data.Entities;

namespace SupportAPI.Data.Repositories
{
    public interface ITicketTypesRepository
    {
        Task<TicketType?> GetAsync(int projectId);
    }

    public class TicketTypesRepository : ITicketTypesRepository
    {
        private readonly SupportDbContext _context;

        public TicketTypesRepository(SupportDbContext context)
        {
            _context = context;
        }

        public async Task<TicketType?> GetAsync(int ticketTypeId)
        {
            return await _context.TicketTypes.FirstOrDefaultAsync(t => t.Id == ticketTypeId);
        }
    }
}
