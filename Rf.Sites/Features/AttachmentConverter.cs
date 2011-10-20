using Rf.Sites.Entities;
using Rf.Sites.Features.Models;
using Rf.Sites.Frame;

namespace Rf.Sites.Features
{
    public class AttachmentConverter : IObjectConverter<Attachment, AttachmentVM>
    {
        private readonly SiteSettings _siteSettings;

        public AttachmentConverter(SiteSettings siteSettings)
        {
            _siteSettings = siteSettings;
        }

        public AttachmentVM Convert(Attachment @in)
        {
            return new AttachmentVM(@in) { DropZone = _siteSettings.AttachmentDropZone };
        }
    }
}