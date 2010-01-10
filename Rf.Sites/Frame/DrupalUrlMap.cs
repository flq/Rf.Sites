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

    public object this[string drupalID]
    {
      get
      {
        var id = drupalID.Replace("node/", "");
        try
        {
          var entry = Entries.Where(e => e.DrupalId == Convert.ToInt32(id)).FirstOrDefault();
          return entry != null ? (object) entry.RfSiteId : null;
        }
        catch (FormatException)
        {
          return null;
        }
      }
    }
  }

  public class MapEntry
  {
    [XmlAttribute]
    public int DrupalId { get; set; }
    [XmlAttribute]
    public int RfSiteId { get; set; }
  }
}