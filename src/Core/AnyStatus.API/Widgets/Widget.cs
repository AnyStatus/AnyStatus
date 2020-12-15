using AnyStatus.API.Attributes;
using AnyStatus.API.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AnyStatus.API.Widgets
{
    [JsonObject]
    public abstract class Widget : ObservableCollection<IWidget>, IWidget
    {
        public Widget()
        {
            Id = Guid.NewGuid().ToString();
        }

#pragma warning disable IDE0051
        [JsonProperty] private IList<IWidget> Data => Items;
        public bool ShouldSerializeData() => IsPersisted;
#pragma warning restore IDE0051

        #region Fields

        private string _name;
        private string _hint;
        private IWidget _parent;
        private bool _isExpanded;
        private bool _isEnabled = true;
        private Status _status = Status.None;
        private WidgetNotificationSettings _notificationSettings;

        #endregion

        #region Properties

        [Browsable(false)]
        public string Id { get; set; }

        [Required]
        [Order(1)]
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        [Browsable(false)]
        public string Hint
        {
            get => _hint;
            set => Set(ref _hint, value);
        }

        [Browsable(false)]
        public IWidget Parent
        {
            get => _parent;
            set => Set(ref _parent, value);
        }

        [JsonIgnore]
        [Browsable(false)]
        public bool HasChildren => Count > 0;

        [JsonIgnore]
        [Browsable(false)]
        public Status Status
        {
            get => _status;
            set
            {
                PreviousStatus = _status;
                Set(ref _status, value);
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public Status PreviousStatus { get; private set; }

        [Browsable(false)]
        public bool IsExpanded
        {
            get => _isExpanded;
            set => Set(ref _isExpanded, value);
        }

        [Browsable(false)]
        [DisplayName("Enabled")]
        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(ref _isEnabled, value);
        }

        /// <summary>
        /// Determines whether the widget children are persisted.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public bool IsPersisted { get; set; } = true;

        [Browsable(false)]
        [DisplayName("Notification Settings")]
        public WidgetNotificationSettings NotificationsSettings
        {
            get => _notificationSettings;
            set => Set(ref _notificationSettings, value);
        }

        #endregion

        #region Protected

        public new void Add(IWidget widget)
        {
            widget.Parent = this;

            base.Add(widget);
        }

        protected override void InsertItem(int index, IWidget widget)
        {
            base.InsertItem(index, widget);

            widget.Parent = this;

            WidgetNotifications.PublishAsync(new WidgetAddedNotification(widget));

            Reassessment();
        }

        protected override void RemoveItem(int index)
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            var widget = this[index];

            widget.Parent = null;

            base.RemoveItem(index);

            WidgetNotifications.PublishAsync(new WidgetDeletedNotification(widget));

            Reassessment();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName.Equals(nameof(Status)) && Status != PreviousStatus && Parent is object)
            {
                Parent.Reassessment();
            }
        }

        protected virtual bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(oldValue, newValue))
            {
                return false;
            }

            oldValue = newValue;

            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            return true;
        }

        #endregion

        //public IEnumerable<IWidget> Descendants()
        //{
        //    var nodes = new Stack<IWidget>(new[] { this });
        //    while (nodes.Any())
        //    {
        //        var widget = nodes.Pop();
        //        yield return widget;
        //        foreach (var n in widget)
        //        {
        //            nodes.Push(n);
        //        }
        //    }
        //}

        public void Reassessment() => Status = Count > 0 ? this.Aggregate((a, b) => a.Status?.Metadata?.Priority < b.Status?.Metadata?.Priority ? a : b)?.Status : Status.None;

        public virtual object Clone()
        {
            var clone = (Widget)Activator.CreateInstance(GetType());

            var properties = GetType()
                .GetProperties()
                .Where(p => p.CanWrite && p.Name != nameof(Id) && p.Name != nameof(Parent) && p.Name != "Item")
                .ToList();

            properties.ForEach(p => p.SetValue(clone, p.GetValue(this, null), null));

            if (HasChildren) foreach (var child in this) clone.Add((IWidget)child.Clone());

            return clone;
        }
    }
}
