namespace Rf.Sites.Frame
{
  public interface IVmExtender<T>
  {
    void Inspect(T viewModel);
  }
}