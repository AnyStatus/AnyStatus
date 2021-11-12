using MediatR;
using System;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Windows
{
    internal class MaterialWindow : IRequest
    {
        private MaterialWindow() { }

        public Type Type { get; private set; }

        public string Title { get; private set; }

        public int? Width { get; private set; }

        public int? MinWidth { get; private set; }

        public int? Height { get; private set; }

        public int? MinHeight { get; private set; }

        public bool Dialog { get; private set; }

        public static MaterialWindow ShowDialog<T>(string title = "", int? width = null, int? height = null, int? minWidth = null, int? minHeight = null)
        {
            return new MaterialWindow
            {
                Type = typeof(T),
                Title = title,
                Width = width,
                MinWidth = minWidth,
                Height = height,
                MinHeight = minHeight,
                Dialog = true,
            };
        }

        public static MaterialWindow Show<T>(string title = "", int? width = null, int? height = null, int? minWidth = null, int? minHeight = null)
        {
            return new MaterialWindow
            {
                Type = typeof(T),
                Title = title,
                Width = width,
                MinWidth = minWidth,
                Height = height,
                MinHeight = minHeight,
            };
        }
    }
}
