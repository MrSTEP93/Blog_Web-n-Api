using FinalBlog.DATA.Models;
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

        public IActionResult Index()
        {
            var model = _tagService.GetAllTags();
            return View("TagList", model);
        }

        public async Task<IActionResult> View(int id)
        {
            var model = await _tagService.GetTagById(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Add() => View();

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

        [HttpGet]
        public ActionResult Edit(int id) => View("Edit", _tagService.GetTagById(id).Result);

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
