using IPSAS.WPFDesktopUI.ViewModels;
using System.Windows;

namespace IPSAS.WPFDesktopUI.Views
{
    /// <summary>
    /// Interaction logic for Payslip.xaml
    /// </summary>
    public partial class Payslip : Window
    {
        private readonly PayslipViewModel _payslipViewModel;
        public Payslip(PayslipViewModel payslipViewModel)
        {
            InitializeComponent();
            _payslipViewModel = payslipViewModel;
            DataContext = payslipViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
