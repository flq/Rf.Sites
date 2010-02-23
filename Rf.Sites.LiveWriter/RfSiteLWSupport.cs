using System;
using WindowsLive.Writer.Api;

namespace Rf.Sites.LiveWriter
{
  public class RfSiteLWSupport : SmartContentSource
  {
    public override string GeneratePublishHtml(ISmartContent content, IPublishingContext publishingContext)
    {
      throw new NotImplementedException();
    }

    public override SmartContentEditor CreateEditor(ISmartContentEditorSite editorSite)
    {
      throw new NotImplementedException();
    }
  }
}
