using System.IO;

namespace Rf.Sites.Tests.Support
{
  public class DataMother
  {
    public static string GetContentSampleBody1()
    {
      var stream = typeof (DataMother).Assembly
        .GetManifestResourceStream(typeof (DataMother), "contentmodsample1.txt");
      var sr = new StreamReader(stream);

      return sr.ReadToEnd();
    }
  }
}