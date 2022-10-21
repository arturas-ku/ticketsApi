using AutoMapper;
using SupportAPI.Data.Entities;
using static SupportAPI.Data.Dtos.Projects.ProjectsDto;
using static SupportAPI.Data.Dtos.TicketComments.TicketCommentsDto;
using static SupportAPI.Data.Dtos.Tickets.TicketsDto;

namespace SupportAPI.Data
{
    public class SupportAPIProfile : Profile
    {
        public SupportAPIProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<CreateProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>();

            CreateMap<Ticket, TicketDto>();
            CreateMap<CreateTicketDto, Ticket>();
            CreateMap<UpdateTicketDto, Ticket>();

            CreateMap<TicketComment, TicketCommentDto>();
            CreateMap<CreateTicketCommentDto, TicketComment>();
            CreateMap<UpdateTicketCommentDto, TicketComment>();
        }
    }
}
