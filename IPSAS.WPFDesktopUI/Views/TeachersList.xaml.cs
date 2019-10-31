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
        private TeachersListViewModel _dataContext;
        private Pointage _pointangeWindow = new Pointage();
        private IPSASDbContext _ipsasDbContext;

        public TeachersList()
        {
            InitializeComponent();
            _ipsasDbContext = new IPSASDbContext();
            _dataContext = new TeachersListViewModel(_ipsasDbContext);
            DataContext = _dataContext;

        }

        private void addTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_addTeacherWindow == null)
            {
                _addTeacherWindow = new AddTeacher(_ipsasDbContext, this, _pointangeWindow);
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

            if (_pointangeWindow == null)
            {
                _pointangeWindow = new Pointage();
            }
            _pointangeWindow.RefreshContext();
            _pointangeWindow.Show();
            _pointangeWindow.Focus();
        }

        private void editTeacherBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_dataContext.SelectedTeacher == null)
            {
                MessageBox.Show("Veuillez sélectionner un enseignant");
                return;
            }
            _editTeacherWindow = new EditTeacher(_dataContext.SelectedTeacher.Id);
            
            _editTeacherWindow.Show();
            _editTeacherWindow.Focus();
        }

        public void RefreshContext(object t = null)
        {
            _dataContext.LoadTeachers();
        }
    }
}
