using FinalBlog.DATA.Models;
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
    }
}
