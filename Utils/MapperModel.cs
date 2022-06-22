using AutoMapper;
using blog_api_dev.Models.Article;
using blog_api_dev.Models.Tech;
using blog_api_dev.Models.Tech_Article;
using blog_api_dev.Models.User;

namespace blog_api_dev.Utils
{
  public class MapperModel : Profile
  {
    public MapperModel()
    {
      CreateMap<UserPost, User>();
      CreateMap<UserDTO, User>()
        .ForMember(obj => obj.password_key, opt => opt.Ignore())
        .ForMember(obj => obj.password, opt => opt.Ignore());
      CreateMap<TechnologyPost, Technology>();
      CreateMap<ArticlePost, Article>();
      CreateMap<Tech_Article_Post, Tech_Article>();
    }
  }
}