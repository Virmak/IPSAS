using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace IPSAS.WPFDesktopUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider;
        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
