using FinalBlog.DATA.Models;

namespace FinalBlog.DATA.Repositories
{
    public class UserRepository : Repository<BlogUser>
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
            
        }

    }
}
