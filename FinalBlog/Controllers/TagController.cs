using FinalBlog.Data.Models;
using FinalBlog.Services;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Tag;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace FinalBlog.Controllers
{
    public class TagController(ITagService tagService) : Controller
    {
        readonly ITagService _tagService = tagService;

        /// <summary>
        /// [GET] Вывод всех тегов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var model = _tagService.GetAllTags();
            return View("TagList", model);
        }

        /// <summary>
        /// [GET] Просмотр тега
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var model = await _tagService.GetTagById(id);
            return View(model);
        }

        /// <summary>
        /// [GET] Показ окна добавления
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add() => View();

        /// <summary>
        /// [POST] Добавление тега
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TagAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _tagService.AddTag(model);
                if (resultModel.IsSuccessed)
                    return RedirectToAction("Index", "Tag");

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return View("Add", model);
        }

        /// <summary>
        /// [GET] Показ окна редактирования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id) => View("Edit", _tagService.GetTagById(id).Result);

        /// <summary>
        /// [POST] Редактирование
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _tagService.UpdateTag(model);
                if (resultModel.IsSuccessed)
                    return RedirectToAction("Index", "Tag");

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return View("Edit", model);
        }

        /// <summary>
        /// [POST] Удаление тега
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _tagService.DeleteTag(id);
                if (resultModel.IsSuccessed)
                    return RedirectToAction("Index", "Tag");

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            return RedirectToAction("Edit", "Tag");
        }
    }
}
