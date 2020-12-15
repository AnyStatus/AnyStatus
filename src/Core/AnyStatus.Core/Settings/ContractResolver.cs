using Newtonsoft.Json.Serialization;
using SimpleInjector;
using System;

namespace AnyStatus.Core.Settings
{
    public class ContractResolver : DefaultContractResolver
    {
        private readonly Container _container;

        public ContractResolver(Container container)
        {
            _container = container;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var contract = base.CreateObjectContract(objectType);

            contract.DefaultCreator = () => _container.GetInstance(objectType);

            return contract;
        }
    }
}
