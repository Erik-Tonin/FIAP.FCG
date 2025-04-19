using FIAP.FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.FCG.Infra.EntityMappers
{
    public class GameMap : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Game", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(65).IsRequired();
            builder.Property(x => x.Category).HasMaxLength(80).IsRequired();
            builder.Property(x => x.Censorship).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Price).HasMaxLength(100).IsRequired();
            builder.Property(x => x.DateRelease).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ImageURL).HasMaxLength(100).IsRequired(false);
        }
    }
}
