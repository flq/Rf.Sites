using System.IO;

namespace Rf.Sites.Test.Support
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

        public static string MarkdownFullHeader()
        {
            return getFile("markdown_fullheader.txt");
        }

        public static string Markdown2()
        {
            return getFile("markdown2.txt");
        }

        public static string Markdown3()
        {
            return getFile("markdown3.txt");
        }

        public static string Markdown4()
        {
            return getFile("markdown4.txt");
        }

        private static string getFile(string file)
        {
            var stream = typeof(DataMother).Assembly
              .GetManifestResourceStream(typeof(DataMother), file);
            var sr = new StreamReader(stream);

            return sr.ReadToEnd();
        }
    }
}