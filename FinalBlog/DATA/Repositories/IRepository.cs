namespace FinalBlog.DATA.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task Create(T item);
        Task Update(T item);
        Task Delete(T item);

        Task<T?> Get(int id);
        Task<T?> Get(string id);
        IEnumerable<T> GetAll();
    }
}
