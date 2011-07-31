using System.Text.RegularExpressions;

namespace Rf.Sites.Frame
{
  public class TagRemover
  {
    private readonly int sizeOfText;
    private readonly bool replaceForTeaser;
    private static readonly Regex stripHtmlRegex;

    static TagRemover()
    {
      stripHtmlRegex = 
        new Regex(@"</?\w+(\s*[\w:]+\s*=\s*(""[^""]*""|'[^']*'))*\s*/?>", 
        RegexOptions.Multiline | RegexOptions.Compiled);
    }

    public TagRemover() : this(int.MaxValue)
    {
      replaceForTeaser = false;
    }

    public TagRemover(int sizeOfText)
    {
      this.sizeOfText = sizeOfText;
      replaceForTeaser = true;
    }

    public string Process(string text)
    {
      if (text.Length < sizeOfText && replaceForTeaser)
        return text;

      string subText = stripHtmlRegex.Replace(text, " ");

      string result = subText.Length > sizeOfText ? subText.Substring(0, sizeOfText) : subText;

      return result;
    }
  }

}