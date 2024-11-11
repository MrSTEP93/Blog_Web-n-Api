using FinalBlog.DATA.Models;

namespace FinalBlog.DATA.Repositories
{
    public class UserRepository(AppDbContext dbContext) : Repository<BlogUser>(dbContext)
    {

    }
}
