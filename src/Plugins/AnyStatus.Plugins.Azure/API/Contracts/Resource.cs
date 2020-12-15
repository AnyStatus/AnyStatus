namespace AnyStatus.Plugins.Azure.API.Contracts
{
    public class Resource
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        public string Kind { get; set; }
        
        public string Type { get; set; }

        public string Location { get; set; }
    }
}
