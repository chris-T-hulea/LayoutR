using CommonUtils;
using Unity;
using WindowController;

namespace LayoutR
{
  internal class MainBootstrapper : BaseBootstrapper
  {
    public MainBootstrapper(IUnityContainer container) : base(container)
    {
    }

    public override void Run()
    {
      this.container.Resolve<CommonUtilsBootstrapper>().Run();
      this.container.Resolve<WindowControllerBootstrapper>().Run();
    }
  }
}
