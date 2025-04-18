using FIAP.FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FIAP.FCG.Domain.Statics;

namespace FIAP.FCG.Infra.EntityMappers
{
    public class CategoryProfileMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(65).IsRequired();

            builder.HasData(CategoryStactic.GetAll().Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name
            }));
        }
    }
}
