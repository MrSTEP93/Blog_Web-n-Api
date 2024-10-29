namespace FinalBlog.DATA.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Create(T item);
        void Update(T item);
        void Delete(T item);

        Task<T> Get(int id);
        Task<T> Get(string id);
        IEnumerable<T> GetAll();
    }
}
