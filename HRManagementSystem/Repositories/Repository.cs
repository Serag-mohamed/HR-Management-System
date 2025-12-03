using HRManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HRManagementSystem.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDBContext _context;
        private readonly DbSet<T> _entities;

        public Repository(AppDBContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public void Delete(int id)
        {
            var entity = _entities.Find(id);
            if (entity != null)
               _entities.Remove(entity);
        }

        public void DeleteAll()
        {
            _entities.RemoveRange(_entities);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T?> GetByNameWithIgnorFilterAsync(string name)
        {
            return await _entities
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => EF.Property<string>(x, "Name") == name);
        }

    }
}
