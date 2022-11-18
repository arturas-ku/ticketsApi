using AutoMapper;
using SupportAPI.Auth.Model;
using SupportAPI.Data.Entities;
using static SupportAPI.Data.Dtos.Projects.ProjectsDto;
using static SupportAPI.Data.Dtos.TicketComments.TicketCommentsDto;
using static SupportAPI.Data.Dtos.Tickets.TicketsDto;

namespace SupportAPI.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<CreateProjectDto, Project>()
                .ForMember(dest =>
                    dest.UserId,
                    opt => opt.MapFrom<IdentityResolver>());
            CreateMap<UpdateProjectDto, Project>()
                .ForMember(dest =>
                    dest.UserId,
                    opt => opt.MapFrom<IdentityResolver>());

            CreateMap<Ticket, TicketDto>();
            CreateMap<CreateTicketDto, Ticket>()
                .ForMember(dest =>
                    dest.UserId,
                    opt => opt.MapFrom<IdentityResolver>());

            CreateMap<UpdateTicketDto, Ticket>()
                .ForMember(dest =>
                    dest.UserId,
                    opt => opt.MapFrom<IdentityResolver>());

            CreateMap<TicketComment, TicketCommentDto>();
            CreateMap<CreateTicketCommentDto, TicketComment>()
                .ForMember(dest =>
                    dest.UserId,
                    opt => opt.MapFrom<IdentityResolver>());

            CreateMap<UpdateTicketCommentDto, TicketComment>()
                .ForMember(dest =>
                    dest.UserId,
                    opt => opt.MapFrom<IdentityResolver>());

            CreateMap<RegisterUserDto, AppUser>()
                .ForMember(dest =>
                    dest.UserName,
                    opt => opt.MapFrom(src => src.Email));
        }
    }
}
