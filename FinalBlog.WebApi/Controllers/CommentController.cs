using FinalBlog.Services.Interfaces;
using FinalBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinalBlog.ViewModels.Comment;
using FinalBlog.Data.Models;

namespace FinalBlog.WebApi.Controllers
{
    /// <summary>
    /// Контроллер комментариев
    /// </summary>
    /// <param name="commentService">Подключаемый из DI сервис для работы с комментариями</param>
    [ApiController]
    [Route("[controller]/[action]")]
    public class CommentController(
        ICommentService commentService
        ) : ControllerBase
    {
        readonly ICommentService _commentService = commentService;

        /// <summary>
        /// Отображение списка комментариев
        /// </summary>
        /// <param name="authorId">если заполнено - комменты этого автора</param>
        [HttpGet]
        public IActionResult Index(string? authorId = null)
        {
            var model = new CommentListViewModel();
            if (string.IsNullOrEmpty(authorId))
            {
                model.CommentList = [.. _commentService.GetAllComments().OrderByDescending(x => x.CreationTime)];
            }
            else
            {
                model.CommentList = [.. _commentService.GetCommentsOfAuthor(authorId).OrderByDescending(x => x.CreationTime)];
            }

            model.CommentsCount = model.CommentList.Count;
            return Ok(model);
        }

        /// <summary>
        /// Создание комментария
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Add(CommentAddViewModel model)
        {
            var ifUserCanAdd = _commentService.CheckIfUserCanAdd(User);
            if (!ifUserCanAdd.IsSuccessed)
                ModelState.AddModelError("", ifUserCanAdd.Messages[0]);

            if (ModelState.IsValid)
            {
                var resultModel = await _commentService.AddComment(model);
                if (resultModel.IsSuccessed)
                    return Ok(resultModel);

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return BadRequest(model);
        }

        /// <summary>
        /// Обновление комментария
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(CommentEditViewModel model)
        {
            var ifUserCanEdit = _commentService.CheckIfUserCanEdit(User, model.AuthorId);
            if (!ifUserCanEdit.IsSuccessed)
                ModelState.AddModelError("", ifUserCanEdit.Messages[0]);

            if (ModelState.IsValid)
            {
                var resultModel = await _commentService.UpdateComment(model);
                if (resultModel.IsSuccessed)
                    return Ok(resultModel);

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return BadRequest(model);
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var ifUserCanEdit = _commentService.CheckIfUserCanEdit(User, _commentService.GetCommentById(id).Result.AuthorId);
            if (!ifUserCanEdit.IsSuccessed)
                ModelState.AddModelError("", ifUserCanEdit.Messages[0]);

            if (ModelState.IsValid)
            {
                var resultModel = await _commentService.DeleteComment(id);
                if (resultModel.IsSuccessed)
                    return Ok(resultModel);

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return BadRequest(ModelState);
        }
    }
}