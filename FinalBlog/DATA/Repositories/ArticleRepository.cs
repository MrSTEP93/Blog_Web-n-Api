using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalBlog.DATA.Repositories
{
    public class ArticleRepository(AppDbContext dbContext) : IArticleRepository
    {
        private readonly AppDbContext _db = dbContext;

        public async Task Create(Article item)
        {
            await _db.Articles.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Article item)
        {
            //_db.ChangeTracker.Clear();
            /*
            var existingArticle = await _db.Articles
                .Include(a => a.Tags)
                .FirstOrDefaultAsync(a => a.Id == item.Id);
            existingArticle = item;*/
            _db.Articles.Update(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Article item)
        {
            _db.Articles.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<Article?> Get(int id)
        {
            return await _db.Articles
                .Include(a => a.Author)
                .Include(a => a.Tags)
                .Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public IEnumerable<Article> GetAll()
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
    }
}
