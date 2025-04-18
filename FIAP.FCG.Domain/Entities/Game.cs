using FIAP.FCG.Domain.Core.Models;

namespace FIAP.FCG.Domain.Entities
{
    public class Game : BaseEntity
    {
        public string Name { get; private set; }
        public string Category { get; set; }
        public int Censorship { get; set; }
        public float Price { get; set; }
        public DateTime DateRelease { get; set; }
        public string? ImageURL { get; private set; }
        
        protected Game() { }

        public Game(string name, string category, int censorship, float price, DateTime dateRelease, string? imageURL)
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
