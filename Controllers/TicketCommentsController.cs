using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SupportAPI.Data.Dtos.Tickets;
using SupportAPI.Data;
using SupportAPI.Data.Entities;
using SupportAPI.Data.Repositories;
using static SupportAPI.Data.Dtos.TicketComments.TicketCommentsDto;
using SupportAPI.Data.Dtos.TicketComments;
using static SupportAPI.Data.Dtos.Tickets.TicketsDto;

namespace SupportAPI.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/tickets/{ticketId}/ticketcomments")]
    public class TicketCommentsController : ControllerBase
    {
        private readonly IProjectsRepository _projectsRepo;
        private readonly ITicketsRepository _ticketsRepo;
        private readonly ITicketCommentsRepository _ticketCommentsRepo;
        private readonly IMapper _mapper;

        public TicketCommentsController(
            IMapper mapper,
            IProjectsRepository projectsRepo,
            ITicketsRepository ticketsRepository,
            ITicketCommentsRepository ticketCommentsRepo)
        {
            _mapper = mapper;
            _projectsRepo = projectsRepo;
            _ticketsRepo = ticketsRepository;
            _ticketCommentsRepo = ticketCommentsRepo;
        }

        [HttpGet(Name = "GetAllTicketComments")]
        public async Task<IEnumerable<TicketCommentDto>> GetAll(int ticketId, [FromQuery] TicketCommentsSearchParameters searchParameters)
        {
            var ticketComments = await _ticketCommentsRepo.GetAllAsync(ticketId, searchParameters);

            var previousPageLink = ticketComments.HasPrevious ?
                CreateProjectsResourceUri(searchParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = ticketComments.HasNext ?
                CreateProjectsResourceUri(searchParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetaData = new
            {
                totalCount = ticketComments.TotalCount,
                pageSize = ticketComments.PageSize,
                currentPage = ticketComments.CurrentPage,
                totalPages = ticketComments.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationMetaData));

            return ticketComments.Select(o => _mapper.Map<TicketCommentDto>(o));
        }

        [HttpPost]
        public async Task<ActionResult<TicketCommentDto>> Create(int projectId, int ticketId, CreateTicketCommentDto ticketCommentDto)
        {
            var project = await _projectsRepo.GetAsync(projectId);

            if (project == null)
                return NotFound();

            var ticket = await _ticketsRepo.GetAsync(projectId, ticketId);

            if (ticket == null)
                return NotFound();

            var ticketComment = _mapper.Map<TicketComment>(ticketCommentDto);
            ticketComment.TicketId = ticketId;
            await _ticketCommentsRepo.CreateAsync(ticketComment);

            return Created($"api/projects/{{projectId}}/tickets/{{ticketId}}/ticketcomments/{ticketComment.Id}", 
                _mapper.Map<TicketCommentDto>(ticketComment));
        }

        [HttpGet("{ticketCommentId}")]
        public async Task<ActionResult<TicketCommentDto>> Get(int ticketCommentId, int ticketId)
        {
            var ticketComment = await _ticketCommentsRepo.GetAsync(ticketCommentId, ticketId);

            if (ticketComment == null)
                return NotFound();

            return _mapper.Map<TicketCommentDto>(ticketComment);
        }

        [HttpPut("{ticketCommentId}")]
        public async Task<ActionResult<TicketCommentDto>> Update(int ticketCommentId, int ticketId, int projectId, UpdateTicketCommentDto updateTicketCommentDto)
        {
            var project = await _projectsRepo.GetAsync(projectId);

            if (project == null)
                return NotFound();

            var ticket = await _ticketsRepo.GetAsync(projectId, ticketId);

            if (ticket == null)
                return NotFound();

            var ticketComment = await _ticketCommentsRepo.GetAsync(ticketCommentId, ticketId);

            if (ticketComment == null)
                return NotFound();

            _mapper.Map(updateTicketCommentDto, ticketComment);
            await _ticketCommentsRepo.PutAsync(ticketComment);

            return Ok(_mapper.Map<TicketCommentDto>(ticketComment));
        }

        [HttpDelete("{ticketCommentId}")]
        public async Task<ActionResult> Delete(int ticketCommentId, int ticketId)
        {
            var ticketComment = await _ticketCommentsRepo.GetAsync(ticketCommentId, ticketId);

            if (ticketComment == null)
                return NotFound();

            await _ticketCommentsRepo.DeleteAsync(ticketComment);

            return NoContent();
        }

        private string? CreateProjectsResourceUri(
            TicketCommentsSearchParameters ticketCommentsSearchParams,
            ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => Url.Link("GetAllTicketComments", new
                {
                    pageNumber = ticketCommentsSearchParams.PageNumber - 1,
                    pageSize = ticketCommentsSearchParams.PageSize,
                }),
                ResourceUriType.NextPage => Url.Link("GetAllTicketComments",
                new
                {
                    pageNumber = ticketCommentsSearchParams.PageNumber + 1,
                    pageSize = ticketCommentsSearchParams.PageSize,
                }),
                _ => Url.Link("GetAllTicketComments",
                new
                {
                    pageNumber = ticketCommentsSearchParams.PageNumber,
                    pageSize = ticketCommentsSearchParams.PageSize,
                })
            };
        }
    }
}
