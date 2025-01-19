using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace FinalBlog.Services
{
    public class ArticleService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IUserService userService,
        ICommentService commentService,
        ITagService tagService
        ) : IArticleService
    {
        readonly IMapper _mapper = mapper;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IUserService _userService = userService;
        readonly ICommentService _commentService = commentService;
        readonly ITagService _tagService = tagService;

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
            //var article = new Article();
            var resultModel = new ResultModel(false, "Internal error");
            var article = await repo.Get(model.Id);

            if (article == null)
                return new ResultModel(false, "Article not found");
            
            article.ConvertArticle(model);
            if (model.SelectedTagIds != null && model.SelectedTagIds.Count != 0)
            {
                var newTags = await _tagService.GetTagsByIds(model.SelectedTagIds);
                article.Tags.Clear();
                article.Tags.AddRange(newTags);
            }
            else if (model.SelectedTagIds == null || model.SelectedTagIds.Count == 0)
            {
                article.Tags ??= [];
                article.Tags.Clear();
            }
            resultModel = await TryToUpdate(repo, article);

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
            model.AllTags = _tagService.GetAllTagsList();
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
        
        public ArticleListViewModel GetArticlesByTag(int tagId)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var list = repo.GetArticlesByTagId(tagId).OrderByDescending(x => x.CreationTime).ToList();
            var model = CreateListOfViewModel(list);
            return model;
        }

        private static async Task<ResultModel> TryToUpdate(ArticleRepository? repo, Article? article)
        {
            var resultModel = new ResultModel(true);
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
