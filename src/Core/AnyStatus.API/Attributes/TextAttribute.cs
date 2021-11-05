using System;

namespace AnyStatus.API.Attributes
{
    public class TextAttribute : Attribute
    {
        public TextAttribute(bool wrap = false, bool acceptReturns = false)
        {
            Wrap = wrap;
            AcceptReturns = acceptReturns;
        }

        public bool Wrap { get; set; }

        public bool AcceptReturns { get; set; }
    }
}
