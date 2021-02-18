using CommonUtils;
using Unity;
using WindowController.Interfaces;

namespace WindowController
{
	public class WindowControllerBootstrapper : BaseBootstrapper
	{
		public WindowControllerBootstrapper(IUnityContainer container) : base(container)
		{

		}

		public override void Run()
		{
			// Register stuff
			this.container.RegisterType<IProcessService, ProcessService>();
			this.container.RegisterType<IWindowConrtoller, WindowController>();
			this.container.RegisterType<IDisplayService, DisplayService>();
		}
	}
}

