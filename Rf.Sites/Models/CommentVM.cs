using System;
using System.Security.Cryptography;
using System.Text;
using Rf.Sites.Domain;
using Rf.Sites.Frame;

namespace Rf.Sites.Models
{
  public class CommentVM
  {
    private const string gravatarRoot = "http://www.gravatar.com/avatar/{0}?s=80&d=identicon";
    private static readonly Random randomizer = new Random();

    public CommentVM(Comment comment) : this(comment,null) { }

    public CommentVM(Comment comment, IVmExtender<CommentVM>[] extender)
    {
      setUp(comment);
      extender.Apply(this);
    }

    public string Website { get; private set; }
    public string Name { get; private set; }
    public string GravatarImageSource { get; private set; }
    public string Body { get; set; }

    private void setUp(Comment comment)
    {
      Name = comment.CommenterName;
      Website = comment.CommenterWebsite;
      Body = comment.Body;
      createGravatarImageSource(comment.CommenterEmail);
    }

    private void createGravatarImageSource(string email)
    {
      if (string.IsNullOrEmpty(email))
        email = "anyemail" + randomizer.Next(1000);
      var md5 = MD5.Create();
      byte[] theHash = md5.ComputeHash(Encoding.ASCII.GetBytes(email));

      var builder = new StringBuilder();
      Array.ForEach(theHash, b => builder.Append(b.ToString("x2")));

      GravatarImageSource = string.Format(gravatarRoot, builder);
    }
  }
}