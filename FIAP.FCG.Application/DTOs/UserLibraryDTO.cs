using FIAP.FCG.Domain.Entities;

namespace FIAP.FCG.Application.DTOs
{
    public class UserLibraryDTO
    {
        public Guid? Id { get; set; }
        public Guid UserProfileId { get; set; }
        public Guid GameId { get; set; }
        //public Game Game { get; set; }
        //public UserProfile UserProfile { get; set; }
    }
}
