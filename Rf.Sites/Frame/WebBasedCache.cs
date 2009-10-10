using System;
using System.Web;
using System.Web.Caching;
using Rf.Sites.Domain.Frame;

namespace Rf.Sites.Frame
{
  public class WebBasedCache : ICache
  {
    private readonly Cache cache;
    private readonly TimeSpan timeOfValidity = TimeSpan.FromDays(1);

    public WebBasedCache()
    {
      cache = HttpRuntime.Cache;
    }

    public void Add<T>(string key, T value)
    {
      cache.Add(key, value,
                null,
                DateTime.Now + timeOfValidity,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Default, null);
    }

    public T Get<T>(string key)
    {
      return (T) cache.Get(key);
    }

    public bool HasValue(string key)
    {
      return cache.Get(key) != null;
    }
  }
}