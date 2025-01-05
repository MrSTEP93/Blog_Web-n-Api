using FinalBlog.DATA.Models;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace FinalBlog.Controllers
{
    public class ArticleController(IArticleService articleService) : Controller
    {
        readonly IArticleService _articleService = articleService;

        [HttpGet]
        public IActionResult Index()
        {
            var articleList = _articleService.GetAllArticles();
            return View("Articles", articleList);
        }

        [Route("Author")]
        [HttpGet]
        public IActionResult Author(string authorId)
        {
            var articleList = _articleService.GetArticlesOfAuthor(authorId);
            return View("Articles", articleList);
        }

        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var article = await _articleService.GetArticleById(id);
            return View("View", article);
        }

        [HttpGet]
        public IActionResult Add() => View();


        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _articleService.AddArticle(model);
                if (resultModel.IsSuccessed)
                    return RedirectToAction("Author", "Article", User.FindFirstValue(ClaimTypes.NameIdentifier));
                
                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return View("Add", ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleService.GetArticleById(id);
            return View("Edit", article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticleEditViewModel model)
        {
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

        // GET: ArticleController/Delete/5
        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var resultModel = await _articleService.DeleteArticle(id);
            return Ok(resultModel);
        }
    }
}
