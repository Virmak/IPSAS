using IPSAS.Persistence;
using IPSAS.WPFDesktopUI.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace IPSAS.WPFDesktopUI.Views
{
    /// <summary>
    /// Interaction logic for Pointage.xaml
    /// </summary>
    public partial class Pointage : Window, IContextChanged
    {
        private PayrollViewModel pointageViewModel;
        public ObservableCollection<string> Years { get; set; }
        public Pointage()
        {
            InitializeComponent();
            var context = new IPSASDbContext();
            pointageViewModel = new PayrollViewModel(context);
            DataContext = pointageViewModel;
            init();
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

        public void RefreshContext(object t = null)
        {
            pointageViewModel.Init();
            if (t != null)
            {
                pointageViewModel.AddTeacherRecord(t.ToString());
            }
        }
    }
}
