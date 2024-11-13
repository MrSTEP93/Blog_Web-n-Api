using FinalBlog.DATA.Models;
using FinalBlog.ViewModels;
using FinalBlog.ViewModels.Article;

namespace FinalBlog.Services
{
    public interface ITagService
    {
        public Task<ResultModel> AddTag(TagViewModel model);
        public Task<ResultModel> UpdateTag(TagViewModel model);
        public Task<ResultModel> DeleteTag(int tagId);
        public Task<TagViewModel> GetTagById(int tagId);
        public List<TagViewModel> GetAllTags();
        public List<TagViewModel> GetTagsOfArticle(int tagId);
    }
}
