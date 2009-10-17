using System.Web.Mvc;

namespace Rf.Sites.Frame
{
  public class CustomActionResult : ActionResult
  {
    private readonly IResponseWriter responseWriter;

    public CustomActionResult(IResponseWriter responseWriter)
    {
      this.responseWriter = responseWriter;
    }

    public override void ExecuteResult(ControllerContext context)
    {
      context.HttpContext.Response.ContentType = responseWriter.ContentType;

      using (var o = context.HttpContext.Response.Output)
      {
        responseWriter.WriteTo(o);
      }
    }
  }
}