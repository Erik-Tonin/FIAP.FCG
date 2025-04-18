using FIAP.FCG.Domain.Core.Models;

namespace FIAP.FCG.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Category() { }

        public Category(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
