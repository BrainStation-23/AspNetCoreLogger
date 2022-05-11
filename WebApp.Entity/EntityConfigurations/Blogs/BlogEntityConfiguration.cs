using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Entity.Entities.Blogs;

namespace WebApp.Entity.EntityConfigurations.Blogs
{
    public class BlogEntityConfiguration : IEntityTypeConfiguration<BlogEntity>
    {
        public void Configure(EntityTypeBuilder<BlogEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
