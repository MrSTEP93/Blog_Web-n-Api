using FinalBlog.Data.Models;

namespace FinalBlog.Data.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task Create(Article item);
        Task Update(Article item);
        Task Delete(Article item);
        Task<Article?> Get(int id);
        IEnumerable<Article> GetAll();
        List<Article> GetArticlesByAuthorId(string authorId);
        List<Article> GetArticlesByTagId(int tagId);
    }
}