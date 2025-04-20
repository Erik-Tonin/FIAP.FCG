using FIAP.FCG.Domain.Core.Models;

namespace FIAP.FCG.Domain.Entities
{
    public class Game : BaseEntity
    {
        public string Name { get; private set; }
        public Guid Category { get; private set; }
        public int Censorship { get; private set; }
        public float Price { get; private set; }
        public DateTime DateRelease { get; private set; }
        public string? ImageURL { get; private set; }
        
        protected Game() { }

        public Game(string name, Guid category, int censorship, float price, DateTime dateRelease, string? imageURL)
        {
            Name = name;
            Category = category;
            Censorship = censorship;
            Price = price;
            DateRelease = dateRelease;
            ImageURL = imageURL;
        }

        public override bool IsValid()
        {
            ValidationResult = new GameValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public void ValidationClass()
        {
            if (this.Name.Length == 0)
                throw new Exception("");
        }

        public void ChangeName(string name)
        {
            Name = name;
            ValidationClass();
        }

        public void UpdateGame(string name, Guid category, int censorship, float price, DateTime dateRelease, string? imageURL)
        {
            Name = name;
            Category = category;
            Censorship = censorship;
            Price = price;
            DateRelease = dateRelease;
            ImageURL = imageURL;
        }
    }
}
