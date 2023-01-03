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

            builder.HasOne(bc => bc.Post).WithMany(b => b.Likes).HasForeignKey(bc => bc.PostId);

        }
    }
}
