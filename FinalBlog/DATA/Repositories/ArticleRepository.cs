using FinalBlog.DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalBlog.DATA.Repositories
{
    public class ArticleRepository(AppDbContext dbContext) : Repository<Article>(dbContext)
    {
        public override IEnumerable<Article> GetAll()
        {
            return [.. _db.Articles.Include(a => a.Author)];
        }

        public List<Article> GetArticlesByAuthorId(string authorId)
        {
            return [.. _db.Articles.Include(a => a.Author).Where(a => a.AuthorId == authorId)];
        }
    }
}
