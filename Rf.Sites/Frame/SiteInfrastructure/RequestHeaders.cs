using System;
using System.Collections.Specialized;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class RequestHeaders
    {
        private readonly NameValueCollection _headers;

        public RequestHeaders(NameValueCollection headers)
        {
            _headers = headers;
        }

        public string this[string key]
        {
            get { return _headers[key]; }
        }
    }
}