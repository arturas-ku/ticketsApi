using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SupportAPI.Data;
using SupportAPI.Data.Dtos.Projects;
using SupportAPI.Data.Entities;
using SupportAPI.Data.Repositories;
using static SupportAPI.Data.Dtos.Projects.ProjectsDto;
using UpdateProjectDto = SupportAPI.Data.Dtos.Projects.ProjectsDto.UpdateProjectDto;

namespace SupportAPI.Controllers
{

    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsRepository _projectsRepo;
        private readonly IMapper _mapper;

        public ProjectsController(IProjectsRepository projectsRepository, IMapper mapper)
        {
            _projectsRepo = projectsRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetProjects")]
        public async Task<IEnumerable<ProjectDto>> GetAll([FromQuery] ProjectSearchParameters searchParameters)
        {
            var projects = await _projectsRepo.GetAllAsync(searchParameters);

            var previousPageLink = projects.HasPrevious ?
                CreateProjectsResourceUri(searchParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = projects.HasNext ?
                CreateProjectsResourceUri(searchParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetaData = new
            {
                totalCount = projects.TotalCount,
                pageSize = projects.PageSize,
                currentPage = projects.CurrentPage,
                totalPages = projects.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationMetaData));

            return projects.Select(o => _mapper.Map<ProjectDto>(o));
        }

        [HttpGet("{id}", Name = "GetProject")]
        public async Task<IActionResult> Get(int id)
        {
            var project = await _projectsRepo.GetAsync(id);

            // 404
            if (project == null)
                return NotFound();

            var projectDto = _mapper.Map<ProjectDto>(project);
            var links = CreateLinksForProject(id);

            // 200
            return Ok(new
            {
                resource = projectDto,
                _links = links
            });
        }

        [HttpPost(Name = "CreateProject")]
        public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);

            await _projectsRepo.CreateAsync(project);

            // 201
            return Created($"/api/projects/{project.Id}", _mapper.Map<ProjectDto>(project));
        }

        [HttpPut("{id}", Name = "UpdateProject")]
        public async Task<ActionResult<ProjectDto>> Put(int id, UpdateProjectDto projectDto)
        {
            var project = await _projectsRepo.GetAsync(id);

            // 404
            if (project == null)
                return NotFound();

            _mapper.Map(projectDto, project);
            await _projectsRepo.PutAsync(project);

            // 200
            return Ok(_mapper.Map<ProjectDto>(project));
        }

        [HttpDelete("{id}", Name = "DeleteProject")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectsRepo.GetAsync(id);

            // 404
            if (project == null)
                return NotFound();

            await _projectsRepo.DeleteAsync(project);

            // 204
            return NoContent();
        }

        private string? CreateProjectsResourceUri(
            ProjectSearchParameters projectSearchParameterDto,
            ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => Url.Link("GetProjects", new
                {
                    pageNumber = projectSearchParameterDto.PageNumber - 1,
                    pageSize = projectSearchParameterDto.PageSize,
                }),
                ResourceUriType.NextPage => Url.Link("GetProjects",
                new
                {
                    pageNumber = projectSearchParameterDto.PageNumber + 1,
                    pageSize = projectSearchParameterDto.PageSize,
                }),
                _ => Url.Link("GetProjects",
                new
                {
                    pageNumber = projectSearchParameterDto.PageNumber,
                    pageSize = projectSearchParameterDto.PageSize,
                })
            };
        }

        private IDictionary<string, LinkDto> CreateLinksForProject(int projectId)
        {
            var dictionary = new Dictionary<string, LinkDto>()
            {
                {"self", new LinkDto { Href = Url.Link("GetProject", new { id = projectId }), Method = "GET" }},
                {"create_project",  new LinkDto { Href = Url.Link("GetProject", new { id = projectId }), Method = "GET" }},
                {"update_project", new LinkDto { Href = Url.Link("UpdateProject", new { id = projectId }), Method = "PUT" }},
                {"delete_project",  new LinkDto { Href = Url.Link("DeleteProject", new { id = projectId }), Method = "DELETE" }}
            };

            return dictionary;
        }
    }
}
