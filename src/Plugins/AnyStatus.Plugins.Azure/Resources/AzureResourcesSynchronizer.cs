using AnyStatus.API.Common;
using AnyStatus.Plugins.Azure.API.Contracts;

namespace AnyStatus.Plugins.Azure.Resources
{
    public class AzureResourcesSynchronizer : CollectionSynchronizer<Resource, AzureResourceWidget>
    {
        public AzureResourcesSynchronizer(AzureResourcesWidget parent)
        {
            Compare = (src, widget) => src.Id.Equals(widget.ResourceId);

            Remove = src => parent.Remove(src);

            Update = (src, widget) =>
            {
                widget.Id = src.Id;
                widget.Name = src.Name;
                widget.Kind = src.Kind;
                widget.Type = src.Type;
                widget.Location = src.Location;
                widget.EndpointId = parent.EndpointId;
            };

            Add = src => parent.Add(
                new AzureResourceWidget
                {
                    Id = src.Id,
                    Name = src.Name,
                    Kind = src.Kind,
                    Type = src.Type,
                    ResourceId = src.Id,
                    Location = src.Location,
                    EndpointId = parent.EndpointId
                });
        }
    }
}
