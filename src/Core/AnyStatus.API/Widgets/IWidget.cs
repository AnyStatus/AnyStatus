using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AnyStatus.API.Widgets
{
    public interface IWidget : IList<IWidget>, ICloneable, INotifyPropertyChanged
    {
        string Id { get; set; }

        string Name { get; set; }

        bool IsEnabled { get; set; }

        IWidget Parent { get; set; }

        bool HasChildren { get; }

        bool IsExpanded { get; set; }

        Status Status { get; set; }

        Status PreviousStatus { get; }

        void Move(int x, int y);

        void Expand();

        void Reassessment();
    }
}
