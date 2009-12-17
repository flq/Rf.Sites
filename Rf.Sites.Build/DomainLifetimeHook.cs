namespace Rf.Sites.Build
{
  public abstract class DomainLifetimeHook
  {
    protected object data;

    public void SetData(object data)
    {
      this.data = data;
    }

    internal abstract void Start();
    internal abstract void Stop();
  }
}