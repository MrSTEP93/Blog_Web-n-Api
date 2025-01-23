using AutoMapper;
using FinalBlog.Data.ApiModels.Articles;
using FinalBlog.Data.ApiModels.Comment;
using FinalBlog.Data.Models;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.Comment;
using FinalBlog.ViewModels.Role;
using FinalBlog.ViewModels.Tag;
using FinalBlog.ViewModels.User;

namespace FinalBlog
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationViewModel, BlogUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<LoginViewModel, BlogUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<BlogUser, AuthorViewModel>();
            CreateMap<BlogUser, UserViewModel>();

            CreateMap<RoleAddViewModel, Role>();
            CreateMap<RoleViewModel, Role>();
            CreateMap<Role, RoleViewModel>();

            #region Article
            CreateMap<ArticleEditViewModel, Article>();
            CreateMap<ArticleAddViewModel, Article>();
            CreateMap<Article, ArticleViewModel>();
            CreateMap<Article, ArticleEditViewModel>();

            CreateMap<ArticleViewModel, ArticleResponse>();
            CreateMap<ArticleEditViewModel, ArticleResponse>();
            CreateMap<ArticleAddRequest, ArticleAddViewModel>();
            CreateMap<ArticleEditRequest, ArticleEditViewModel>();
            #endregion

            #region Comment
            CreateMap<CommentAddViewModel, Comment>();
            CreateMap<CommentEditViewModel, Comment>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<Comment, CommentEditViewModel>();

            CreateMap<CommentViewModel, CommentResponse>();
            CreateMap<CommentEditViewModel, CommentResponse>();
            CreateMap<CommentAddRequest, CommentAddViewModel>();
            CreateMap<CommentEditRequest, CommentEditViewModel>();
            #endregion

            CreateMap<TagAddViewModel, Tag>();
            CreateMap<TagEditViewModel, Tag>();
            CreateMap<Tag, TagViewModel>()
                .ForMember(dest => dest.ArticleCount, opt => opt.MapFrom(src => src.Articles.Count));
            CreateMap<Tag, TagEditViewModel>();
        }
    }
}
