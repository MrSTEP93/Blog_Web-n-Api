using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.ViewModels;
using FinalBlog.ViewModels.Article;

namespace FinalBlog.Services
{
    public class TagService(
        IMapper mapper,
        IUnitOfWork unitOfWork
        ) : ITagService
    {
        readonly IMapper _mapper = mapper;
        readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResultModel> AddTag(TagViewModel model)
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

        public async Task<ResultModel> UpdateTag(TagViewModel model)
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

        public List<TagViewModel> GetAllTags()
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tags = repo.GetAll().ToList();
            return CreateListOfViewModel(tags);
        }

        public async Task<TagViewModel> GetTagById(int tagId)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = await repo.Get(tagId);
            return _mapper.Map<TagViewModel>(tag);
        }

        public List<TagViewModel> GetTagsOfArticle(int tagId)
        {
            throw new NotImplementedException();
        }

        private List<TagViewModel> CreateListOfViewModel(List<Tag> list)
        {
            var model = new List<TagViewModel>();
            foreach (var entity in list)
            {
                model.Add(_mapper.Map<TagViewModel>(entity));
            }

            return model;
        }
    }
}
