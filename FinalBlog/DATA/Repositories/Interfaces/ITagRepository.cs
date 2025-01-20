using FinalBlog.DATA.Models;

namespace FinalBlog.DATA.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task Create(Tag item);
        Task Delete(Tag item);
        Task<Tag?> Get(int id);
        IEnumerable<Tag> GetAll();
        Task<Tag?> GetAsNoTracking(int id);
        Task Update(Tag item);
    }
}