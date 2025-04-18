using FIAP.FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.FCG.Infra.EntityMappers
{
    public class UserProfileMap : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfile", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(65).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(80).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ConfirmPassword).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ImageURL).HasMaxLength(100).IsRequired(false);
        }
    }
}
