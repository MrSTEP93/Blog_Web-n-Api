using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.ViewModels;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.Role;
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

            CreateMap<RoleViewModel, Role>();
            CreateMap<Role, RoleViewModel>();

            CreateMap<ArticleViewModel, Article>();
            CreateMap<Article, ArticleViewModel>();

            CreateMap<TagViewModel, Tag>();
            CreateMap<Tag, TagViewModel>();
        }
    }
}
