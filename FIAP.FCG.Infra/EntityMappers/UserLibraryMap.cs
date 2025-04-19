using FIAP.FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FIAP.FCG.Infra.EntityMappers
{
    public class UserLibraryMap : IEntityTypeConfiguration<UserLibrary>
    {
        public void Configure(EntityTypeBuilder<UserLibrary> builder)
        {
            builder.ToTable("UserLibrary", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserProfileId).HasMaxLength(65).IsRequired();
            builder.Property(x => x.GameId).HasMaxLength(80).IsRequired();
        }
    }
}
