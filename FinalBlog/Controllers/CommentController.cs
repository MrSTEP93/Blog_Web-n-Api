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

        public async Task<IActionResult> Details(int id)
        {
            var model = await _commentService.GetCommentById(id);
            return Ok(model);
        }

        public IActionResult Add() => View();

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

        [HttpGet]
        public ActionResult Edit(int id) => View("Edit", _commentService.GetCommentById(id).Result);

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
