using AutoMapper;
using FinalBlog.Data.Models;
using FinalBlog.Data.Repositories.Interfaces;
//using FinalBlog.DATA.Repositories;
//using FinalBlog.DATA.UoW;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.Tag;

namespace FinalBlog.Services
{
    public class TagService(
        IMapper mapper,
        ITagRepository tagRepository,
        ILogger<TagService> logger
        ) : ITagService
    {
        readonly IMapper _mapper = mapper;
        readonly ITagRepository repo = tagRepository;
        readonly ILogger<TagService> _logger = logger;

        public async Task<ResultModel> AddTag(TagAddViewModel model)
        {
            var resultModel = new ResultModel(true, "Tag created");
            try
            {
                await repo.Create(_mapper.Map<Tag>(model));
                _logger.LogInformation($"Добавлен тег {model.Name}");
            }
            catch (Exception ex)
            {
                resultModel = ProcessException($"При добавлении тега {model.Name} произошла ошибка", ex);
            }
            return resultModel;
        }

        public async Task<ResultModel> UpdateTag(TagEditViewModel model)
        {
            var resultModel = new ResultModel(true, "Tag updated");
            try
            {
                await repo.Update(_mapper.Map<Tag>(model));
                _logger.LogInformation($"Обновлен тег {model.Name}");
            }
            catch (Exception ex)
            {
                resultModel = ProcessException($"При обновлении тега {model.Name} произошла ошибка", ex);
            }
            return resultModel;
        }

        public async Task<ResultModel> DeleteTag(int tagId)
        {
            var tag = await repo.Get(tagId);
            var resultModel = new ResultModel(true, "Tag deleted");
            try
            {
                await repo.Delete(tag);
                _logger.LogInformation($"Удален тег id={tagId}");
            }
            catch (Exception ex)
            {
                resultModel = ProcessException($"При удалении тега id={tagId} произошла ошибка", ex);
            }
            return resultModel;
        }

        public TagListViewModel GetAllTags()
        {
            var tags = repo.GetAll().ToList();
            return CreateListViewModel(tags);
        }
        
        public List<TagViewModel> GetAllTagsList()
        {
            var tags = repo.GetAll().ToList();
            return CreateListOfViewModels(tags);
        }

        public async Task<TagEditViewModel> GetTagById(int tagId)
        {
            var tag = await repo.Get(tagId);
            return _mapper.Map<TagEditViewModel>(tag);
        }
        
        public async Task<TagEditViewModel> GetTagByIdAsNoTracking(int tagId)
        {
            var tag = await repo.Get(tagId);
            return _mapper.Map<TagEditViewModel>(tag);
        }

        public async Task<List<Tag>> GetTagsByIds(List<int> selectedIds)
        {
            var list = new List<Tag>();
            foreach (var id in selectedIds)
                list.Add(_mapper.Map<Tag>(await GetTagById(id)));

            return list;
        }

        private TagListViewModel CreateListViewModel(List<Tag> list)
        {
            var model = new TagListViewModel();
            foreach (var entity in list)
            {
                var newItem = _mapper.Map<TagViewModel>(entity);
                model.Tags.Add(newItem);
            }
            return model;
        }

        private List<TagViewModel> CreateListOfViewModels(List<Tag> list)
        {
            var model = new List<TagViewModel>();
            foreach (var entity in list)
            {
                var newItem = _mapper.Map<TagViewModel>(entity);
                model.Add(newItem);
            }
            return model;
        }
                
        /// <summary>
        /// Глобальный обработчик ошибок (если можно так выразиться xD)
        /// глобальный для этого класса (другое не успел протестировать и внедрить)
        /// </summary>
        /// <param name="logMessage">Сообщение для записи в лог</param>
        /// <param name="ex">полученное исключение </param>
        /// <returns></returns>
        private ResultModel ProcessException(string logMessage, Exception ex)
        {
            var resultModel = new ResultModel();
            resultModel.MarkAsFailed(ex.Message);
            _logger.LogError($"{logMessage}: {ex.Message}");
            if (ex.InnerException is not null)
            {
                _logger.LogError($" --- InnerException: {ex.InnerException.Message}");
                resultModel.AddMessage(ex.InnerException.Message);
            }
            return resultModel;
        }
    }
}
