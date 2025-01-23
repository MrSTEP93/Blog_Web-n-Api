using FinalBlog.Services.Interfaces;
using FinalBlog.Services;
using Microsoft.AspNetCore.Mvc;
using FinalBlog.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;

namespace FinalBlog.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TagController(ITagService tagService) : ControllerBase
    {
        private readonly ITagService _tagService = tagService;

        /// <summary>
        /// [GET] Вывод всех тегов
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            var list = _tagService.GetAllTags();
            return Ok(list);
        }

        /// <summary>
        /// [GET] Просмотр тега
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var model = await _tagService.GetTagById(id);
            if (model == null)
                return BadRequest($"Tag id={id} doesn't exsists");
            return Ok(model);
        }

        /// <summary>
        /// [POST] Добавление тега
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add(TagAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.AddTag(model);
                return Ok(result);
            }
            return BadRequest(model);
        }

        /// <summary>
        /// [PUT] Редактирование
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.UpdateTag(model);
                return Ok(result);
            }
            return BadRequest(model);
        }

        [Authorize(Roles = "Администратор, Модератор")]
        /// <summary>
        /// [POST] Удаление тега
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagService.DeleteTag(id);
                if (result.IsSuccessed)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            return BadRequest(ModelState);
        }
    }
}
