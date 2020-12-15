using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API.Contracts;
using AnyStatus.Plugins.Azure.DevOps.Builds;
using AnyStatus.Plugins.Azure.DevOps.PullRequests;

namespace AnyStatus.Plugins.Azure
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<Build, AzureDevOpsPipelineWidget>()
                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.GetStatus ?? Status.Unknown))
                .ForMember(d => d.Branch, opt => opt.MapFrom(src => src.SourceBranch))
                .ForMember(d => d.URL, opt => opt.MapFrom(src => src.Links["web"]["href"]));

            CreateMap<GitPullRequest, AzureDevOpsPullRequestWidget>()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.GetStatus()));
        }
    }
}
