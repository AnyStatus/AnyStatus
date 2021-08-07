using AnyStatus.API.Attributes;
using AnyStatus.API.Endpoints;
using AnyStatus.Apps.Windows.Features.Endpoints;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Pages;
using AnyStatus.Core.App;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    public class EndpointPropertyViewModel : DropDownPropertyViewModel
    {
        public EndpointPropertyViewModel(IMediator mediator, Core.App.IAppContext context, PropertyInfo propertyInfo, object source, IItemsSource itemsSource, IEnumerable<IPropertyViewModel> properties, bool autoload) : base(propertyInfo, source, itemsSource, properties, autoload)
        {
            Commands.Add("Endpoints", new Command(_ => mediator.Send(Page.Show<EndpointsViewModel>("Endpoints", onClose: OnEndpointsViewClosed))));
            Commands.Add("Edit", new Command(_ => Edit(mediator, context), _ => Value is string));
        }

        private void Edit(IMediator mediator, IAppContext context)
        {
            IEndpoint ep = context.Endpoints.FirstOrDefault(e => e.Id.Equals(Value));

            if (ep is not null)
            {
                _ = mediator.Send(new EditEndpoint.Request(ep));
            }
        }

        private void OnEndpointsViewClosed()
        {
            object tmp = Value;

            Load();

            Value = tmp;
        }
    }
}
