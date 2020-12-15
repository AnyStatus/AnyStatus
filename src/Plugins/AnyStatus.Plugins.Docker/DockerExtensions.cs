using AnyStatus.API.Widgets;
using Docker.DotNet.Models;
using System;

namespace AnyStatus.Plugins.Docker
{
    public static class DockerExtensions
    {
        public static Status GetStatus(this ContainerListResponse container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            switch (container.State)
            {
                case "created": return Status.None;
                case "restarting": return Status.None;
                case "running": return Status.OK;
                case "paused": return Status.Paused;
                case "exited": return Status.Stopped;
                case "removing": return Status.Unknown;
                case "dead": return Status.Unknown;
                default: return Status.Unknown;
            }
        }

        public static string GetName(this ContainerListResponse container)
        {
            if (container is null) throw new ArgumentNullException(nameof(container));

            return container.Names?.Count > 0 ? container.Names[0].Remove(0, 1) : container.ID;
        }

        public static string GetName(this ImagesListResponse container)
        {
            if (container is null) throw new ArgumentNullException(nameof(container));

            return container.RepoTags?.Count > 0 ? container.RepoTags[0] : container.ID;
        }
    }
}
