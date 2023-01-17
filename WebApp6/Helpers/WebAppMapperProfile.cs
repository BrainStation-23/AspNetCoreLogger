using AutoMapper;
using WebApp6.ViewModels;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp6.Helpers
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
