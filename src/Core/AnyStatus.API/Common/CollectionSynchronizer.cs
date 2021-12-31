using System;
using System.Collections.Generic;
using System.Linq;

namespace AnyStatus.API.Common
{
    /// <summary>
    /// Synchronizes two collections.
    /// </summary>
    public class CollectionSynchronizer<TSource, TDestination>
    {
        public Action<TSource> Add { get; set; }
        public Action<TDestination> Remove { get; set; }
        public Action<TSource, TDestination> Update { get; set; }
        public Func<TSource, TDestination, bool> Compare { get; set; }

        /// <summary>
        /// Synchronize two collections.
        /// </summary>
        public void Synchronize(ICollection<TSource> sourceItems, ICollection<TDestination> destinationItems)
        {
            if (sourceItems is null)
            {
                throw new ArgumentNullException(nameof(sourceItems));
            }

            if (destinationItems is null)
            {
                throw new ArgumentNullException(nameof(destinationItems));
            }

            Clean(sourceItems, destinationItems);

            AddOrUpdate(sourceItems, destinationItems);
        }

        /// <summary>
        /// Remove items from destination collection.
        /// </summary>
        private void Clean(ICollection<TSource> sourceCollection, ICollection<TDestination> destinationCollection)
        {
            foreach (var destinationItem in destinationCollection)
            {
                var sourceItem = sourceCollection.FirstOrDefault(item => Compare(item, destinationItem));

                if (sourceItem is null)
                {
                    Remove(destinationItem);
                }
            }
        }

        /// <summary>
        /// Add or update items on  destination collection.
        /// </summary>
        private void AddOrUpdate(ICollection<TSource> sourceCollection, ICollection<TDestination> destinationCollection)
        {
            foreach (var sourceItem in sourceCollection)
            {
                var destinationItem = destinationCollection.FirstOrDefault(item => Compare(sourceItem, item));

                if (destinationItem is null)
                {
                    Add(sourceItem);
                }
                else
                {
                    Update(sourceItem, destinationItem);
                }
            }
        }
    }
}
