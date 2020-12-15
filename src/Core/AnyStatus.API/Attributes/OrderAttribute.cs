using System;

namespace AnyStatus.API.Attributes
{
    public class OrderAttribute : Attribute
    {
        public OrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }
    }
}
