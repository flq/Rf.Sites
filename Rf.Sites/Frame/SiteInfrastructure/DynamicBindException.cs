using System;

namespace Rf.Sites.Frame.SiteInfrastructure
{
    public class DynamicBindException : Exception
    {
        public DynamicBindException(string message) : base(message)
        {
        }
    }
}