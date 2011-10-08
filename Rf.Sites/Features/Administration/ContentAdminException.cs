using System;

namespace Rf.Sites.Features.Administration
{
    public class ContentAdminException : Exception
    {

        public ContentAdminException(string message) : base(message)
        {
        }

        public ContentAdminException(string message, Exception inner) : base(message, inner)
        {
        }   
    }
}