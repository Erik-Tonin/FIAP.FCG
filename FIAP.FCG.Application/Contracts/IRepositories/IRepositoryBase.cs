namespace FIAP.FCG.Application.Contracts.IRepositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity obj);
        void Remove(TEntity obj);
        void RemoveAll(TEntity[] objs);
        void Dispose();
        void UpdateAll(TEntity[] users);
    }
}
