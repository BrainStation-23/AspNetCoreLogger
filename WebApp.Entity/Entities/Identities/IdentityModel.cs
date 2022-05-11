using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Entity.Entities.Identities
{
    public class IdentityModel
    {
        [Table("Users")]
        public class User : IdentityUser<long>
        {
            [Required]
            [MaxLength(100)]
            public string Firstname { get; set; }

            [MaxLength(100)]
            public string Lastname { get; set; }

            [MaxLength(512)]
            public string Address1 { get; set; }

            [MaxLength(512)]
            public string Address2 { get; set; }

            public long CreatedUserId { get; set; }
            public DateTime CreatedDateTimeUtc { get; set; }
            public long UpdatedUserId { get; set; }
            public DateTime? UpdatedDateTimeUtc { get; set; }

        }

        [Table("UserRoles")]
        public class UserRole : IdentityUserRole<long>
        {
        }

        [Table("UserClaims")]
        public class UserClaim : IdentityUserClaim<long>
        {
        }

        public class UserLogin : IdentityUserLogin<long>
        {
            [ForeignKey("User"), Key]
            public override long UserId { get => base.UserId; set => base.UserId = value; }
            public User User { get; set; }
        }

        [NotMapped]
        public class RoleClaim : IdentityRoleClaim<long>
        {
        }

        [Table("UserTokens")]
        public class UserToken : IdentityUserToken<long>
        {
        }

        [Table("Roles")]
        public class Role : IdentityRole<long>
        {
            public Role() { }
            public Role(string name) { Name = name; }

            public int StatusId { get; set; }
            public string Description { get; set; }

            public long CreatedBy { get; set; }
            public DateTime CreatedDateUtc { get; set; }
            public long UpdatedBy { get; set; }
            public DateTime? UpdatedDateUtc { get; set; }
        }
    }
}
