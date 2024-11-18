using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics;

namespace FinalBlog.DATA.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _db;

        public DbSet<T> Set { get; private set; }

        public Repository(AppDbContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.Load();

            Set = set;
        }

        public async Task Create(T item)
        {
            await Set.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(T item)
        {
            _db.ChangeTracker.Clear();
            Set.Update(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(T item)
        {
            Set.Remove(item);
            await _db.SaveChangesAsync();
        }

        public virtual async Task<T?> Get(int id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<T?> Get(string id)
        {
            return await Set.FindAsync(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Set;
        }

    }
}
