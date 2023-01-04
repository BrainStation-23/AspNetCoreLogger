using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Service.Contract.Models.Blogs;

namespace WebApp.Entity.EntityConfigurations.Blogs
{
    public class CommentEntityConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(p => p.Post).WithMany(m => m.Comments).HasForeignKey(p => p.PostId);
        }
    }
}
