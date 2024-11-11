using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.ViewModels.Article;

namespace FinalBlog.Services
{
    public class ArticleService(
         IMapper mapper,
         IUnitOfWork unitOfWork
         ) : IArticleService
    {
        IMapper _mapper = mapper;
        IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResultModel> AddArticle(ArticleViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = _mapper.Map<Article>(model);
            ResultModel resultModel = new(true, "Article Created");
            try
            {
                await repo.Create(article);
            }
            catch (Exception ex)
            {
                resultModel.AddMessage(ex.Message);
            }
            return resultModel;
        }

        public async Task<ResultModel> UpdateArticle(ArticleViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = new Article();
            article.ConvertArticle(model);
            ResultModel resultModel = new(true, "Article updated");
            try
            {
                await repo.Update(article);
            }
            catch (Exception ex)
            {
                resultModel.MarkAsFailed(ex.Message);
                if (ex.InnerException is not null)
                    resultModel.AddMessage(ex.InnerException.Message);
            }
            return resultModel;
        }

        public async Task<ResultModel> DeleteArticle(int articleId)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = await repo.Get(articleId);
            ResultModel resultModel = new(true);
            try
            {
                await repo.Delete(article);
            }
            catch (Exception ex)
            {
                resultModel.AddMessage(ex.Message);
            }
            return resultModel;
        }

        public async Task<ArticleViewModel> GetArticleById(int articleId)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = await repo.Get(articleId);
            var model = _mapper.Map<ArticleViewModel>(article);
            return model;
        }

        //public Task<ArticleViewModel> GetArticleByName(string articleName)
        //{
        //    throw new NotImplementedException();
        //}

        public List<ArticleViewModel> GetAllArticles()
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var list = repo.GetAll().ToList();
            var model = new List<ArticleViewModel>();
            foreach (var entity in  list)
            {
                model.Add(_mapper.Map<ArticleViewModel>(entity));
            }
            return model;
        }

        public List<ArticleViewModel> GetAllArticlesOfAuthor(string authorId)
        {
            var list = GetAllArticles().Where(a => a.AuthorID == authorId);
            var model = new List<ArticleViewModel>();
            foreach (var entity in list)
            {
                model.Add(_mapper.Map<ArticleViewModel>(entity));
            }
            return model;
        }

    }
}
