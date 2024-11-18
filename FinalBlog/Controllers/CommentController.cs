using FinalBlog.Services;
using FinalBlog.ViewModels.Comment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class CommentController(
        ICommentService commentService
        ) : Controller
    {
        readonly ICommentService _commentService = commentService;

        public ActionResult Index()
        {
            var list = _commentService.GetAllComments();
            return Ok(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _commentService.GetCommentById(id);
            return Ok(model);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CommentAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.AddComment(model);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPut]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CommentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.UpdateComment(model);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        // GET: CommentController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: CommentController/Delete/5
        [HttpDelete]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.DeleteComment(id);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}
