using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FinalBlog.DATA.Repositories
{
    public class TagRepository(AppDbContext dbContext) : ITagRepository
    {
        readonly AppDbContext _db = dbContext;
        public async Task Create(Tag item)
        {
            await _db.Tags.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Tag item)
        {
            _db.ChangeTracker.Clear();
            _db.Tags.Update(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Tag item)
        {
            _db.Tags.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<Tag?> Get(int id)
        {
            var tag = await _db.Tags
                .Include(a => a.Articles)
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
            ShutTheFuckUp();
            return tag;
        }

        public async Task<Tag?> GetAsNoTracking(int id)
        {
            return await _db.Tags
                .AsNoTracking()
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }

        public IEnumerable<Tag> GetAll()
        {
            return [.. _db.Tags
                .Include(a => a.Articles)];
        }

        private void ShutTheFuckUp()
        {
            _db.ChangeTracker.Clear();
        }
    }
}
