using FinalBlog.DATA.Models;
using FinalBlog.Services;
using FinalBlog.ViewModels.Article;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class ArticleController(IArticleService articleService) : Controller
    {
        IArticleService _articleService = articleService;

        // GET: ArticleController
        public IActionResult Index()
        {
            var articleList = _articleService.GetAllArticles();
            return Ok(articleList);
        }

        // GET: ArticleController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var article = await _articleService.GetArticleById(id);
            return Ok(article);
            //return View();
        }

        // GET: ArticleController/Create
        public async Task<IActionResult> Create(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _articleService.AddArticle(model);
                return Ok(resultModel);
            }
            return BadRequest(ModelState);
        }

        // GET: ArticleController/Edit/5
        public ActionResult Edit(string id)
        {
            return Ok(_articleService.GetArticleById(id));
        }

        // POST: ArticleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel(false);
                return Ok(resultModel);
            }
            return BadRequest(ModelState);
        }

        // GET: ArticleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ArticleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
