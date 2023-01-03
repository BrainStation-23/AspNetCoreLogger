using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Common.Collections;
using WebApp.Core;
using WebApp.Core.Collections;
using WebApp.Entity.Entities.Blogs;
using WebApp.Service.Contract.Models.Blogs;
using WebApp.Services;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp.Service
{
    public class BlogService : BaseService<BlogEntity, BlogModel>, IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public BlogService(IUnitOfWork unitOfWork,
                IMapper mapper,
                UserManager<User> userManager) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
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

        public async Task<List<BlogBulkModel>> AddBulkBlogAsync(List<BlogBulkModel> blogs)
        {
            var blogEntities = new List<BlogEntity>();
            var users = blogs.SelectMany(e => e.Posts).SelectMany(e => e.Likes).SelectMany(e => e.Username);
            var blogNames = blogs.Select(e => e.Name).ToList();
            var existBlogEntities = await _unitOfWork.Repository<BlogEntity>().GetAllAsync(e => blogNames.Contains(e.Name));
            var insertBlogEntites = blogs.Where(e => !existBlogEntities.Select(b => b.Name)
                    .ToList()
                    .Contains(e.Name))
                    .Select(e => new BlogEntity
                    {
                        Name = e.Name,
                        Description = e.Description,
                        Motto = e.Motto,
                        Posts = e.Posts.Select(p => new PostEntity
                        {
                            Title = p.Title,
                            Description = p.Description,
                            ShortDescription = p.ShortDescription,
                            PublishedFromDateUtc = p.PublishedFromDateUtc,
                            Tags = p.Tags.Select(t => new TagEntity
                            {
                                Name = t.Name
                            }).ToList(),
                            Comments = p.Comments.Select(c => new CommentEntity
                            {
                                Comment = c.Comment,
                            }).ToList(),
                            Likes = p.Likes.Select(l => new LikeEntity
                            {
                            }).ToList()
                        }).ToList(),
                    }).ToList();

            foreach (var entity in existBlogEntities)
            {
                var blog = blogs.FirstOrDefault(e => e.Name == entity.Name);

                entity.Name = blog.Name;
                entity.Description = blog.Description;
                entity.Posts = new List<PostEntity> {
                    new PostEntity
                    {
                        Tags = new List<TagEntity> (),
                        Comments = new List<CommentEntity> ()
                    }
                };
            }

            var inserted = await _unitOfWork.Repository<BlogEntity>().InsertRangeAsync(insertBlogEntites);
            await _unitOfWork.CompleteAsync();

            var blogBulks = existBlogEntities.Union(inserted);

            var insertedModels = _mapper.Map<List<BlogBulkModel>>(blogBulks);

            return insertedModels;
        }

        public async Task AddBlogOperationAsync()
        {
            var blog = new BlogEntity
            {
                Name = "Medium_" + DateTime.Now,
                Description = "Medium is an open platform where readers find dynamic thinking, " +
                "and where expert and undiscovered voices can share their writing on any topic.",
                Motto = "Where good ideas find you."                
            };
            await _unitOfWork.Repository<BlogEntity>().InsertAsync(blog);
            await _unitOfWork.CompleteAsync();

            var post = new PostEntity
            {
                BlogId = blog.Id,
                Title = "2022: A Review of the Year in Neuroscience",
                ShortDescription = "Can we go back to boring now? No wars launched by a slightly unhinged " +
                "despot, no global inflation, energy crisis, or ongoing pandemic. ",
                Description = "Neuroscience could do with being a bit more boring. Slowing down a bit," +
                " giving us all a chance to catch up. A chance to take it all in.",
                CreatedDateUtc = DateTime.Now,
            };
            await _unitOfWork.Repository<PostEntity>().InsertAsync(post);
            await _unitOfWork.CompleteAsync();

            var comment = new CommentEntity
            {
                PostId = post.Id,
                Comment = "You write the most wonderful articles. I wish I was young enough to go back" +
                " to school and learn the science of the brain.",
                CreatedDateUtc = DateTime.Now,
            };
            await _unitOfWork.Repository<CommentEntity>().InsertAsync(comment);
            await _unitOfWork.CompleteAsync();

            var updateBlogEntity = await _unitOfWork.Repository<BlogEntity>().FirstOrDefaultAsync(blog.Id);
            updateBlogEntity.Motto = "Where innovative ideas find you.";
            await _unitOfWork.Repository<BlogEntity>().UpdateAsync(updateBlogEntity);

            var updatePostEntity = await _unitOfWork.Repository<PostEntity>().FirstOrDefaultAsync(post.Id);
            updatePostEntity.Title = "A Review of the Year in Neuroscience";
            await _unitOfWork.Repository<PostEntity>().UpdateAsync(updatePostEntity);

            var updateCommentEntity = await _unitOfWork.Repository<CommentEntity>().FirstOrDefaultAsync(comment.Id);
            updateCommentEntity.Comment = "Where innovative ideas find you.";
            await _unitOfWork.Repository<CommentEntity>().UpdateAsync(updateCommentEntity);
            await _unitOfWork.CompleteAsync();

            var deleteCommentEntity = await _unitOfWork.Repository<CommentEntity>().FirstOrDefaultAsync(comment.Id);
            await _unitOfWork.Repository<CommentEntity>().DeleteAsync(deleteCommentEntity);


            var deletePostEntity = await _unitOfWork.Repository<PostEntity>().FirstOrDefaultAsync(post.Id);
            await _unitOfWork.Repository<PostEntity>().DeleteAsync(deletePostEntity);


            var deleteBlogEntity = await _unitOfWork.Repository<BlogEntity>().FirstOrDefaultAsync(blog.Id);
            await _unitOfWork.Repository<BlogEntity>().DeleteAsync(deleteBlogEntity);
            await _unitOfWork.CompleteAsync();

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
