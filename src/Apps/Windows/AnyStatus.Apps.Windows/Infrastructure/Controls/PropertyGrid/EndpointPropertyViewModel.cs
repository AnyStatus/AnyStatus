using AnyStatus.API.Attributes;
using AnyStatus.Apps.Windows.Features.Endpoints;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using MediatR;
using System.Collections.Generic;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    public class EndpointPropertyViewModel : DropDownPropertyViewModel
    {
        public EndpointPropertyViewModel(IMediator mediator, PropertyInfo propertyInfo, object source, IItemsSource itemsSource, IEnumerable<IPropertyViewModel> properties, bool autoload) : base(propertyInfo, source, itemsSource, properties, autoload)
        {
            Commands.Add("Endpoints", new Command(_ => mediator.Send(Page.Show<EndpointsViewModel>("Endpoints", onClose: Load))));
        }
    }
}
