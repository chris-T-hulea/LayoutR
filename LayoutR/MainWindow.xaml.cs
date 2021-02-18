using System.Windows;
using Unity;

namespace LayoutR
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      var container = new UnityContainer();

      this.RunBootstrapper(container);

      this.DataContext = container.Resolve<MainWindowViewModel>();
      this.InitializeComponent();
    }

    private void RunBootstrapper(Unity.UnityContainer container)
    {
      container.RegisterInstance<IUnityContainer>(container);
      container.Resolve<MainBootstrapper>().Run();
    }
  }
}
