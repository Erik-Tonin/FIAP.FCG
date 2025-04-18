namespace FIAP.FCG.Domain.Statics
{
    public class CategoryStactic
    {
        /// <summary>
        /// Object instance.
        /// </summary>
        public CategoryStactic(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static CategoryStactic Action = new CategoryStactic(Guid.Parse("20581836-2641-4794-8d3c-4f13bb6c9d53"), "Ação");
        public static CategoryStactic Fear = new CategoryStactic(Guid.Parse("a3ff5d49-2a58-44d4-aad6-e1c151db7aea"), "Terror");
        public static CategoryStactic Comedy = new CategoryStactic(Guid.Parse("33e644e9-3be5-4490-98be-82ed04ed28a9"), "Comedia");

        public static List<CategoryStactic> GetAll()
        {
            return new List<CategoryStactic>()
            {
                Action,
                Fear,
                Comedy
            };
        }

        public static CategoryStactic GetById(Guid id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public static CategoryStactic[] DataArray()
        {
            return GetAll().Select(x => new CategoryStactic(x.Id, x.Name)).ToArray();
        }
    }
}
