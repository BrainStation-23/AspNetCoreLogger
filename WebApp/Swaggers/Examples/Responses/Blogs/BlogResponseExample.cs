using Swashbuckle.AspNetCore.Filters;
using WebApp.Service.Contract.Models.Blogs;

namespace WebApp.Swaggers.Examples.Responses.Blogs
{
    public class BlogResponseExample : IExamplesProvider<BlogModel>
    {
        public BlogModel GetExamples()
        {
            return new BlogModel
            {
                Name = "My Blog Response",
                Description = "My blog description",
                Motto = "Blog Motto"
            };
        }
    }
}
