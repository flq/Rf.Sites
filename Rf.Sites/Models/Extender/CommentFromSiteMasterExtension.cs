using System;
using Rf.Sites.Frame;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Models.Extender
{
  public class CommentFromSiteMasterExtension : IVmExtender<CommentVM>
  {
    private readonly Environment env;
    private static readonly Random r = new Random();

    private readonly string[] siteMasterTitles =
      new string[]
        {
          "The one who runs this bin",
          "Le grandseigneur du le Site",
          "The local Borg outlet"
        };

    public CommentFromSiteMasterExtension(Environment env)
    {
      this.env = env;
    }

    public void Inspect(CommentVM viewModel)
    {
      if (!viewModel.IsFromSiteMaster)
        return;

      viewModel.CreateGravatarImageSource(env.SiteMasterEmail);

      viewModel.Name = siteMasterTitles[r.Next(0, siteMasterTitles.Length - 1)];
      viewModel.Website = env.SiteMasterWebPage;
    }
  }
}