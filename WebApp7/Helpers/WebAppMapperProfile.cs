using AutoMapper;
using WebApp7.ViewModels;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp7.Helpers
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
