using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.ViewModels.Article;
using System.Collections.Generic;

namespace FinalBlog.Services
{
    public class ArticleService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IUserService userService
        ) : IArticleService
    {
        readonly IMapper _mapper = mapper;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IUserService _userService = userService;

        public async Task<ResultModel> AddArticle(ArticleViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = _mapper.Map<Article>(model);
            var resultModel = CheckAuthor(model.AuthorId);
            if (resultModel.IsSuccessed)
                try
                {
                    await repo.Create(article);
                    resultModel.MarkAsSuccess("Article created");
                }
                catch (Exception ex)
                {
                    resultModel.AddMessage(ex.Message);
                    if (ex.InnerException is not null)
                        resultModel.AddMessage(ex.InnerException.Message);
                }
            return resultModel;
        }

        public async Task<ResultModel> UpdateArticle(ArticleViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = new Article();
            article.ConvertArticle(model);
            var resultModel = CheckAuthor(model.AuthorId);
            if (resultModel.IsSuccessed)
                try
                {
                    await repo.Update(article);
                    resultModel.MarkAsSuccess("Article updated");
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
                if (ex.InnerException is not null)
                    resultModel.AddMessage(ex.InnerException.Message);
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
            var model = CreateListOfViewModel(list);
            return model;
        }

        public List<ArticleViewModel> GetArticlesOfAuthor(string authorId)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var list = repo.GetArticlesByAuthorId(authorId);
            var model = CreateListOfViewModel(list);
            return model;
        }

        private ResultModel CheckAuthor(string authorId)
        {
            var resultModel = new ResultModel(true, "Author found");
            var user = _userService.GetUserById(authorId);
            if (user == null)
                resultModel.MarkAsFailed("Author not exists");

            return resultModel;
        }

        private List<ArticleViewModel> CreateListOfViewModel(List<Article> list)
        {
            var model = new List<ArticleViewModel>();
            foreach (var entity in list)
            {
                model.Add(_mapper.Map<ArticleViewModel>(entity));
            }

            return model;
        }
    }
}
