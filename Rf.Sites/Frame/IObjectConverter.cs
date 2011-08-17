using System;

namespace Rf.Sites.Frame
{
  public interface IObjectConverter<in TIn, out TOut>
  {
    TOut Convert(TIn @in);
  }

  public static class ObjectConverter
  {
    public static ObjectConverter<TIn,TOut> From<TIn,TOut>(Func<TIn,TOut> converterFunction)
    {
      return new ObjectConverter<TIn, TOut>(converterFunction);
    }
  }

  public class ObjectConverter<TIn, TOut> : IObjectConverter<TIn, TOut>
  {
    private readonly Func<TIn, TOut> converterFunction;

    public ObjectConverter(Func<TIn, TOut> converterFunction)
    {
      this.converterFunction = converterFunction;
    }

    public TOut Convert(TIn @in)
    {
      return converterFunction(@in);
    }
  }
}