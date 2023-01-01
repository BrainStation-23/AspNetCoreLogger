using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Service.Contract.Models.Blogs;

namespace WebApp.Entity.EntityConfigurations.Blogs
{
    public class PostTagEntityConfiguration : IEntityTypeConfiguration<PostTagEntity>
    {
        public void Configure(EntityTypeBuilder<PostTagEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasKey(sc => new { sc.PostId, sc.TagId });

            builder.HasOne(bc => bc.Post).WithMany(b => b.PostTags).HasForeignKey(bc => bc.PostId);

            builder.HasOne(bc => bc.Tag).WithMany(c => c.Posts).HasForeignKey(bc => bc.TagId);
        }
    }
}
