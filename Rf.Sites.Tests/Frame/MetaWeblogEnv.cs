using System;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.MetaWeblogApi;
using StructureMap;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Tests.Frame
{
  public class MetaWeblogEnv
  {
    private const string url = "http://thesite.com/";
    private const string uid = "a@b.com";
    private const string pwd = "pwd";

    private readonly IContainer container = new Container();

    private Environment env = new Environment
    {
      SiteMasterEmail = uid,
      SiteMasterPassword = pwd,
      SiteMasterWebPage = "http://lugos.com",
      AbsoluteBaseUrl = new Uri(url)
    };

    public MetaWeblogEnv()
    {
      Tags =
        new TestRepository<Tag>
          {
            new Tag {Created = DateTime.Now, Name = "1", Description = "A"},
            new Tag {Created = DateTime.Now, Name = "2", Description = "B"}
          };

      container.Configure(
        c => c.ForRequestedType<IRepository<Tag>>().TheDefault.IsThis(Tags));
    }

    public string Uid { get { return uid; }}
    public string Pwd { get { return pwd; }}
    public string Url { get { return url; }}

    public IRepository<Tag> Tags { get; private set; }

    public Environment Env
    {
      get { return env; }
    }

    public IMetaWeblog GetApi()
    {
      return new MetaWeblog(Env, container);
    }

    public MetaWeblogEnv ConfigureContainer(Action<ConfigurationExpression> action)
    {
      container.Configure(action);
      return this;
    }
  }
}