using AnyStatus.API.Widgets;
using AnyStatus.Plugins.Azure.API.Contracts;
using AnyStatus.Plugins.Azure.DevOps.Builds;
using AnyStatus.Plugins.Azure.DevOps.PullRequests;
using AnyStatus.Plugins.Azure.WorkItems;

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

            CreateMap<WorkItem, AzureDevOpsWorkItemWidget>()
                .ForMember(d => d.WorkItemId, opt => opt.MapFrom(src => src.Fields["System.Id"]))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Fields["System.Title"]))
                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.GetStatus()))
                .ForMember(d => d.URL, opt => opt.MapFrom(src => src.Links["html"]["href"]))
                .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Fields["System.WorkItemType"]));
        }
    }
}
