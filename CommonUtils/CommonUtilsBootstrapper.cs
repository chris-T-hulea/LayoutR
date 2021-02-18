using Unity;
using CommonUtils.Interfaces;

namespace CommonUtils
{
  public class CommonUtilsBootstrapper : BaseBootstrapper
  {

    public CommonUtilsBootstrapper(IUnityContainer container) : base(container)
    {

    }

    public override void Run()
    {
      container.RegisterType<IConstants, Constants>();
    }
  }
}
