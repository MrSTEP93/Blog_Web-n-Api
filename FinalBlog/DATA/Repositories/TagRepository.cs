using FinalBlog.DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalBlog.DATA.Repositories
{
    public class TagRepository(AppDbContext dbContext) : Repository<Tag>(dbContext)
    {


        public async Task<Tag?> GetTagAsNoTracking(int id)
        {
            return await _db.Tags
                .AsNoTracking()
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
