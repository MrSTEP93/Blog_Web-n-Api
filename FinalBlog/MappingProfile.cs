using AutoMapper;
using FinalBlog.DATA.Models;
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

            CreateMap<ArticleEditViewModel, Article>();
            CreateMap<ArticleAddViewModel, Article>();
            CreateMap<Article, ArticleViewModel>();
            CreateMap<Article, ArticleEditViewModel>();

            CreateMap<CommentAddViewModel, Comment>();
            CreateMap<CommentEditViewModel, Comment>();
            CreateMap<Comment, CommentEditViewModel>();
            CreateMap<Comment, CommentViewModel>();

            CreateMap<TagAddViewModel, Tag>();
            CreateMap<TagEditViewModel, Tag>();
            CreateMap<Tag, TagViewModel>();
            CreateMap<Tag, TagEditViewModel>();
        }
    }
}
