﻿using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid;
using AnyStatus.Core.Domain;
using System;

namespace AnyStatus.Apps.Windows.Features.Widgets
{
    internal sealed class WidgetViewModel : BaseViewModel
    {
        public WidgetViewModel(IAppContext context, IPropertyGridViewModel propertyGridViewModel)
        {
            Widget = context?.Session?.SelectedWidget ?? throw new InvalidOperationException("Widget not found.");

            PropertyGridViewModel = propertyGridViewModel;

            PropertyGridViewModel.Target = Widget;
        }

        public IWidget Widget { get; set; }

        public IPropertyGridViewModel PropertyGridViewModel { get; set; }
    }
}
