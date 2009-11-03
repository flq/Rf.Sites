using Rf.Sites.Frame;

namespace Rf.Sites.Models.Extender
{
  public class AttachmentPathModifier : IVmExtender<AttachmentVM>
  {
    private readonly string dropzoneUrl;

    public AttachmentPathModifier(Environment environment)
    {
      dropzoneUrl = environment.DropZoneUrl;
    }

    public void Inspect(AttachmentVM viewModel)
    {
      var possibleSlash = viewModel.Path.StartsWith("/") ? "" : "/";
      int urlIndex = viewModel.Path.IndexOf(dropzoneUrl);

      if (urlIndex == -1 || urlIndex > 1)
        viewModel.Path = string.Concat("/" + dropzoneUrl + possibleSlash, viewModel.Path);
      else
        viewModel.Path = string.Concat(possibleSlash, viewModel.Path);
    }
  }
}