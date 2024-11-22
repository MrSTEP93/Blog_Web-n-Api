using FinalBlog.DATA.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FinalBlog.DATA.Repositories
{
    public class UserRepository(AppDbContext dbContext) : Repository<BlogUser>(dbContext)
    {
        /*
        public override IEnumerable<BlogUser> GetAll()
        {
            return [.. _db.BlogUsers.Include(x => x.Roles)];
        }
        */
        public override async Task<BlogUser?> Get(string id)
        {
            return await _db.BlogUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
