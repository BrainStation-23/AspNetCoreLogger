using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Service.Contract.Models.Blogs;

namespace WebApp.Entity.EntityConfigurations.Blogs
{
    public class LikeEntityConfiguration : IEntityTypeConfiguration<LikeEntity>
    {
        public void Configure(EntityTypeBuilder<LikeEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasKey(sc => new { sc.PostId, sc.UserId });

            builder.HasOne(bc => bc.Post).WithMany(b => b.Users).HasForeignKey(bc => bc.PostId);

            builder.HasOne(bc => bc.User).WithMany(c => c.Posts).HasForeignKey(bc => bc.UserId);
        }
    }
}
