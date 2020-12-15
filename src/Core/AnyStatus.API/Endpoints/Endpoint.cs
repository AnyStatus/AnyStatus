using AnyStatus.API.Attributes;
using AnyStatus.API.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnyStatus.API.Endpoints
{
    public abstract class Endpoint : NotifyPropertyChanged, IEndpoint
    {
        private string _id;
        private string _name;
        private string _address;

        [Browsable(false)]
        public string Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        [Required]
        [Order(1)]
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        [Required]
        public string Address
        {
            get => _address;
            set => Set(ref _address, value);
        }
    }
}
