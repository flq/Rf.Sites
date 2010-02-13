using System;

namespace Rf.Sites.Frame
{
  [AttributeUsage(AttributeTargets.Class)]
  public class OrderAttribute : Attribute, IComparable<OrderAttribute>
  {
    public int OrderKey { get; set; }

    public OrderAttribute(int orderKey)
    {
      OrderKey = orderKey;
    }

    public int CompareTo(OrderAttribute other)
    {
      return OrderKey.CompareTo(other.OrderKey);
    }
  }
}