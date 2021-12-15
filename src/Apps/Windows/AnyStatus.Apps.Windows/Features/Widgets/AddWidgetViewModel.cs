using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Core.Services;
using AnyStatus.Core.Widgets;
using MediatR;
using System.Collections.Generic;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    public class AddWidgetViewModel : BaseViewModel
    {
        private IWidget _parent;
        private Template _template;
        private Category _category;

        public AddWidgetViewModel(IMediator mediator)
        {
            Commands.Add("Save", new Command(async _ => await mediator.Send(new CreateWidget.Request(Template, Parent)), CanAdd));
        }

        private bool CanAdd(object p) => Category != null && Template != null && Parent != null;

        public IEnumerable<Category> Categories => Scanner.GetWidgetCategories();

        public Category Category
        {
            get => _category;
            set => Set(ref _category, value);
        }

        public Template Template
        {
            get => _template;
            set => Set(ref _template, value);
        }

        public IWidget Parent
        {
            get => _parent;
            set => Set(ref _parent, value);
        }
    }
}
