using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Entity.Entities.Blogs;

namespace WebApp.Entity.EntityConfigurations.Blogs
{
    public class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(p => p.Blog).WithMany(m => m.Posts).HasForeignKey(p => p.BlogId);
        }
    }
}
