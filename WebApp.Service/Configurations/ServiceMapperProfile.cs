using AutoMapper;
using WebApp.Core.Sqls;
using WebApp.Service.Models;
using WebApp.Service.Models.Blogs;
using WebApp.Service.Models.Users;
using WebApp.Entity.Entities.Blogs;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp.Service.Configurations
{
    public class ServiceMapperProfile : Profile
    {
        public ServiceMapperProfile()
        {
            CreateMap<MasterEntity, MasterModel>()
                 .IncludeAllDerived()
                 .ReverseMap();

            CreateMap<User, UserModel>().ReverseMap();

            CreateMap<BlogEntity, BlogModel>().ReverseMap();
            CreateMap<PostEntity, PostModel>().ReverseMap();
        }
    }
}
