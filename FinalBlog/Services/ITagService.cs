using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.Tag;

namespace FinalBlog.Services
{
    public interface ITagService
    {
        public Task<ResultModel> AddTag(TagAddViewModel model);
        public Task<ResultModel> UpdateTag(TagEditViewModel model);
        public Task<ResultModel> DeleteTag(int tagId);
        public Task<TagEditViewModel> GetTagById(int tagId);
        public List<TagEditViewModel> GetAllTags();
        public List<TagEditViewModel> GetTagsOfArticle(int tagId);
    }
}
