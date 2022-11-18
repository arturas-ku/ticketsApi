using System.ComponentModel.DataAnnotations;

namespace SupportAPI.Data.Dtos.Projects
{
    public class ProjectsDto
    {
        public record ProjectDto(int Id, string Name, DateTime CreationDate, DateTime UpdateDate);
        public record CreateProjectDto([Required, MaxLength(100)] string Name);
        public record UpdateProjectDto([Required, MaxLength(100)] string Name);
    }
}
