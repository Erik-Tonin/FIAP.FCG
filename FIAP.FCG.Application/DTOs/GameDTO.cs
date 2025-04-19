namespace FIAP.FCG.Application.DTOs
{
    public class GameDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public int Censorship { get; set; }
        public float Price { get; set; }
        public DateTime DateRelease { get; set; }
        public string? ImageURL { get; private set; }
    }
}
