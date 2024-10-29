using Microsoft.EntityFrameworkCore;

namespace FinalBlog.DATA.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _db;

        public DbSet<T> Set { get; private set; }

        public Repository(AppDbContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.Load();

            Set = set;
        }

        public async void Create(T item)
        {
            await Set.AddAsync(item);
            await _db.SaveChangesAsync();
        }
        public async void Update(T item)
        {
             Set.Update(item);
            await _db.SaveChangesAsync();
        }

        public async void Delete(T item)
        {
            Set.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<T> Get(string id)
        {
            return await Set.FindAsync(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Set;
        }

    }
}
