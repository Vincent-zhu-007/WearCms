using System;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using MobileCms.Cache;

namespace MobileCms.CacheService
{
    public class Cache : ICache
    {
        private readonly ICacheManager cache;

        public Cache()
        {
            this.cache = CacheFactory.GetCacheManager();
        }

        /// <summary>
        /// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        /// the new item is added. If any failure occurs during this process, the cache will not contain the item being added. 
        /// Items added with this method will be not expire, and will have a Normal priority.
        /// </summary>
        public void Add(string key, object value)
        {
            this.cache.Add(key, value);
        }

        /// <summary>
        /// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        /// the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
        /// </summary>
        public void Add(string key, object value, TimeSpan slidingExpiration)
        {
            this.cache.Add(key, value, CacheItemPriority.Normal, null, new SlidingTime(slidingExpiration));
        }

        /// <summary>
        /// Removes the given item from the cache. If no item exists with that key, this method does nothing.
        /// </summary>
        public void Remove(string key)
        {
            this.cache.Remove(key);
        }

        /// <summary>
        /// Returns the value associated with the given key.
        /// </summary>
        public object Get(string key)
        {
            return this.cache.GetData(key);
        }

        /// <summary>
        /// Clear Cache
        /// </summary>
        public void Clear()
        {
            this.cache.Flush();
        }
    }
}
