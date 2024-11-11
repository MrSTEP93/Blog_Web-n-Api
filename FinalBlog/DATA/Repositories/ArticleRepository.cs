using FinalBlog.DATA.Models;

namespace FinalBlog.DATA.Repositories
{
    public class ArticleRepository(AppDbContext dbContext) : Repository<Article>(dbContext)
    {

    }
}
