using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Obscura {

    public class Cache {
        public const string COMPONENT_KEY_FORMAT = "__obscura_component_{0}_{1}";
        public const string COMPONENT_MASTER_KEY = "__MASTER_KEY_LIST__";
        private const int CACHE_WAIT_INTERVAL = 500;

        /// <summary>
        /// The keys under which the Obscura stores things in the Cache
        /// </summary>
        public struct CACHE_KEYS {
            public const string ComponentList = "__obscura_ComponentList";
            public const string LastUpdated = "__obscura_ServicesLastUpdated";
        }

        public enum Component{
            Images,
            ImageThumbnails
        }

        /// <summary>
        /// Dumps the contents of the cache to a string
        /// </summary>
        /// <returns>A string dump of the cache</returns>
        public static string Dump() {
            //TODO: dump cache
            return null;
        }

        /// <summary>
        /// Inserts an object into the cache with the highest priority and time
        /// </summary>
        /// <param name="key">The key to insert with</param>
        /// <param name="value">The object to insert</param>
        private static void InsertIntoCache(string key, object value) {
            HttpContext.Current.Cache.Insert(key, value, null, DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }

        #region component methods

        /// <summary>
        /// Add a component's value to the specified cache
        /// </summary>
        /// <param name="component">the component adding the value</param>
        /// <param name="key">the key to add with</param>
        /// <param name="value">the value to add</param>
        public static void Add(Component component, string key, object value) {
            System.Web.Caching.Cache cache = HttpContext.Current.Cache;
            string masterkey = BuildComponentKey(component, COMPONENT_MASTER_KEY);
            string cachekey = BuildComponentKey(component, key);

            List<string> keys = (List<string>)cache[masterkey];

            //add value key to key list
            keys.Add(key);
            cache[masterkey] = keys;

            //insert value
            InsertIntoCache(cachekey, value);
        }

        /// <summary>
        /// Get a component's value from the cache
        /// </summary>
        /// <param name="component">the component adding the value</param>
        /// <param name="key">the key to get</param>
        /// <returns>the value, or null if not found</returns>
        public static object Get(Component component, string key) {
            System.Web.Caching.Cache cache = HttpContext.Current.Cache;
            string cachekey = BuildComponentKey(component, key);

            if (cache[cachekey] != null)
                return cache[cachekey];
            else
                return null;
        }

        /// <summary>
        /// Clears all keys associated with the component
        /// </summary>
        /// <param name="component">the component to clear</param>
        public static void ClearComponent(Component component) {
            System.Web.Caching.Cache cache = HttpContext.Current.Cache;
            string masterkey = BuildComponentKey(component, COMPONENT_MASTER_KEY);

            if (cache[masterkey] != null) {
                List<string> keys = (List<string>)cache[masterkey];
                foreach (string key in keys)
                    cache.Remove(BuildComponentKey(component, key));

                cache.Remove(masterkey);
            }
        }

        /// <summary>
        /// Builds the key the component's value is stored under
        /// </summary>
        /// <param name="component">the component</param>
        /// <param name="key">the key</param>
        /// <returns>the cache key for the component/value</returns>
        public static string BuildComponentKey(Component component, string key) {
            return string.Format(COMPONENT_KEY_FORMAT, component.ToString(), key);
        }

        #endregion
    }
}
