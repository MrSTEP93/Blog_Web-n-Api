using FinalBlog.DATA.Models;
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

        public ActionResult Index()
        {
            var model = new CommentListViewModel
            {
                CommentList = [.. _commentService.GetAllComments().OrderByDescending(x => x.CreationTime)],
            };
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

        public ActionResult Edit(int id)
        {
            var model = _commentService.GetCommentById(id).Result;
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CommentEditViewModel model)
        {
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

        // GET: CommentController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
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
