using System.Collections.Generic;

namespace AnyStatus.Plugins.Azure.API.Contracts
{
    internal class Release
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public IdentityRef CreatedBy { get; set; }

        public IEnumerable<Environment> Environments { get; set; }

        public string Text
        {
            get
            {
                return $"Release {Name}\nStatus {Status}\n{(Description.Length > 0 ? Description.Remove(Description.Length - 1) : "")}\nCreated by {CreatedBy?.DisplayName}";
            }
        }
    }
}
