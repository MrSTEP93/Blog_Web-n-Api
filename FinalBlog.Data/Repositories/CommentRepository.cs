using FinalBlog.Data;
using FinalBlog.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalBlog.Data.Repositories
{
    public class CommentRepository(AppDbContext dbContext) : Repository<Comment>(dbContext)
    {
        public override async Task<Comment?> Get(int id)
        {
            return await _db.Comments.Include(a => a.Author).Include(a => a.Article)
                .Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public override IEnumerable<Comment> GetAll()
        {
            return [.. _db.Comments.Include(a => a.Author).Include(a => a.Article)];
        }

        public List<Comment> GetCommentsByAuthorId(string authorId)
        {
            return [.. _db.Comments.Include(a => a.Author).Include(a => a.Article)
                .Where(a => a.AuthorId == authorId)];
        }
    }
}
