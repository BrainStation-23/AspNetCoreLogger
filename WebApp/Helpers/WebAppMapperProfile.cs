using AutoMapper;
using WebApp.ViewModels;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp.Helpers
{
    public class WebAppMapperProfile : Profile
    {
        public WebAppMapperProfile()
        {
            CreateMap<User, UserVm>().ReverseMap();

            CreateMap<Role, RoleVm>().ReverseMap();
        }
    }
}
