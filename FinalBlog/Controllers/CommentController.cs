using FinalBlog.DATA.Models;
using FinalBlog.Services;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Comment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalBlog.Controllers
{
    public class CommentController(
        ICommentService commentService
        ) : Controller
    {
        readonly ICommentService _commentService = commentService;
        
        /// <summary>
        /// [GET] Отображение списка статей
        /// </summary>
        /// <param name="authorId">если заполнено - статьи этого автора</param>
        /// <param name="authorFullName">имя автора для отображения в заголовке</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(string? authorId = null, string? authorFullName = null)
        {
            var model = new CommentListViewModel();
            if (string.IsNullOrEmpty(authorId))
            {
                model.CommentList = [.. _commentService.GetAllComments().OrderByDescending(x => x.CreationTime)];
            } else
            {
                model.CommentList = [.. _commentService.GetCommentsOfAuthor(authorId).OrderByDescending(x => x.CreationTime)];
                model.AuthorFullName = authorFullName;
            }
            
            model.CommentsCount = model.CommentList.Count;
            return View("CommentsList", model);
        }

        /// <summary>
        /// [GET] Страница создания комментария
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add() => View();

        /// <summary>
        /// [POST] Создание комментари
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CommentAddViewModel model)
        {
            var ifUserCanAdd = _commentService.CheckIfUserCanAdd(User);
            if (!ifUserCanAdd.IsSuccessed)
                ModelState.AddModelError("", ifUserCanAdd.Messages[0]);

            model.CreationTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                var resultModel = await _commentService.AddComment(model);
                if (resultModel.IsSuccessed)
                    return RedirectToAction("View", "Article", new { id = model.ArticleId });

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return View("Add", model);
        }

        /// <summary>
        /// [GET] Страница редактирования комментария
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id) => View("Edit", _commentService.GetCommentById(id).Result);

        /// <summary>
        /// [POST] Обновление комментария
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CommentEditViewModel model)
        {
            var ifUserCanEdit = _commentService.CheckIfUserCanEdit(User, model.AuthorId);
            if (!ifUserCanEdit.IsSuccessed)
                ModelState.AddModelError("", ifUserCanEdit.Messages[0]);

            if (ModelState.IsValid)
            {
                var resultModel = await _commentService.UpdateComment(model);
                if (resultModel.IsSuccessed)
                    return RedirectToAction("View", "Article", new { id = model.ArticleId });

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return View("Edit", model);
        }

        /// <summary>
        /// [POST] Удаление комментария
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ifUserCanEdit = _commentService.CheckIfUserCanEdit(User, _commentService.GetCommentById(id).Result.AuthorId);
            if (!ifUserCanEdit.IsSuccessed)
                ModelState.AddModelError("", ifUserCanEdit.Messages[0]);

            if (ModelState.IsValid)
            {
                var resultModel = await _commentService.DeleteComment(id);
                if (resultModel.IsSuccessed)
                    return RedirectToAction("Index", "Article");

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return RedirectToAction("Edit", "Comment", new { id });
        }
    }
}
