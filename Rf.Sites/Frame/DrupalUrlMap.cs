using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace Rf.Sites.Frame
{
  [XmlRoot]
  public class DrupalUrlMap
  {

    [XmlElement(ElementName = "Entry")]
    public List<MapEntry> Entries { get; set; }

    public string this[string drupalID]
    {
      get
      {
        var entry = Entries
          .Where(e => e.DrupalUrl.Equals(drupalID, StringComparison.InvariantCultureIgnoreCase))
          .FirstOrDefault();
        return entry != null ? entry.RfSiteUrl : null;
      }
    }
  }

  public class MapEntry
  {
    [XmlAttribute]
    public string DrupalUrl { get; set; }
    [XmlAttribute]
    public string RfSiteUrl { get; set; }
  }
}