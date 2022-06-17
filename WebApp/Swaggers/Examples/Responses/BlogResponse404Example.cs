using Swashbuckle.AspNetCore.Filters;
using WebApp.Service.Contract.Models;

namespace WebApp.Swaggers.Examples.Responses
{
    public class BlogResponse404Example : IExamplesProvider<BlogDto>
    {
        public BlogDto GetExamples()
        {
            return new BlogDto
            {
                Name = "My Blog Response 404",
                Description = "My blog description",
                Motto = "Blog Motto"
            };
        }
    }
}
