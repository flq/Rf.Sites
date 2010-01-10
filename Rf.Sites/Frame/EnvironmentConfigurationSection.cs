using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Rf.Sites.Frame
{
  public class XmlSerializedConfigurationSection<T>
  {
    protected T create(object parent, object configContext, XmlNode section)
    {
      var stream = new MemoryStream();
      var w = XmlWriter.Create(stream);
      section.WriteTo(w);
      if (w != null) w.Close();
      stream.Seek(0, SeekOrigin.Begin);
      var r = XmlReader.Create(stream);
      var s = new XmlSerializer(typeof(T));
      return (T)s.Deserialize(r);
    }
  }

  public class EnvironmentConfigurationSection : XmlSerializedConfigurationSection<Environment>, IConfigurationSectionHandler
  {
    public object Create(object parent, object configContext, XmlNode section)
    {
      return create(parent, configContext, section);
    }
  }
  
  public class DrupalUrlMapConfigurationSection : XmlSerializedConfigurationSection<DrupalUrlMap>, IConfigurationSectionHandler
  {
    public object Create(object parent, object configContext, XmlNode section)
    {
      return create(parent, configContext, section);
    }
  }
}