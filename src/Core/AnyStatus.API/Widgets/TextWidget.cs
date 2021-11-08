using System.ComponentModel;

namespace AnyStatus.API.Widgets
{
    public abstract class TextWidget : StatusWidget
    {
        private string _text;

        [Browsable(false)]
        public string Text
        {
            get => _text;
            set => Set(ref _text, value);
        }
    }
}
