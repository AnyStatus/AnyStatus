using Newtonsoft.Json.Serialization;
using System;

namespace AnyStatus.Core.Settings
{
    public class ContractResolver : DefaultContractResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ContractResolver(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        protected override JsonObjectContract CreateObjectContract(Type type)
        {
            var contract = base.CreateObjectContract(type);

            contract.DefaultCreator = () => _serviceProvider.GetService(type);

            return contract;
        }
    }
}
