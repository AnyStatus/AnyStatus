﻿using AnyStatus.API.Widgets;
using AnyStatus.Apps.Windows.Features.Widgets;
using AnyStatus.Apps.Windows.Infrastructure.Mvvm;
using AnyStatus.Core.ContextMenu;
using AnyStatus.Core.Domain;
using MediatR;

namespace AnyStatus.Apps.Windows.Features.ContextMenu.Items
{
    public class AddFolder<TWidget> : ContextMenu<TWidget> where TWidget : IAddFolder
    {
        public AddFolder(IMediator mediator, IAppContext context)
        {
            Order = 120;
            Name = "Add Folder";
            Command = new Command(_ => mediator.Send(new AddFolder.Request(context.Session.SelectedWidget ?? context.Session.Widget)));
        }
    }
}
