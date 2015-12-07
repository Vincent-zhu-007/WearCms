using System;

namespace MobileCms.Cache
{
    public interface ICache
    {
        /// <summary>
        /// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        /// the new item is added. If any failure occurs during this process, the cache will not contain the item being added. 
        /// Items added with this method will be not expire, and will have a Normal priority.
        /// </summary>
        void Add(string key, object value);

        /// <summary>
        /// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        /// the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
        /// </summary>
        void Add(string key, object value, TimeSpan slidingExpiration);

        /// <summary>
        /// Removes the given item from the cache. If no item exists with that key, this method does nothing.
        /// </summary>
        void Remove(string key);

        void Clear();
        /// <summary>
        /// Returns the value associated with the given key.
        /// </summary>
        object Get(string key);
    }
}
