using Rf.Sites.Frame;

namespace Rf.Sites.Features.Administration
{
    public class Content
    {
        public Content() { }
        public Content(string json) { Json = json; }

        public string Json { get; set; }

        public dynamic DynamicAccessToJson()
        {
            return DynamicJson.Parse(Json);
        }
    }
}