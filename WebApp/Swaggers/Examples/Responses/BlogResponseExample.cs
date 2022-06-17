using Swashbuckle.AspNetCore.Filters;
using WebApp.Service.Contract.Models;

namespace WebApp.Swaggers.Examples.Responses
{
    public class BlogResponseExample : IExamplesProvider<BlogDto>
    {
        public BlogDto GetExamples()
        {
            return new BlogDto
            {
                Name = "My Blog Response",
                Description = "My blog description",
                Motto = "Blog Motto"
            };
        }
    }
}
