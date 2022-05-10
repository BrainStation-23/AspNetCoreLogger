using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Collections;
using WebApp.Service.Models.Blogs;
using WebApp.Services;
using WebApp.Sql.Entities.Blogs;

namespace WebApp.Service
{
    public class BlogService : BaseService<BlogEntity>, IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogService(IUnitOfWork unitOfWork,
                IMapper mapper) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Paging<BlogModel>> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var data = await _unitOfWork.Repository<BlogEntity>().GetPageAsync(pageIndex,
                pageSize,
                s => (string.IsNullOrEmpty(searchText) || s.Name.Contains(searchText) || s.Description.Contains(searchText)),
                o => o.OrderBy(ob => ob.Id),
                se => se,
                i => i.Posts);

            var response = data.ToPagingModel<BlogEntity, BlogModel>(_mapper);

            return response;
        }

        public async Task<BlogModel> GetBlogDetailAsync(long blogId)
        {
            var data = await _unitOfWork.Repository<BlogEntity>().FirstOrDefaultAsync(f => f.Id == blogId,
                o => o.OrderBy(ob => ob.Id),
                i => i.Posts);

            var response = _mapper.Map<BlogEntity, BlogModel>(data);

            return response;
        }

        public async Task<BlogModel> UpdateBlogDetailAsync(long blogId, BlogModel blog)
        {
            var entity = _mapper.Map<BlogModel, BlogEntity>(blog);

            await _unitOfWork.Repository<BlogEntity>().UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new BlogModel();
        }
    }
}
