using FinalBlog.Data;
using FinalBlog.Data.Models;

namespace FinalBlog.Data.Repositories
{
    public class RoleRepository(AppDbContext db) : Repository<Role>(db)
    {

    }
}
