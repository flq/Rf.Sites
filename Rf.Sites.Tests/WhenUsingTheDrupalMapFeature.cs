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
                                    new MapEntry {DrupalUrl = "node/1", RfSiteUrl = "/Content/Entry/4"}
                                  }
    };

    [Test]
    public void DrupalMapIsCorrectlyDeserialized()
    {
      string xml =
      @"<DrupalUrlMap><Entry DrupalUrl=""node/1"" RfSiteUrl=""/Content/Entry/4"" />
      <Entry DrupalUrl=""blog/1/feed"" RfSiteUrl=""/rss"" /></DrupalUrlMap>";

      var s = new XmlSerializer(typeof(DrupalUrlMap));
      var d = (DrupalUrlMap)s.Deserialize(new StringReader(xml));
      d.Entries.ShouldHaveCount(2);
      d["blog/1/feed"].ShouldBeEqualTo("/rss");
    }

    [Test]
    public void DrupalMapReturnsCorrectValue()
    {
      var ret = drupalMap["node/1"];
      ret.ShouldNotBeNull();
      ret.ShouldBeEqualTo("/Content/Entry/4");
    }

    [Test]
    public void NotExistingMapReturnsNull()
    {
      var ret = drupalMap["32"];
      ret.ShouldBeNull();
    }
  }
}