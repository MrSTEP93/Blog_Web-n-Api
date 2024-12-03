using FinalBlog.DATA.Models;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalBlog.Controllers
{
    public class ArticleController(IArticleService articleService) : Controller
    {
        readonly IArticleService _articleService = articleService;

        [HttpGet]
        public IActionResult Index()
        {
            var articleList = _articleService.GetAllArticles();
            return Ok(articleList);
        }

        [Route("Author")]
        [HttpGet]
        public IActionResult Author(string id)
        {
            var articleList = _articleService.GetArticlesOfAuthor(id);
            return Ok(articleList);
        }

        [HttpGet]
        public async Task<ArticleEditViewModel> Details(int id)
        {
            var article = await _articleService.GetArticleById(id);
            return article;
            //return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _articleService.AddArticle(model);
                return Ok(resultModel);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<ArticleEditViewModel> Edit(int id)
        {
            return await _articleService.GetArticleById(id);
        }

        [HttpPut]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticleEditViewModel model)
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

        [HttpDelete]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var resultModel = await _articleService.DeleteArticle(id);
            return Ok(resultModel);
        }
    }
}
