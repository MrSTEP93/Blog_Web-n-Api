using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.Tag;

namespace FinalBlog.Services.Interfaces
{
    public interface ITagService
    {
        public Task<ResultModel> AddTag(TagAddViewModel model);
        public Task<ResultModel> UpdateTag(TagEditViewModel model);
        public Task<ResultModel> DeleteTag(int tagId);
        public Task<TagEditViewModel> GetTagById(int tagId);
        public Task<TagEditViewModel> GetTagByIdAsNoTracking(int tagId);
        public TagListViewModel GetAllTags();
        public List<TagViewModel> GetAllTagsList();
        public Task<List<Tag>> GetTagsByIds(List<int> selectedIds);
    }
}
