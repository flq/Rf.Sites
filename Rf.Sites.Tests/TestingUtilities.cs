using System.Collections.Generic;
using NUnit.Framework;
using Rf.Sites.Actions.Args;
using Rf.Sites.Frame;
using Rf.Sites.Models;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class TestingUtilities
  {

    [Test]
    public void ArgsAreConvertibleToDictionaries()
    {
      var args = ArgsFrom.Month(2007, 4, 5);
      var dict = args.ToDictionary();
      dict["val1"].ShouldBeEqualTo(2007);
      dict["val2"].ShouldBeEqualTo(4);
      dict["val3"].ShouldBeEqualTo(5);
    }

    [Test]
    public void ApplyDoesNotFailOnNullList()
    {
      IEnumerable<IVmExtender<object>> e = null;
      var cvm = new object();
      e.Apply(cvm);
    }
  }
}