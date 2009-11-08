namespace Rf.Sites.MetaWeblogApi
{
  public interface IMediaStorage
  {
    string StoreMedia(string name, byte[] content);
  }
}