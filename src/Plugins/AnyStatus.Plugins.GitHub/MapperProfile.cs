using AnyStatus.Plugins.GitHub.API.Models;
using AnyStatus.Plugins.GitHub.Issues;

namespace AnyStatus.Plugins.GitHub
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<GitHubIssue, GitHubIssueWidget>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(d => d.URL, opt => opt.MapFrom(src => src.HtmlUrl))
                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.State));
        }
    }
}
