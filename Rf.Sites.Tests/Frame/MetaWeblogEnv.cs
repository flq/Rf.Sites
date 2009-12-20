using System;
using Rf.Sites.Domain;
using Rf.Sites.Domain.Frame;
using Rf.Sites.MetaWeblogApi;
using StructureMap;
using Environment=Rf.Sites.Frame.Environment;

namespace Rf.Sites.Tests.Frame
{
  public class MetaWeblogEnv : IntegrationEnv
  {
    private const string url = "http://thesite.com/";
    private const string uid = "a@b.com";
    private const string pwd = "pwd";

    private Environment env = new Environment
    {
      SiteMasterEmail = uid,
      SiteMasterPassword = pwd,
      SiteMasterWebPage = "http://lugos.com",
      ApplicationBaseUrl = new Uri(url)
    };

    public string Uid { get { return uid; }}
    public string Pwd { get { return pwd; }}
    public string Url { get { return url; }}

    public Environment Env
    {
      get { return env; }
    }

    public IMetaWeblog GetApi()
    {
      return new MetaWeblog(Env, nestedContainer ?? container);
    }
  }
}