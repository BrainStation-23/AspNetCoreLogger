using Swashbuckle.AspNetCore.Filters;
using WebApp.Service.Contract.Models.Blogs;

namespace WebApp6.Swaggers.Examples.Responses.Blogs
{
    public class BlogResponse404Example : IExamplesProvider<BlogModel>
    {
        public BlogModel GetExamples()
        {
            return new BlogModel
            {
                Name = "My Blog Response 404",
                Description = "My blog description",
                Motto = "Blog Motto"
            };
        }
    }
}
