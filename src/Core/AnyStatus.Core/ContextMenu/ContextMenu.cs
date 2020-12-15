using System.Windows.Input;

namespace AnyStatus.Core.ContextMenu
{
    public abstract class ContextMenu<TContext> : IContextMenu
    {
        public virtual bool IsVisible => true;
        public int Order { get; protected set; }
        public string Icon { get; protected set; }
        public string Name { get; protected set; }
        public virtual bool Break { get; protected set; }
        public bool IsSeparator { get; protected set; }
        public string InputGestureText { get; protected set; }
        public ICommand Command { get; protected set; }
        public TContext Context { get; set; }
    }
}
