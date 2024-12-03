using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Tag;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class TagController(ITagService tagService) : Controller
    {
        readonly ITagService _tagService = tagService;

        public IActionResult Index()
        {
            var list = _tagService.GetAllTags();
            return Ok(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _tagService.GetTagById(id);
            return Ok(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return BadRequest();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TagAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.AddTag(model);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TagController/Edit/5
        [HttpPut]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.UpdateTag(model);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        // GET: TagController/Delete/5
        public ActionResult Delete()
        {
            return View();
        }

        [HttpDelete]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.DeleteTag(id);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}
