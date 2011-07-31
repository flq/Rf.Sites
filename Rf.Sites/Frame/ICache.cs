namespace Rf.Sites.Frame
{
  public interface ICache
  {
    void Add<T>(string key, T value);
    T Get<T>(string key);
    bool HasValue(string key);
  }
}