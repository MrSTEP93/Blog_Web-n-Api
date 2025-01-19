using FinalBlog.DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalBlog.DATA.Repositories
{
    public class TagRepository(AppDbContext dbContext) : Repository<Tag>(dbContext)
    {
        public void AttachTags(List<Tag> tags)
        {
            foreach (var tag in tags)
            {
                var entity = _db.Tags.Local.FirstOrDefault(t => t.Id == tag.Id);

                if (entity == null) // Если тег не отслеживается, присоединяем его
                {
                    _db.Tags.Attach(tag);
                }
            }
        }
    }
}
