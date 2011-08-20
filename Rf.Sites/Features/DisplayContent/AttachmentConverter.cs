using System;
using Rf.Sites.Entities;
using Rf.Sites.Frame;

namespace Rf.Sites.Features.DisplayContent
{
    public class AttachmentConverter : IObjectConverter<Attachment, AttachmentVM>
    {
        private readonly MediaSettings _mediaSettings;

        public AttachmentConverter(MediaSettings mediaSettings)
        {
            _mediaSettings = mediaSettings;
        }

        public AttachmentVM Convert(Attachment @in)
        {
            return new AttachmentVM(@in) { DropZone = _mediaSettings.AttachmentDropZone };
        }
    }
}