using System;
using System.IO;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.MetaWeblogApi
{
  public class MediaStorage : IMediaStorage
  {
    private readonly Environment environment;

    public MediaStorage(Environment environment)
    {
      this.environment = environment;
      if (!Directory.Exists(Path.Combine(environment.BaseDirectory, environment.DropZoneUrl)))
        throw new InvalidOperationException(
          string.Format("Location '{0}' to store media does not exist", environment.DropZoneUrl));
    }

    public string StoreMedia(string name, byte[] content)
    {
      string filename = Path.Combine(environment.DropZoneUrl, Path.GetFileName(name));

      int uniqueFileCount = 0;

      while (File.Exists(filename))
      {
        uniqueFileCount++;
        filename = Path.Combine(Path.GetDirectoryName(filename),
                                Path.GetFileNameWithoutExtension(filename) + uniqueFileCount) +
                   Path.GetExtension(filename);
      }

      filename = Path.Combine(environment.BaseDirectory, filename);

      using (FileStream fs = File.Create(filename))
      {
        fs.Write(content, 0, content.Length);
      }

      return environment.DropZoneUrl + "/" + Path.GetFileName(filename);
    }
  }
}