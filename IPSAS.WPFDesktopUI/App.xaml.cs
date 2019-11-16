using IPSAS.Application.UseCases;
using IPSAS.Domain.Entities;
using IPSAS.Persistence;
using IPSAS.WPFDesktopUI.ViewModels;
using IPSAS.WPFDesktopUI.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace IPSAS.WPFDesktopUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static ServiceProvider ServiceProvider;
        public static PayrollViewModel PayrollViewModel;
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();
            services.AddDbContext<IPSASDbContext>();

            services.AddScoped<TeachersListViewModel>();
            services.AddScoped<PayrollViewModel>();
            services.AddScoped<PayslipViewModel>();
            services.AddScoped<SettingsViewModel>();
            services.AddScoped<TeachersList>();
            services.AddScoped<Payroll>();
            services.AddScoped<Views.Payslip>();
            services.AddScoped<SettingsWindow>();

            ServiceProvider = services.BuildServiceProvider();

            PayrollViewModel = ServiceProvider.GetService<PayrollViewModel>();

            var teachersListWindow = ServiceProvider.GetService<TeachersList>();
            teachersListWindow.Show();

        }
    }
}
