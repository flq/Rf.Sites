using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;
using Rf.Sites.Frame;
using Rf.Sites.Tests.Frame;

namespace Rf.Sites.Tests
{
  [TestFixture]
  public class WhenUsingTheDrupalMapFeature
  {
    private readonly DrupalUrlMap drupalMap = new DrupalUrlMap
    {
      Entries = new List<MapEntry>
                                  {
                                    new MapEntry {DrupalId = 1, RfSiteId = 2}
                                  }
    };

    [Test]
    public void DrupalMapIsCorrectlyDeserialized()
    {
      string xml =
      @"<DrupalUrlMap><Entry DrupalId=""1"" RfSiteId=""3"" />
      <Entry DrupalId=""2"" RfSiteId=""4"" /></DrupalUrlMap>";

      var s = new XmlSerializer(typeof(DrupalUrlMap));
      var d = (DrupalUrlMap)s.Deserialize(new StringReader(xml));
      d.Entries.ShouldHaveCount(2);
      d["2"].ShouldBeOfType<int>();
      ((int)d["2"]).ShouldBeEqualTo(4);
    }

    [Test]
    public void DrupalMapReturnsCorrectValue()
    {
      var ret = drupalMap["1"];
      ret.ShouldNotBeNull();
      ret.ShouldBeOfType<int>();
      ((int)ret).ShouldBeEqualTo(2);
    }

    [Test]
    public void DrupalMapRemovesNodePrefix()
    {
      var ret = drupalMap["node/1"];
      ret.ShouldNotBeNull();
      ret.ShouldBeOfType<int>();
      ((int)ret).ShouldBeEqualTo(2);
    }

    [Test]
    public void NotExistingMapReturnsNull()
    {
      var ret = drupalMap["32"];
      ret.ShouldBeNull();
    }

    [Test]
    public void NonNumericDrupalIdReturnsNull()
    {
      var ret = drupalMap["hello"];
      ret.ShouldBeNull();
    }
  }
}