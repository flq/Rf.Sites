using System;
using System.Collections.Generic;
using System.Linq;

namespace Rf.Sites.Domain.Frame
{
  /// <summary>
  /// Factory for Paginators.
  /// </summary>
  public class Paginator
  {
    private static ICache cache = new NullCache();

    /// <summary>
    /// Set an <see cref="ICache"/> instance if you want to use count caching
    /// to avoid count selects. Default is a <see cref="NullCache"/> which 
    /// essentially means that no caching occurs.
    /// </summary>
    public static void SetPaginatorCountCache(ICache cache)
    {
      Paginator.cache = cache;
    }

    public static PaginatorArguments<A> For<A>(IQueryable<A> data)
    {
      return new PaginatorArguments<A>().SetCache(cache).SetData(data);
    }
  }

  public class PaginatorArguments<T>
  {
    public string CacheKey { get; private set; }
    public int PageToLoad { get; private set; }
    public int PageSize { get; private set; }
    public IQueryable<T> Data { get; private set; }

    internal ICache Cache { get; private set; }

    public PaginatorArguments()
    {
      CacheKey = null;
      PageToLoad = 0;
      PageSize = 5;
    }

    public PaginatorArguments<T> SetCacheKey(string cacheKey)
    {
      CacheKey = cacheKey;
      return this;
    }

    public PaginatorArguments<T> SetPageToLoad(int pageToLoad)
    {
      PageToLoad = pageToLoad;
      return this;
    }

    public PaginatorArguments<T> SetPageSize(int pageSize)
    {
      PageSize = pageSize;
      return this;
    }

    public PaginatorArguments<T> SetData(IQueryable<T> data)
    {
      Data = data;
      return this;
    }

    internal PaginatorArguments<T> SetCache(ICache cache)
    {
      Cache = cache;
      return this;
    }

    public Paginator<T> Get()
    {
      return new Paginator<T>(this);
    }
  }

  /// <summary>
  /// A paginator for a queryable entity. Use <see cref="Paginator"/>
  /// to construct if you have e.g. a T of an anonymous type.
  /// </summary>
  public class Paginator<T>
  {
    private readonly PaginatorArguments<T> args;
    private readonly int elementCount;

    public Paginator(PaginatorArguments<T> args)
    {
      this.args = args;
      var cache = args.Cache;
      
      if (args.CacheKey != null && cache.HasValue(args.CacheKey))
        elementCount = cache.Get<int>(args.CacheKey);
      else
      {
        elementCount = args.Data.Count();
        cache.Add(args.CacheKey, elementCount);
      }
    }

    public int PageSize
    {
      get { return args.PageSize; }
    }

    public int PageCount
    {
      get
      {
        return (int)Math.Ceiling((double)elementCount / args.PageSize);
      }
    }

    public int Page
    {
      get { return args.PageToLoad; }
    }



    public IEnumerable<T> GetPage()
    {
      var ret = args.Data.Skip(Page*PageSize).Take(PageSize);
      return ret;
    }
  }
}