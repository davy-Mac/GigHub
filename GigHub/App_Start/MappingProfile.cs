using AutoMapper;
using GigHub.Controllers.Api;
using GigHub.Core.Dtos;
using GigHub.Core.Models;

namespace GigHub.App_Start
{
    public class MappingProfile : Profile
    {
        // WE NEED TO TELL AUTOMAPPER WE HAVE A PROFILE BY REFERENCING IT IN Global.asax.cs
        public MappingProfile()
        {
            Mapper.CreateMap<ApplicationUser, UserDto>();
            Mapper.CreateMap<Gig, GigDto>();
            Mapper.CreateMap<Notification, NotificationDto>();
        }
    }
}
