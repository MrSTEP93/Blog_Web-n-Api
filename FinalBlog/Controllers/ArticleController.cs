using FinalBlog.Data.Models;
using FinalBlog.Services;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FinalBlog.Controllers
{
    public class ArticleController(
        IArticleService articleService
        ) : Controller
    {
        readonly IArticleService _articleService = articleService;
        string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        /// <summary>
        /// [GET] Получение всех статей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var articleList = _articleService.GetAllArticles();
            return View("Articles", articleList);
        }

        /// <summary>
        /// [GET] Получение статей автора
        /// </summary>
        /// <param name="authorId">ID автора</param>
        /// <returns></returns>
        [Route("Author")]
        [HttpGet]
        public IActionResult Author(string authorId)
        {
            var articleList = _articleService.GetArticlesOfAuthor(authorId);
            return View("Articles", articleList);
        }

        /// <summary>
        /// [GET] Выбор статей по тегу
        /// </summary>
        /// <param name="id">ID тега</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Tag(int id)
        {
            var articleList = _articleService.GetArticlesByTag(id);
            return View("Articles", articleList);
        }

        /// <summary>
        /// [GET] Просмотр статьи
        /// </summary>
        /// <param name="id">ID статьи</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var article = await _articleService.GetArticleById(id);
            return View("View", article);
        }

        /// <summary>
        /// [GET] Страница создания статьи
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add() => View();

        /// <summary>
        /// [POST] Добавление статьи
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddViewModel model)
        {
            var ifUserCanAdd = _articleService.CheckIfUserCanAdd(User);
            if (!ifUserCanAdd.IsSuccessed)
                return RedirectToAction("AccessDenied", "Error");

            if (ModelState.IsValid)
            {
                var resultModel = await _articleService.AddArticle(model);
                if (resultModel.IsSuccessed)
                    return RedirectToAction("Author", "Article", new { authorId = CurrentUserId });
                
                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return View("Add", model);
        }

        /// <summary>
        /// [GET] Страница редактирования статьи
        /// </summary>
        /// <param name="id">ID статьи</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleService.GetArticleById(id);
            return View("Edit", article);
        }

        /// <summary>
        /// Редактирование статьи
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticleEditViewModel model)
        {
            //model.NewTags = newTags;
            var ifUserCanEdit = _articleService.CheckIfUserCanEdit(User, model.AuthorId);
            if (!ifUserCanEdit.IsSuccessed)
                return RedirectToAction("AccessDenied", "Error");

            if (ModelState.IsValid)
            {
                var resultModel = await _articleService.UpdateArticle(model);
                if (resultModel.IsSuccessed)
                    return RedirectToAction("View", "Article", new { id = model.Id });

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return View("Edit", model);
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="id">ID статьи</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ifUserCanEdit = _articleService.CheckIfUserCanEdit(User, _articleService.GetArticleById(id).Result.AuthorId);
            if (!ifUserCanEdit.IsSuccessed)
                return RedirectToAction("AccessDenied", "Error");

            if (ModelState.IsValid)
            {
                var resultModel = await _articleService.DeleteArticle(id);
                if (resultModel.IsSuccessed)
                    return RedirectToAction("Index", "Article");

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return RedirectToAction("Edit", "Article", new { id });
        }
    }
}
