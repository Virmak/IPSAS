using IPSAS.Persistence;
using IPSAS.WPFDesktopUI.Messages;
using IPSAS.WPFDesktopUI.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace IPSAS.WPFDesktopUI.Views
{
    /// <summary>
    /// Interaction logic for Pointage.xaml
    /// </summary>
    public partial class Payroll : Window
    {
        private PayrollViewModel payrollViewModel;
        public ObservableCollection<string> Years { get; set; }
        public Payroll(PayrollViewModel payrollViewModel)
        {
            InitializeComponent();
            DataContext = payrollViewModel;
            init();


            //Messenger.Default.Register<PayrollRecordChanged>(this, message =>
            //{
            //    payrollViewModel.PayrollRecords.First(pr => pr.Id == message.Record.Id).HoursCount = message.Record.HoursCount;
            //});
        }
        
        private void init()
        {
            Years = new ObservableCollection<string>();
            for (var i = 2019; i < DateTime.Today.Year +1; i++)
            {
                Years.Add(i + " - " + i + 1);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void teachersDG_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as PayrollViewModel).SaveChanges();
        }
    }
}
