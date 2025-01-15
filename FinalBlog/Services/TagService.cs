using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.Tag;

namespace FinalBlog.Services
{
    public class TagService(
        IMapper mapper,
        IUnitOfWork unitOfWork
        //IArticleService articleService
        ) : ITagService
    {
        readonly IMapper _mapper = mapper;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        //readonly IArticleService _articleService = articleService;

        public async Task<ResultModel> AddTag(TagAddViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
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
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
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
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
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
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tags = repo.GetAll().ToList();
            return CreateListViewModel(tags);
        }

        public async Task<TagEditViewModel> GetTagById(int tagId)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = await repo.Get(tagId);
            return _mapper.Map<TagEditViewModel>(tag);
        }

        public TagListViewModel GetTagsOfArticle(int articleId)
        {
            throw new NotImplementedException();
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
    }
}
