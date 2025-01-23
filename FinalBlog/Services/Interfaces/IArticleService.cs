using FinalBlog.Data.ApiModels.Articles;
using FinalBlog.Data.Models;
using FinalBlog.ViewModels.Article;
using System.Security.Claims;

namespace FinalBlog.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<ResultModel> AddArticle(ArticleAddViewModel model);
        public Task<ArticleViewModel> GetArticleById(int articleId);
        public Task<ResultModel> UpdateArticle(ArticleEditViewModel model);
        public Task<ResultModel> DeleteArticle(int articleId);
        public ArticleListViewModel GetAllArticles();
        public ArticleListViewModel GetArticlesOfAuthor(string authorId);
        public ArticleListViewModel GetArticlesByTag(int tagId);
        public ResultModel CheckIfUserCanEdit(ClaimsPrincipal user, string authorId);
        public ResultModel CheckIfUserCanAdd(ClaimsPrincipal user);

        public ArticleResponse ConvertToApiModel(ArticleViewModel viewModel);
        public List<ArticleResponse> ConvertToApiModel(List<ArticleViewModel> viewModel);
        public List<ArticleResponse> ConvertToApiModel(ArticleListViewModel viewModel);

        public ArticleAddViewModel ConvertToAddViewModel(ArticleAddRequest request);
        public ArticleEditViewModel ConvertToEditViewModel(ArticleEditRequest request);

    }
}
