using System.Text.RegularExpressions;

namespace Rf.Sites.Domain.Frame
{
  public class TeaserGenerator
  {
    private readonly int sizeOfText;
    private static readonly Regex stripHtmlRegex;

    static TeaserGenerator()
    {
      stripHtmlRegex = 
        new Regex(@"</?\w+(\s*[\w:]+\s*=\s*(""[^""]*""|'[^']*'))*\s*/?>", 
        RegexOptions.Multiline | RegexOptions.Compiled);
    }

    public TeaserGenerator(int sizeOfText)
    {
      this.sizeOfText = sizeOfText;
    }

    public string Process(string text)
    {
      if (text.Length < sizeOfText)
        return text;

      string subText = stripHtmlRegex.Replace(text, " ");

      string result = subText.Length > sizeOfText ? subText.Substring(0, sizeOfText) : subText;

      return result;
    }
  }

}