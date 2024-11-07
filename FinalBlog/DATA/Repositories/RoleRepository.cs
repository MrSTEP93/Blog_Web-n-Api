using FinalBlog.DATA.Models;

namespace FinalBlog.DATA.Repositories
{
    public class RoleRepository(AppDbContext db) : Repository<Role>(db)
    {

    }
}
