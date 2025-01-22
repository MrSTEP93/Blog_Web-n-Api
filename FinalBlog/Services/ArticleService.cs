using AutoMapper;
using FinalBlog.Controllers;
using FinalBlog.Data.Models;
using FinalBlog.Data.Repositories.Interfaces;
//using FinalBlog.DATA.Repositories;
//using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace FinalBlog.Services
{
    public class ArticleService(
        IMapper mapper,
        IUserService userService,
        ICommentService commentService,
        ITagService tagService,
        IArticleRepository articleRepository,
        ILogger<ArticleService> logger
        ) : IArticleService
    {
        readonly IMapper _mapper = mapper;
        readonly IUserService _userService = userService;
        readonly ICommentService _commentService = commentService;
        readonly ITagService _tagService = tagService;
        readonly IArticleRepository repo = articleRepository;
        readonly ILogger<ArticleService> _logger = logger;

        public async Task<ResultModel> AddArticle(ArticleAddViewModel model)
        {
            var article = _mapper.Map<Article>(model);
            var resultModel = CheckIfAuthorExists(model.AuthorId);
            if (resultModel.IsSuccessed)
                try
                {
                    await repo.Create(article);
                    resultModel.MarkAsSuccess("Article created");
                    _logger.LogInformation($"Создана статья \"{model.Title}\" id={article.Id}");
                }
                catch (Exception ex)
                {
                    resultModel = ProcessException($"Ошибка создания статьи id={article.Id} ", ex);
                }
            return resultModel;
        }

        public async Task<ResultModel> UpdateArticle(ArticleEditViewModel model)
        {
            var newTags = await _tagService.GetTagsByIds(model.SelectedTagIds);
            var resultModel = new ResultModel(false, "Internal error");
            var article = await repo.Get(model.Id);

            if (article == null)
            {
                _logger.LogError($"Ошибка при редактировании статьи id={article.Id}: статья не обнаружена (неверный id)");
                return new ResultModel(false, "Article not found");
            }
            
            article.ConvertArticle(model);
            if (model.SelectedTagIds?.Count != 0)
            {
                article.Tags.Clear();
                article.Tags.AddRange(newTags);
            }
            else if (model.SelectedTagIds == null || model.SelectedTagIds.Count == 0)
            {
                article.Tags ??= [];
                article.Tags.Clear();
            }
            resultModel = await TryToUpdate(article);

            return resultModel;
        }

        public async Task<ResultModel> DeleteArticle(int articleId)
        {
            var article = await repo.Get(articleId);
            ResultModel resultModel = new(true, "Article deleted");
            try
            {
                await repo.Delete(article);
                _logger.LogInformation($"Удалена статья id={articleId}");
            }
            catch (Exception ex)
            {
                resultModel = ProcessException($"Ошибка удаления статьи id={article.Id}", ex);
            }
            return resultModel;
        }

        public async Task<ArticleViewModel> GetArticleById(int articleId)
        {
            var article = await repo.Get(articleId);
            var model = _mapper.Map<ArticleViewModel>(article);
            model.Comments = _commentService.GetCommentsOfArticle(articleId);
            model.AllTags = _tagService.GetAllTagsList();
            return model;
        }

        public ArticleListViewModel GetAllArticles()
        {
            var list = repo.GetAll().OrderByDescending(x => x.CreationTime).ToList();
            var model = CreateListOfViewModel(list);
            return model;
        }

        public ArticleListViewModel GetArticlesOfAuthor(string authorId)
        {
            var list = repo.GetArticlesByAuthorId(authorId).OrderByDescending(x => x.CreationTime).ToList();
            var model = CreateListOfViewModel(list, authorId);
            return model;
        }
        
        public ArticleListViewModel GetArticlesByTag(int tagId)
        {
            var list = repo.GetArticlesByTagId(tagId).OrderByDescending(x => x.CreationTime).ToList();
            var model = CreateListOfViewModel(list);
            return model;
        }

        private async Task<ResultModel> TryToUpdate(Article article)
        {
            var resultModel = new ResultModel(true);
            try
            {
                await repo.Update(article!);
                _logger.LogInformation($"Обновлена статья id={article.Id}");
                resultModel.MarkAsSuccess("Article updated");
            }
            catch (Exception ex)
            {
                resultModel = ProcessException($"Ошибка обновления статьи id={article.Id}", ex);
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
            else
                _logger.LogError($"Пользователю {user.FindFirstValue(ClaimTypes.Email)} запрещено редактировать статью");

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
            else
                _logger.LogError($"Пользователю {user.FindFirstValue(ClaimTypes.Email)} запрещено добавлять статью");

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
            {
                var item = _mapper.Map<ArticleViewModel>(entity);
                item.Comments = _commentService.GetCommentsOfArticle(entity.Id);
                model.Articles.Add(item);
            }

            return model;
        }
        
        private ArticleListViewModel CreateListOfViewModel(List<Article> list, string authorId)
        {
            var model = CreateListOfViewModel(list);
            var user = _userService.GetUserById(authorId).Result;
            model.Title = $"Статьи автора {user.FirstName} {user.LastName}";
            
            return model;
        }

        /// <summary>
        /// Глобальный обработчик ошибок (если можно так выразиться xD)
        /// глобальный для этого класса (другое не успел протестировать и внедрить)
        /// </summary>
        /// <param name="logMessage">Сообщение для записи в лог</param>
        /// <param name="ex">полученное исключение </param>
        /// <returns></returns>
        private ResultModel ProcessException(string logMessage, Exception ex)
        {
            var resultModel = new ResultModel();
            resultModel.MarkAsFailed(ex.Message);
            _logger.LogError($"{logMessage}: {ex.Message}");
            if (ex.InnerException is not null)
            {
                _logger.LogError($" --- InnerException: {ex.InnerException.Message}");
                resultModel.AddMessage(ex.InnerException.Message);
            }
            return resultModel;
        }
    }
}
