using FinalBlog.DATA.Models;

namespace FinalBlog.DATA.Repositories
{
    public class TagRepository(AppDbContext dbContext) : Repository<Tag>(dbContext)
    {

    }
}
