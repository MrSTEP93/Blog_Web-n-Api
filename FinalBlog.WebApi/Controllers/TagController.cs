using FinalBlog.Services.Interfaces;
using FinalBlog.Services;
using Microsoft.AspNetCore.Mvc;
using FinalBlog.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;

namespace FinalBlog.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tagService">Подключаемый из DI сервис для работы с</param>
    [ApiController]
    [Route("[controller]/[action]")]
    public class TagController(ITagService tagService) : ControllerBase
    {
        private readonly ITagService _tagService = tagService;

        /// <summary>
        /// Вывод всех тегов
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            var list = _tagService.GetAllTags();
            return Ok(list);
        }

        /// <summary>
        /// Добавление тега
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
        /// Редактирование
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

        /// <summary>
        /// Удаление тега
        /// </summary>
        [Authorize(Roles = "Администратор, Модератор")]
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
