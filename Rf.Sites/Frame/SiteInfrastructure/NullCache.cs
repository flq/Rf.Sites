using System;

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
}