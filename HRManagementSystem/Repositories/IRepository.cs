namespace HRManagementSystem.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByNameWithIgnorFilterAsync(string name);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(int id);
        void DeleteAll();
        Task SaveChangesAsync();
    }
}
