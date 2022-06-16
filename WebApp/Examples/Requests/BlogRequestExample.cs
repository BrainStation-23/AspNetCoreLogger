using Swashbuckle.AspNetCore.Filters;
using WebApp.Service.Contract.Models;

namespace WebApp.Examples.Requests
{
    public class BlogRequestExample : IExamplesProvider<BlogDto>
    {
        public BlogDto GetExamples()
        {
            return new BlogDto
            {
                Name = "My Blog",
                Description = "My blog description",
                Motto = "Blog Motto"
            };
        }
    }
}
