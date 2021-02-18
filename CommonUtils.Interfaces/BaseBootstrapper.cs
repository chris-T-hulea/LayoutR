using Unity;

namespace CommonUtils
{
  public abstract class BaseBootstrapper
  {
    protected IUnityContainer container;

    public BaseBootstrapper(IUnityContainer container)
    {
      this.container = container;
    }

    public abstract void Run();
  }
}
