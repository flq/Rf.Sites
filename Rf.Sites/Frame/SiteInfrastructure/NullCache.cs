using System;
using System.Collections.Generic;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class NullCache : ICache
    {
        public void Add<T>(string key, T value)
        {
        }

        public T Get<T>(string key)
        {
            throw new NotSupportedException("Null cache does not support extracting cached values");
        }

        public bool HasValue(string key)
        {
            return false;
        }
    }

    public class InMemoryCache : ICache
    {
        private readonly Dictionary<string,object> _d = new Dictionary<string, object>();

        public void Add<T>(string key, T value)
        {
            _d.Add(key, value);
        }

        public T Get<T>(string key)
        {
            return (T)_d[key];
        }

        public bool HasValue(string key)
        {
            return _d.ContainsKey(key);
        }
    }
}