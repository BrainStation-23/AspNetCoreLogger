using Swashbuckle.AspNetCore.Filters;
using WebApp.Service.Contract.Models.Blogs;

namespace WebApp6.Swaggers.Examples.Requests.Blogs
{
    public class BlogRequestExample : IExamplesProvider<BlogModel>
    {
        public BlogModel GetExamples()
        {
            return new BlogModel
            {
                Name = "My Blog",
                Description = "My blog description",
                Motto = "Blog Motto"
            };
        }
    }
}
