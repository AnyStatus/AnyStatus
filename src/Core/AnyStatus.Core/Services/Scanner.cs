using AnyStatus.API.Widgets;
using AnyStatus.Core.Domain;
using AnyStatus.Core.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AnyStatus.Core.Services
{
    public class Scanner : IScanner
    {
        private static readonly Lazy<IEnumerable<Assembly>> _assemblies = new Lazy<IEnumerable<Assembly>>(LoadAssemblies);

        public static IEnumerable<Assembly> GetAssemblies() => _assemblies.Value;


        public static IEnumerable<Type> GetTypesOf<T>(bool browsableOnly = true) => from assembly in GetAssemblies()
                                                                                    from type in assembly.GetTypes()
                                                                                    where type.IsClass && !type.IsAbstract && typeof(T).IsAssignableFrom(type) && (!browsableOnly || type.IsBrowsable())
                                                                                    select type;
        public static IEnumerable<Category> GetWidgetCategories() => from type in GetTypesOf<IWidget>()
                                                                     let categoryAttribute = type.GetCustomAttribute<CategoryAttribute>()
                                                                     where categoryAttribute != null && !string.IsNullOrEmpty(categoryAttribute.Category)
                                                                     let template = new Template(type)
                                                                     group template by categoryAttribute.Category into category
                                                                     select new Category
                                                                     {
                                                                         Name = category.Key,
                                                                         Templates = category.ToList()
                                                                     };
        private static List<Assembly> LoadAssemblies()
        {
            var query = from fileInfo in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles()
                        where fileInfo.Extension.ToLower().Equals(".dll") && fileInfo.Name.Contains("AnyStatus")
                        select Assembly.Load(AssemblyName.GetAssemblyName(fileInfo.FullName));

            var assemblies = query.ToList();

            assemblies.Add(typeof(IMediator).GetTypeInfo().Assembly);

            return assemblies;
        }
    }
}
