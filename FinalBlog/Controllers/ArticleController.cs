using FinalBlog.DATA.Models;
using FinalBlog.Services;
using FinalBlog.ViewModels.Article;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalBlog.Controllers
{
    public class ArticleController(IArticleService articleService) : Controller
    {
        IArticleService _articleService = articleService;

        // GET: ArticleController
        [HttpGet]
        public IActionResult Index()
        {
            var articleList = _articleService.GetAllArticles();
            return Ok(articleList);
        }

        // GET: ArticleController/Details/5
        [HttpGet]
        public async Task<ArticleViewModel> Details(int id)
        {
            var article = await _articleService.GetArticleById(id);
            return article;
            //return View();
        }

        // POST: ArticleController/Add
        [HttpPost]
        public async Task<IActionResult> Add(ArticleViewModel model)
        {
            //model.CreationTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                var resultModel = await _articleService.AddArticle(model);
                return Ok(resultModel);
            }
            return BadRequest(ModelState);
        }

        // GET: ArticleController/Edit/5
        [HttpGet]
        public async Task<ArticleViewModel> Edit(int id)
        {
            return await _articleService.GetArticleById(id);
        }

        // POST: ArticleController/Edit/5
        //[ValidateAntiForgeryToken]
        [HttpPut]
        public async Task<IActionResult> Edit(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _articleService.UpdateArticle(model);
                return Ok(resultModel);
            }
            return BadRequest(ModelState);
        }

        // GET: ArticleController/Delete/5
        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}

        // DELETE: ArticleController/Delete/5
        //[ValidateAntiForgeryToken]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var resultModel = await _articleService.DeleteArticle(id);
            return Ok(resultModel);
        }
    }
}
