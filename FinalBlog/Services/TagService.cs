using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.Repositories.Interfaces;
using FinalBlog.DATA.UoW;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.Tag;

namespace FinalBlog.Services
{
    public class TagService(
        IMapper mapper,
        //IUnitOfWork unitOfWork,
        ITagRepository tagRepository
        ) : ITagService
    {
        readonly IMapper _mapper = mapper;
        //readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly ITagRepository repo = tagRepository;

        public async Task<ResultModel> AddTag(TagAddViewModel model)
        {
            //var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var resultModel = new ResultModel(true, "Tag created");
            try
            {
                await repo.Create(_mapper.Map<Tag>(model));
            }
            catch (Exception ex)
            {
                resultModel.MarkAsFailed();
                resultModel.AddMessage(ex.Message); 
                if (ex.InnerException is not null)
                    resultModel.AddMessage(ex.InnerException.Message);
            }
            return resultModel;
        }

        public async Task<ResultModel> UpdateTag(TagEditViewModel model)
        {
            //var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var resultModel = new ResultModel(true, "Tag updated");
            try
            {
                await repo.Update(_mapper.Map<Tag>(model));
            }
            catch (Exception ex)
            {
                resultModel.MarkAsFailed();
                resultModel.AddMessage(ex.Message);
                if (ex.InnerException is not null)
                    resultModel.AddMessage(ex.InnerException.Message);
            }
            return resultModel;
        }

        public async Task<ResultModel> DeleteTag(int tagId)
        {
            //var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = await repo.Get(tagId);
            var resultModel = new ResultModel(true, "Tag deleted");
            try
            {
                await repo.Delete(tag);
            }
            catch (Exception ex)
            {
                resultModel.MarkAsFailed();
                resultModel.AddMessage(ex.Message);
                if (ex.InnerException is not null)
                    resultModel.AddMessage(ex.InnerException.Message);
            }
            return resultModel;
        }

        public TagListViewModel GetAllTags()
        {
            //var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tags = repo.GetAll().ToList();
            //return CreateListViewModel(tags);
            return CreateListViewModel(tags);
        }
        
        public List<TagViewModel> GetAllTagsList()
        {
            //var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tags = repo.GetAll().ToList();
            //return CreateListViewModel(tags);
            return CreateListOfViewModels(tags);
        }

        public async Task<TagEditViewModel> GetTagById(int tagId)
        {
            //var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = await repo.Get(tagId);
            return _mapper.Map<TagEditViewModel>(tag);
        }
        
        public async Task<TagEditViewModel> GetTagByIdAsNoTracking(int tagId)
        {
            //var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = await repo.Get(tagId);
            return _mapper.Map<TagEditViewModel>(tag);
        }

        public TagListViewModel GetTagsOfArticle(int articleId)
        {
            throw new NotImplementedException();
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
                //newItem.ArticleCount = _articleService.GetArticlesByTag(entity.Id).Articles.Count;
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
                //newItem.ArticleCount = _articleService.GetArticlesByTag(entity.Id).Articles.Count;
                model.Add(newItem);
            }
            return model;
        }
    }
}
