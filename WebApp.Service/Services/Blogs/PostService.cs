using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Collections;
using WebApp.Entity.Entities.Blogs;
using WebApp.Service.Contract.Models.Blogs;
using WebApp.Services;

namespace WebApp.Service
{
    public class PostService : BaseService<PostEntity, PostModel>, IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork,
                IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Paging<PostModel>> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var data = await _unitOfWork.Repository<PostEntity>().GetPageAsync(pageIndex,
                pageSize,
                s => (string.IsNullOrEmpty(searchText) || s.Title.Contains(searchText) || s.ShortDescription.Contains(searchText) || s.Description.Contains(searchText)),
                o => o.OrderBy(ob => ob.Id),
                se => se);

            var response = data.ToPagingModel<PostEntity, PostModel>(_mapper);

            return response;
        }
    }
}
