namespace AnyStatus.API.Attributes
{
    public class NameValueItem
    {
        public NameValueItem(string name)
        {
            Name = name;
            Value = name;
        }

        public NameValueItem(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public object Value { get; set; }
    }
}
