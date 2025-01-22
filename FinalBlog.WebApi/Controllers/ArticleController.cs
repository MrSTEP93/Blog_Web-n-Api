using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController(IArticleService articleService) : ControllerBase
    {
        readonly IArticleService _articleService = articleService;

        /// <summary>
        /// [GET] Список всех статей
        /// </summary>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Index()
        {
            var articleList = _articleService.GetAllArticles();
            return Ok(articleList);
        }

        /// <summary>
        /// [GET] Получение статей автора
        /// </summary>
        /// <param name="id">ID автора</param>
        [HttpGet]
        [Route("Author")]
        public IActionResult Author(string id)
        {
            var articleList = _articleService.GetArticlesOfAuthor(id);
            return Ok(articleList);
        }

        /// <summary>
        /// [GET] Выбор статей по тегу
        /// </summary>
        /// <param name="id">ID тега</param>
        [HttpGet]
        [Route("Tag")]
        public IActionResult Tag(int id)
        {
            var articleList = _articleService.GetArticlesByTag(id);
            return Ok(articleList);
        }

        /// <summary>
        /// [GET] Просмотр статьи
        /// </summary>
        /// <param name="id">ID статьи</param>
        [HttpGet]
        [Route("View")]
        public async Task<IActionResult> View(int id)
        {
            var article = await _articleService.GetArticleById(id);
            return Ok(article);
        }

        /// <summary>
        /// [PUT] Добавление статьи
        /// </summary>
        [HttpPut]
        //[ValidateAntiForgeryToken]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] ArticleAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _articleService.AddArticle(model);
                return Ok(resultModel);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// [Post] Редактирование статьи
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] ArticleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _articleService.UpdateArticle(model);
                return Ok(resultModel);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// [Delete] Удаление статьи
        /// </summary>
        /// <param name="id">ID статьи</param>
        [HttpDelete]
        //[ValidateAntiForgeryToken]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultModel = await _articleService.DeleteArticle(id);
            return Ok(resultModel);
        }
    }
}
