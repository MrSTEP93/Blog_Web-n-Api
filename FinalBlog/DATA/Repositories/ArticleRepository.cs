using FinalBlog.DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalBlog.DATA.Repositories
{
    public class ArticleRepository(AppDbContext dbContext) : Repository<Article>(dbContext)
    {
        public override async Task<Article?> Get(int id)
        {
            return await _db.Articles
                .Include(a => a.Author)
                .Include(a => a.Tags)
                .Where(a => a.Id == id).FirstOrDefaultAsync();
        }
        
        public override IEnumerable<Article> GetAll()
        {
            return [.. _db.Articles
                .Include(a => a.Author)
                .Include(a => a.Tags)];
        }

        public List<Article> GetArticlesByAuthorId(string authorId)
        {
            return [.. _db.Articles
                .Include(a => a.Author)
                .Include(a => a.Tags)
                .Where(a => a.AuthorId == authorId)];
        }

        public List<Article> GetArticlesByTagId(int tagId)
        {
            return [.. _db.Articles
                .Include(a => a.Author)
                .Include(a => a.Tags)
                .Where(a => a.Tags.Any(x => x.Id == tagId))];
        }

        public override async Task Update(Article item)
        {
            //_db.ChangeTracker.Clear();
            var existingArticle = await _db.Articles
                .Include(a => a.Tags)
                .FirstOrDefaultAsync(a => a.Id == item.Id);

            existingArticle = item;
            await _db.SaveChangesAsync();
        }
    }
}
