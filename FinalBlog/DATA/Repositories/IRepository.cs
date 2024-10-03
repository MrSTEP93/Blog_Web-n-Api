namespace FinalBlog.DATA.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<T> Get(int id);
        Task<T> Get(string id);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}
