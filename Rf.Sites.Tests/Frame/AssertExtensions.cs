using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Rf.Sites.Tests.Frame
{
  public static class AssertExtensions
  {
    public static void ShouldHaveCount(this ICollection collection, int expectedCount)
    {
      Assert.AreEqual(
        expectedCount,
        collection.Count,
        "collection should have {0} elements but has {1}", expectedCount, collection.Count);
    }

    public static void ShouldHaveMoreItemsThan(this ICollection collection, int smallestCount)
    {
      Assert.IsTrue(collection.Count > smallestCount, "collection should more than {0} elements but has {1}", smallestCount, collection.Count);
    }

    //public static void ShouldHaveCount<T>(this IList<T> collection, int expectedCount)
    //{
    //  Assert.AreEqual(
    //    expectedCount,
    //    collection.Count,
    //    "collection should have {0} elements but has {1}", expectedCount, collection.Count);
    //}

    public static void ShouldBeGreaterThan<T>(this T target, T other) where T : IComparable<T>
    {
      Assert.IsTrue(target.CompareTo(other) > 0, "{0} is not bigger than {1}", target, other);
    }

    public static void ShouldHaveLength(this Array array, int expectedCount)
    {
      Assert.AreEqual(
        expectedCount,
        array.Length,
        "Array should have {0} elements but has {1}", array.Length, expectedCount);
    }

    public static void ShouldBeEqualTo<T>(this T target, T expectedValue)
    {
      Assert.AreEqual(expectedValue, target);
    }

    public static void ShouldAllBeEqualTo<T>(this IEnumerable<T> target, T value)
    {
      var badOnes = target.Where(v => !value.Equals(v)).Select((_, i) => i.ToString()).ToArray();
      if (badOnes.Length == 0) return;
      
      string s = string.Join(",", badOnes);
      Assert.Fail("Elements {0} are not equal to {1}", s, value);
    }

    public static void ShouldBeASubsetOf<T>(this IEnumerable<T> target, IEnumerable<T> expectedSet)
    {
      Assert.IsTrue(target.All(t => expectedSet.Contains(t)), "Some elements are not inside the expected set");
    }

    public static void ShouldBeTheSameAs<T>(this T target, T expectedValue)
    {
      Assert.AreSame(expectedValue, target);
    }


    public static void ShouldBeTrue(this bool target)
    {
      Assert.IsTrue(target);
    }

    public static void ShouldBeFalse(this bool target)
    {
      Assert.IsFalse(target);
    }

    public static void ShouldContain<T>(this IEnumerable<T> target, Func<T,bool> predicate)
    {
      Assert.IsTrue(target.Any(predicate));
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> target, Func<T, bool> predicate)
    {
      Assert.IsFalse(target.Any(predicate), "The checked element does contain what is looked for");
    }

    public static void ShouldNotBeNull<T>(this T target) where T : class
    {
      Assert.IsNotNull(target);
    }

    public static void ShouldBeNull<T>(this T target) where T : class
    {
      Assert.IsNull(target);
    }

    public static void ShouldBeOfType<T>(this object target)
    {
      Assert.IsInstanceOfType(typeof(T), target);
    }

  }
}