using AnyStatus.API.Widgets;
using Docker.DotNet.Models;
using System;

namespace AnyStatus.Plugins.Docker
{
    public static class DockerExtensions
    {
        public static string GetStatus(this ContainerListResponse container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            return container.State switch
            {
                "created" => Status.None,
                "restarting" => Status.None,
                "running" => Status.OK,
                "paused" => Status.Paused,
                "exited" => Status.Stopped,
                "removing" => Status.Unknown,
                "dead" => Status.Unknown,
                _ => Status.Unknown,
            };
        }

        public static string GetName(this ContainerListResponse container) => container is null
                ? throw new ArgumentNullException(nameof(container))
                : container.Names?.Count > 0 ? container.Names[0].Remove(0, 1) : container.ID;

        public static string GetName(this ImagesListResponse container) => container is null
                ? throw new ArgumentNullException(nameof(container))
                : container.RepoTags?.Count > 0 ? container.RepoTags[0] : container.ID;
    }
}
