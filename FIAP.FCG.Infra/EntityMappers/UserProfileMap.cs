using FIAP.FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.FCG.Infra.EntityMappers
{
    public class UserProfileMap : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfile", "userprofile");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(65).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(80).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(15).IsRequired();
            builder.Property(x => x.ConfirmPassword).HasMaxLength(15).IsRequired();
        }
    }
}
