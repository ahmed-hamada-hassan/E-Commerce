using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configs
{
    public class IdentityGlobalConfiguration :
        IEntityTypeConfiguration<IdentityRole<Guid>>,
        IEntityTypeConfiguration<IdentityUserRole<Guid>>,
        IEntityTypeConfiguration<IdentityUserClaim<Guid>>,
        IEntityTypeConfiguration<IdentityUserLogin<Guid>>,
        IEntityTypeConfiguration<IdentityRoleClaim<Guid>>,
        IEntityTypeConfiguration<IdentityUserToken<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder) => builder.ToTable("Roles", "Identity");
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder) => builder.ToTable("UserRoles", "Identity");
        public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder) => builder.ToTable("UserClaims", "Identity");
        public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder) => builder.ToTable("UserLogins","Identity");
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder) => builder.ToTable("RoleClaims","Identity");
        public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder) => builder.ToTable("UserTokens","Identity");
    }
}
