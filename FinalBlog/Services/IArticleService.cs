using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Article;

namespace FinalBlog.Services
{
    public interface IArticleService
    {
        public Task<ResultModel> AddArticle(ArticleViewModel model);
        public Task<ArticleViewModel> GetArticleById(int articleId);
        //public Task<ArticleViewModel> GetArticleByName(string articleName);
        public Task<ResultModel> UpdateArticle(ArticleViewModel model);
        public Task<ResultModel> DeleteArticle(int articleId);
        public List<ArticleViewModel> GetAllArticles();
        public List<ArticleViewModel> GetAllArticlesOfAuthor(string authorId);
    }
}
