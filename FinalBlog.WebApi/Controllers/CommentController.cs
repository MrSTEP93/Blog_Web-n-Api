using FinalBlog.Services.Interfaces;
using FinalBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinalBlog.ViewModels.Comment;
using FinalBlog.Data.Models;
using FinalBlog.Data.ApiModels.Comment;

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
            var response = new List<CommentResponse>();
            if (string.IsNullOrEmpty(authorId))
            {
                response =
                    _commentService.ConvertToApiModel([.. _commentService
                    .GetAllComments().OrderByDescending(x => x.CreationTime)]);
            }
            else
            {
                response =
                    _commentService.ConvertToApiModel([.. _commentService
                    .GetCommentsOfAuthor(authorId).OrderByDescending(x => x.CreationTime)]);
            }

            return Ok(response);
        }

        /// <summary>
        /// Создание комментария
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Add(CommentAddRequest request)
        {
            var ifUserCanAdd = _commentService.CheckIfUserCanAdd(User);
            if (!ifUserCanAdd.IsSuccessed)
                ModelState.AddModelError("", ifUserCanAdd.Messages[0]);

            if (ModelState.IsValid)
            {
                var resultModel = await _commentService
                    .AddComment(_commentService.ConvertToAddViewModel(request));

                if (resultModel.IsSuccessed)
                    return Ok(resultModel);

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return BadRequest(request);
        }

        /// <summary>
        /// Обновление комментария
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(CommentEditRequest request)
        {
            var ifUserCanEdit = _commentService.CheckIfUserCanEdit(User, request.AuthorId);
            if (!ifUserCanEdit.IsSuccessed)
                ModelState.AddModelError("", ifUserCanEdit.Messages[0]);

            if (ModelState.IsValid)
            {
                var resultModel = await _commentService
                    .UpdateComment(_commentService.ConvertToEditViewModel(request));

                if (resultModel.IsSuccessed)
                    return Ok(resultModel);

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return BadRequest(request);
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