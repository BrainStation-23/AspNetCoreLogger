using AutoMapper;
using WebApp.Common.Sqls;
using WebApp.Entity.Entities.Blogs;
using WebApp.Service.Contract.Models;
using WebApp.Service.Contract.Models.Blogs;
using WebApp.Service.Contract.Models.Users;
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

            //CreateMap<BlogEntity, BlogDto>().ReverseMap();
            //CreateMap<PostEntity, PostDto>().ReverseMap();
        }
    }
}
