using Microsoft.EntityFrameworkCore;
using SupportAPI.Data.Dtos.Projects;
using SupportAPI.Data.Entities;
using SupportAPI.Helpers;

namespace SupportAPI.Data.Repositories
{
    public interface IProjectsRepository
    {
        Task CreateAsync(Project project);
        Task DeleteAsync(Project project);
        Task<Project?> GetAsync(int projectId);
        Task<PagedList<Project>> GetAllAsync(ProjectSearchParameters searchParameters);
        Task PutAsync(Project project);
    }

    public class ProjectsRepository : IProjectsRepository
    {
        private readonly SupportDbContext _context;

        public ProjectsRepository(SupportDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Project>> GetAllAsync(ProjectSearchParameters searchParameters)
        {
            var queryable = _context.Projects.AsQueryable().OrderBy(o => o.Name);

            return await PagedList<Project>.CreateAsync(queryable, searchParameters.PageNumber, searchParameters.PageSize);
        }

        public async Task<Project?> GetAsync(int projectId)
        {
            return await _context.Projects.FirstOrDefaultAsync(t => t.Id == projectId);
        }

        public async Task CreateAsync(Project project)
        {
            project.CreationDate = DateTime.UtcNow;
            project.UpdateDate = DateTime.UtcNow;

            _context.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task PutAsync(Project project)
        {
            project.UpdateDate = DateTime.UtcNow;

            _context.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
