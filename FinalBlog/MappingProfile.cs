using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Article;
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

            CreateMap<RoleAddViewModel, Role>();
            CreateMap<RoleEditViewModel, Role>();
            CreateMap<Role, RoleEditViewModel>();

            CreateMap<ArticleViewModel, Article>();
            CreateMap<Article, ArticleViewModel>();

            CreateMap<TagAddViewModel, Tag>();
            CreateMap<TagEditViewModel, Tag>();
            CreateMap<Tag, TagEditViewModel>();
        }
    }
}
