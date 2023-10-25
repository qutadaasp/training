using AutoMapper;

namespace WebApplication6.helpers
{
    public class mappingprofile : Profile
    {
        public mappingprofile() {
         CreateMap<Movies,MovieDTo>();
            CreateMap<MovieDTo,Movies>().ForMember(src => src.poster, opt => opt.Ignore());
        }
    }
}
