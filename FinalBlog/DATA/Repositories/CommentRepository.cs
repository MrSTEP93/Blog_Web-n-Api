using FinalBlog.DATA.Models;

namespace FinalBlog.DATA.Repositories
{
    public class CommentRepository(AppDbContext dbContext) : Repository<Comment>(dbContext)
    {

    }
}
