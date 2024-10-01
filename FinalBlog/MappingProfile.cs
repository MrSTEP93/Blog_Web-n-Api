using AutoMapper;
using FinalBlog.Models;
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

        }
    }
}
