using FIAP.FCG.Domain.Core.Models;

namespace FIAP.FCG.Domain.Entities
{
    public class UserLibrary : BaseEntity
    {
        public Guid UserProfileId { get; private set; }
        public Guid GameId { get; private set; }
        public virtual Game Game { get; private set; }
        public virtual UserProfile UserProfile { get; private set; }

        protected UserLibrary() { }

        public UserLibrary(Guid userProfileId, Guid gameId)
        {
            UserProfileId = userProfileId;
            GameId = gameId;
            DateCreated = DateTime.Now;
        }
    }
}
