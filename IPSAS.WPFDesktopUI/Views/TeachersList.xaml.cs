using System.Windows;
using System;
using IPSAS.WPFDesktopUI.ViewModels;
using IPSAS.Persistence;

namespace IPSAS.WPFDesktopUI.Views
{
    /// <summary>
    /// Interaction logic for TeachersList.xaml
    /// </summary>
    public partial class TeachersList : Window, IContextChanged
    {
        private AddTeacher _addTeacherWindow;
        private EditTeacher _editTeacherWindow;
        private TeachersListViewModel _teachersListVM;
        private Payroll _payrollWindow;
        private IPSASDbContext _ipsasDbContext;

        public TeachersList(IPSASDbContext dbContext, Payroll payrollWindow, TeachersListViewModel teachersListView)
        {
            InitializeComponent();
            _ipsasDbContext = dbContext;
            _teachersListVM = teachersListView;
            _payrollWindow = payrollWindow;


            DataContext = _teachersListVM;
        }

        private void addTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_addTeacherWindow == null)
            {
                _addTeacherWindow = new AddTeacher(_ipsasDbContext);
            }
            _addTeacherWindow.Show();
            _addTeacherWindow.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Voulez vous vraiment quitter?", "Fermer L'application", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            e.Cancel = result == MessageBoxResult.Cancel;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Environment.Exit(0);
        }

        private void teachersDG_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {

        }

        private void FichePointageClick(object sender, RoutedEventArgs e)
        {
            //Messenger.Default.Send(new TeacherAddedMessage()); // Send new teacher to Payroll and TeacherList ViewModels

            _payrollWindow.Show();
            _payrollWindow.Focus();
        }

        private void editTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_teachersListVM.SelectedTeacher == null)
            {
                MessageBox.Show("Veuillez sélectionner un enseignant");
                return;
            }
            _editTeacherWindow = new EditTeacher(_teachersListVM.SelectedTeacher.Id);
            
            _editTeacherWindow.Show();
            _editTeacherWindow.Focus();
        }

        public void RefreshContext(object t = null)
        {
            _teachersListVM.LoadTeachers();
        }
    }
}
