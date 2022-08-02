using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Common.Collections;
using WebApp.Entity.Entities.Blogs;
using WebApp.Service.Contract.Models.Blogs;
using WebApp.Services;
using WebApp.Core.Collections;

namespace WebApp.Service
{
    public class BlogService : BaseService<BlogEntity, BlogModel>, IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogService(IUnitOfWork unitOfWork,
                IMapper mapper) : base(unitOfWork, mapper)
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

        public async Task<BlogModel> AddBlogDetailAsync(BlogModel model)
        {
            var entity = _mapper.Map<BlogModel, BlogEntity>(model);

            var inserted = await _unitOfWork.Repository<BlogEntity>().InsertAsync(entity);
            await _unitOfWork.CompleteAsync();

            var insertedModel = _mapper.Map<BlogEntity, BlogModel>(inserted);

            return insertedModel;
        }

        public async Task<BlogModel> UpdateBlogDetailAsync(long blogId, BlogModel model)
        {
            var entity = _mapper.Map<BlogModel, BlogEntity>(model);
            entity.Id = blogId;

            var updated = await _unitOfWork.Repository<BlogEntity>().UpdateAsync(entity);
            await _unitOfWork.CompleteAsync();

            var updateModel = _mapper.Map<BlogEntity, BlogModel>(updated);

            return updateModel;
        }

        public async Task<List<BlogModel>> GetBlogsSpAsync()
        {
            var searchText = string.Empty;
            List<SqlParameter> parametes = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@search", SqlDbType = SqlDbType.NVarChar, Value = searchText?? (object) DBNull.Value},
                new SqlParameter() {ParameterName = "@pageIndex", SqlDbType = SqlDbType.Int, Value = 0},
                new SqlParameter() {ParameterName = "@pageSize", SqlDbType = SqlDbType.Int, Value = 10}
            };

            var data = await _unitOfWork.Repository<BlogEntity>().RawSqlListAsync("EXEC dbo.usp_GetBlogs @search, @pageIndex, @pageSize", parametes.ToArray());

            var response = _mapper.Map<List<BlogModel>>(data);

            return response;
        }
    }
}
