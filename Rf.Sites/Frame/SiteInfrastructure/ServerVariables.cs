using System.Collections.Specialized;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class ServerVariables
    {
        public const string URL = "URL";
        public const string SERVER_NAME = "SERVER_NAME";
        public const string SERVER_PORT = "SERVER_PORT";

        private readonly NameValueCollection _serverVariables;
        

        public ServerVariables(NameValueCollection serverVariables)
        {
            _serverVariables = serverVariables;
        }

        public string this[string key]
        {
            get { return _serverVariables[key]; }
        }
    }
}