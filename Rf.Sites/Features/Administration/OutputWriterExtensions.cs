using System.Web;
using FubuMVC.Core.Runtime;

namespace Rf.Sites.Features.Administration
{
    /// <summary>
    /// Copied from http://www.beingnew.net/2011/09/how-i-permanently-redirect-using.html
    /// </summary>
    public static class OutputWriterExtensions
    {
        public static void WriteHeader(this IOutputWriter outputWriter,
                        string headerName, string headerValue)
        {
            HttpContext.Current.Response.AppendHeader(headerName, headerValue);
        }
    }
}