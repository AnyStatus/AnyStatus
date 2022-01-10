using AnyStatus.API.Attributes;
using AnyStatus.API.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AnyStatus.API.Widgets
{
    [JsonObject]
    public abstract class Widget : ObservableCollection<IWidget>, IWidget
    {
        #region Fields

        private string _name;
        private string _status;
        private IWidget _parent;
        private bool _isExpanded;
        private bool _isEnabled = true;

#pragma warning disable IDE0051
        [JsonProperty] private IList<IWidget> Data => Items;
#pragma warning restore IDE0051

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
        public string Status
        {
            get => _status;
            set
            {
                if (IsEnabled && Set(ref _status, value))
                {
                    PreviousStatus = _status;
                }
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public string PreviousStatus { get; private set; }

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
        /// When true, child widgets are saved and persisted between sessions. Default is false.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public bool IsPersisted { get; set; }

        /// <summary>
        /// When true, status is aggregated by priority. Default is false.
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public bool IsAggregate { get; protected set; }

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

            _ = WidgetNotifications.PublishAsync(new WidgetAddedNotification(widget));

            Reassess();
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

            _ = WidgetNotifications.PublishAsync(new WidgetDeletedNotification(widget));

            Reassess();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName.Equals(nameof(Status)) && Status != PreviousStatus && Parent is object)
            {
                Parent.Reassess();
            }
        }

        protected virtual bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            oldValue = newValue;

            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            return true;
        }

        #endregion

        #region Public Methods

        public void Reassess()
        {
            if (IsAggregate)
            {
                Status = Count > 0 ? this.Aggregate((a, b) => Widgets.Status.Priority(a.Status) < Widgets.Status.Priority(b.Status) ? a : b).Status : Widgets.Status.None;
            }
        }

        public void Expand() => IsExpanded = true;

        public bool ShouldSerializeData() => IsPersisted;

        public virtual object Clone()
        {
            var clone = (Widget)Activator.CreateInstance(GetType());

            var properties = GetType()
                .GetProperties()
                .Where(p => p.CanWrite && p.Name != nameof(Id) && p.Name != nameof(Parent) && p.Name != "Item")
                .ToList();

            properties.ForEach(p => p.SetValue(clone, p.GetValue(this, null), null));

            clone.Id = Guid.NewGuid().ToString();

            if (HasChildren)
            {
                foreach (var child in this)
                {
                    clone.Add((IWidget)child.Clone());
                }
            }

            return clone;
        }

        public bool IsConfigurable() => this is IConfigurable && GetType().GetProperties().Any(p => p.IsDefined(typeof(RequiredAttribute)) && p.GetValue(this) is null);

        #endregion
    }
}
