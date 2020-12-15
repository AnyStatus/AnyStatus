using System;
using System.Collections.Generic;
using System.Linq;

namespace AnyStatus.API.Common
{
    /// <summary>
    /// Synchronize changes between two collections.
    /// </summary>
    /// <typeparam name="TSource">The source collection (not changed).</typeparam>
    /// <typeparam name="TDestination">The destination collection.</typeparam>
    public class CollectionSynchronizer<TSource, TDestination>
    {
        public Action<TSource> Add { get; set; }
        public Action<TDestination> Remove { get; set; }
        public Action<TSource, TDestination> Update { get; set; }
        public Func<TSource, TDestination, bool> Compare { get; set; }

        /// <summary>
        /// Synchronize changes from source to destination collection.
        /// Source collection is not changed.
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

            RemoveItems(sourceItems, destinationItems);

            AddOrUpdateItems(sourceItems, destinationItems);
        }

        /// <summary>
        /// Remove items from destination collection.
        /// </summary>
        private void RemoveItems(ICollection<TSource> sourceCollection, ICollection<TDestination> destinationCollection)
        {
            foreach (var destinationItem in destinationCollection.ToArray())
            {
                var sourceItem = sourceCollection.FirstOrDefault(item => Compare(item, destinationItem));

                if (sourceItem is null)
                {
                    Remove(destinationItem);
                }
            }
        }

        /// <summary>
        /// Add or update item in destination collection.
        /// </summary>
        /// <param name="sourceCollection"></param>
        /// <param name="destinationCollection"></param>
        private void AddOrUpdateItems(ICollection<TSource> sourceCollection, ICollection<TDestination> destinationCollection)
        {
            var destinationList = destinationCollection.ToList();

            foreach (var sourceItem in sourceCollection)
            {
                var destinationItem = destinationList.FirstOrDefault(item => Compare(sourceItem, item));

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
