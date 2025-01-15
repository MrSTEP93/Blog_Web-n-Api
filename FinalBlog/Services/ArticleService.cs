using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using System.Collections.Generic;
using System.Security.Claims;

namespace FinalBlog.Services
{
    public class ArticleService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IUserService userService,
        ICommentService commentService
        ) : IArticleService
    {
        readonly IMapper _mapper = mapper;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IUserService _userService = userService;
        readonly ICommentService _commentService = commentService;

        public async Task<ResultModel> AddArticle(ArticleAddViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = _mapper.Map<Article>(model);
            var resultModel = CheckIfAuthorExists(model.AuthorId);
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

        public async Task<ResultModel> UpdateArticle(ArticleEditViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = new Article();
            article.ConvertArticle(model);
            var resultModel = new ResultModel(true);
            //var resultModel = CheckAuthor(model.AuthorId);
            /*
            A second operation was started on this context instance before a previous operation completed. 
            This is usually caused by different threads concurrently using the same instance of DbContext.
            For more information on how to avoid threading issues with DbContext, see https://go.microsoft.com/fwlink/?linkid=2097913.
             */
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
            ResultModel resultModel = new(true, "Article deleted");
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
            model.Comments = _commentService.GetCommentsOfArticle(articleId);
            return model;
        }

        public ArticleListViewModel GetAllArticles()
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var list = repo.GetAll().OrderByDescending(x => x.CreationTime).ToList();
            var model = CreateListOfViewModel(list);
            foreach (var post in model.Articles)
            {
                post.Comments = _commentService.GetCommentsOfArticle(post.Id);
            }
            return model;
        }

        public ArticleListViewModel GetArticlesOfAuthor(string authorId)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var list = repo.GetArticlesByAuthorId(authorId).OrderByDescending(x => x.CreationTime).ToList();
            var model = CreateListOfViewModel(list, authorId);
            return model;
        }

        public ResultModel CheckIfUserCanEdit(ClaimsPrincipal user, string authorId)
        {
            var resultModel = new ResultModel(false, "Вы не можете редактировать эту статью");
            if (
                    user.FindFirstValue(ClaimTypes.NameIdentifier) == authorId
                    || user.IsInRole("Администратор") 
                    || user.IsInRole("Модератор")
                )
                resultModel.MarkAsSuccess("Редактирование статьи разрешено");

            return resultModel;
        }
        
        public ResultModel CheckIfUserCanAdd(ClaimsPrincipal user)
        {
            var resultModel = new ResultModel(false, "Вы не можете добавлять статьи");
            if (
                    user.IsInRole("Администратор")
                    || user.IsInRole("Модератор")
                    || user.IsInRole("Пользователь")
                )
                resultModel.MarkAsSuccess("Добавление статьи разрешено");

            return resultModel;
        }

        private ResultModel CheckIfAuthorExists(string authorId)
        {
            var resultModel = new ResultModel(true, "Author found");
            var user = _userService.GetUserById(authorId).Result;
            if (user == null)
                resultModel.MarkAsFailed("Author not exists");

            return resultModel;
        }

        private ArticleListViewModel CreateListOfViewModel(List<Article> list)
        {
            var model = new ArticleListViewModel() { Articles = [] };
            foreach (var entity in list)
                model.Articles.Add(_mapper.Map<ArticleViewModel>(entity));

            return model;
        }
        
        private ArticleListViewModel CreateListOfViewModel(List<Article> list, string authorId)
        {
            var model = CreateListOfViewModel(list);
            var user = _userService.GetUserById(authorId).Result;
            model.Title = $"Статьи автора {user.FirstName} {user.LastName}";
            
            return model;
        }
    }
}
