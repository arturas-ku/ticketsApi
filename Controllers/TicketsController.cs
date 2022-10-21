using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SupportAPI.Data;
using SupportAPI.Data.Dtos.Tickets;
using SupportAPI.Data.Entities;
using SupportAPI.Data.Repositories;
using static SupportAPI.Data.Dtos.Tickets.TicketsDto;

namespace SupportAPI.Controllers
{
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketsRepository _ticketsRepo;
        private readonly IProjectsRepository _projectsRepo;
        private readonly ITicketTypesRepository _ticketTypesRepo;
        private readonly ITicketStatusesRepository _ticketStatusesRepo;
        private readonly IMapper _mapper;

        private const string URI_DEFAULT = "api/tickets";
        private const string URI_PROJECT_NESTED_DEFAULT = "api/projects/{projectId}/tickets";
        private const string URI_PROJECT_NESTED_SPECIFIC = "api/projects/{projectId}/tickets/{ticketId}";

        public TicketsController(
            IMapper mapper,
            ITicketsRepository ticketsRepository, 
            IProjectsRepository projectsRepository, 
            ITicketTypesRepository ticketTypesRepo,
            ITicketStatusesRepository ticketStatusesRepo)
        {
            _mapper = mapper;
            _ticketsRepo = ticketsRepository;
            _projectsRepo = projectsRepository;
            _ticketTypesRepo = ticketTypesRepo;
            _ticketStatusesRepo = ticketStatusesRepo;
        }

        [HttpGet(URI_DEFAULT, Name = "GetAllTickets")]
        public async Task<IEnumerable<TicketDto>> GetAll([FromQuery] TicketSearchParameters searchParameters)
        {
            var tickets = await _ticketsRepo.GetAllAsync(searchParameters);

            var previousPageLink = tickets.HasPrevious ?
                CreateProjectsResourceUri(searchParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = tickets.HasNext ?
                CreateProjectsResourceUri(searchParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetaData = new
            {
                totalCount = tickets.TotalCount,
                pageSize = tickets.PageSize,
                currentPage = tickets.CurrentPage,
                totalPages = tickets.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationMetaData));

            return tickets.Select(o => _mapper.Map<TicketDto>(o));
        }

        [HttpGet(URI_PROJECT_NESTED_DEFAULT)]
        public async Task<IEnumerable<TicketDto>> GetAll(int projectId, [FromQuery] TicketSearchParameters searchParameters)
        {
            var tickets = await _ticketsRepo.GetAllAsync(searchParameters);

            var previousPageLink = tickets.HasPrevious ?
                CreateProjectsResourceUri(searchParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = tickets.HasNext ?
                CreateProjectsResourceUri(searchParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetaData = new
            {
                totalCount = tickets.TotalCount,
                pageSize = tickets.PageSize,
                currentPage = tickets.CurrentPage,
                totalPages = tickets.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationMetaData));

            return tickets.Select(o => _mapper.Map<TicketDto>(o));
        }

        [HttpGet(URI_PROJECT_NESTED_SPECIFIC)]
        public async Task<ActionResult<TicketDto>> Get(int projectId, int ticketId)
        {
            var ticket = await _ticketsRepo.GetAsync(projectId, ticketId);

            if (ticket == null) 
                return NotFound();

            return _mapper.Map<TicketDto>(ticket);
        }

        [HttpPost(URI_PROJECT_NESTED_DEFAULT)]
        public async Task<ActionResult<TicketDto>> Create(int projectId, CreateTicketDto ticketDto)
        {
            var project = await _projectsRepo.GetAsync(projectId);

            if (project == null) 
                return NotFound();

            var ticketType = await _ticketTypesRepo.GetAsync(ticketDto.TicketTypeId);

            if (ticketType == null)
                return NotFound();

            var ticket = _mapper.Map<Ticket>(ticketDto);
            ticket.ProjectId = projectId;
            ticket.TicketStatusId = 1;
            await _ticketsRepo.CreateAsync(ticket);

            return Created(URI_PROJECT_NESTED_SPECIFIC, _mapper.Map<TicketDto>(ticket));
        }

        [HttpPut(URI_PROJECT_NESTED_SPECIFIC)]
        public async Task<ActionResult<TicketDto>> Update(int ticketId, int projectId, UpdateTicketDto updateTicketDto)
        {
            var project = await _projectsRepo.GetAsync(updateTicketDto.ProjectId);

            if (project == null) 
                return NotFound();

            var ticket = await _ticketsRepo.GetAsync(projectId, ticketId);

            if (ticket == null)
                return NotFound();

            var ticketType = await _ticketTypesRepo.GetAsync(updateTicketDto.TicketTypeId);

            if (ticketType == null)
                return NotFound();

            var ticketStatus = await _ticketStatusesRepo.GetAsync(updateTicketDto.TicketStatusId);

            if (ticketStatus == null)
                return NotFound();

            _mapper.Map(updateTicketDto, ticket);
            await _ticketsRepo.PutAsync(ticket);

            return Ok(_mapper.Map<TicketDto>(ticket));
        }

        [HttpDelete(URI_PROJECT_NESTED_SPECIFIC)]
        public async Task<ActionResult> Delete(int projectId, int ticketId)
        {
            var ticket = await _ticketsRepo.GetAsync(projectId, ticketId);

            if (ticket == null) 
                return NotFound();

            await _ticketsRepo.DeleteAsync(ticket);

            return NoContent();
        }

        private string? CreateProjectsResourceUri(
            TicketSearchParameters projectSearchParameterDto,
            ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => Url.Link("GetAllTickets", new
                {
                    pageNumber = projectSearchParameterDto.PageNumber - 1,
                    pageSize = projectSearchParameterDto.PageSize,
                }),
                ResourceUriType.NextPage => Url.Link("GetAllTickets",
                new
                {
                    pageNumber = projectSearchParameterDto.PageNumber + 1,
                    pageSize = projectSearchParameterDto.PageSize,
                }),
                _ => Url.Link("GetAllTickets",
                new
                {
                    pageNumber = projectSearchParameterDto.PageNumber,
                    pageSize = projectSearchParameterDto.PageSize,
                })
            };
        }
    }
}
