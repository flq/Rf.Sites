using System.IO;

namespace Rf.Sites.Tests.Support
{
  public class DataMother
  {
    public static string GetContentSampleBody1()
    {
      return getFile("contentmodsample1.txt");
    }

    public static string GetContentWithBracketCode()
    {
      return getFile("postWTagCheck.txt");
    }

    public static string SecndContentWithBracketCode()
    {
      return getFile("2ndPostWTagCheck.txt");
    }

    private static string getFile(string file)
    {
      var stream = typeof (DataMother).Assembly
        .GetManifestResourceStream(typeof(DataMother), file);
      var sr = new StreamReader(stream);

      return sr.ReadToEnd();
    }
  }
}