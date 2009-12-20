using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Rf.Sites.Frame
{
  public class EnvironmentConfigurationSection : IConfigurationSectionHandler
  {
    public object Create(object parent, object configContext, XmlNode section)
    {
      var stream = new MemoryStream();
      XmlWriter w = XmlWriter.Create(stream);
      section.WriteTo(w);
      w.Close();
      stream.Seek(0, SeekOrigin.Begin);
      XmlReader r = XmlReader.Create(stream);
      XmlSerializer s = new XmlSerializer(typeof(Environment));
      return s.Deserialize(r);
    }
  }
}