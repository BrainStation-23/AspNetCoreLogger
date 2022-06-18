using Swashbuckle.AspNetCore.Filters;
using System;
using WebApp.Service.Contract.Models.Blogs;

namespace WebApp.Examples.Responses.Blogs
{
    public class PostResponse404Example : IExamplesProvider<PostModel>
    {
        public PostModel GetExamples()
        {
            return new PostModel
            {
                BlogId = 112998,
                Title = "My post title",
                Description = "My blog description",
                ShortDescription = "My blog short description",
                IsPublished = false,
                PublishedFromDateUtc = DateTime.UtcNow.AddDays(2)
            };
        }
    }
}
