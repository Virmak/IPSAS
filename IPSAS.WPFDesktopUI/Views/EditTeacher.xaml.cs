using IPSAS.Persistence;
using IPSAS.WPFDesktopUI.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace IPSAS.WPFDesktopUI.Views
{
    /// <summary>
    /// Interaction logic for EditTeacher.xaml
    /// </summary>
    public partial class EditTeacher : Window
    {
        private AddTeacherViewModel _addTeacherViewModel;

        public EditTeacher(int teacherId)
        {
            InitializeComponent();
            _addTeacherViewModel = new AddTeacherViewModel(new IPSASDbContext()); // add apropriate window objects 
            _addTeacherViewModel.LoadTeacher(teacherId);
            DataContext = _addTeacherViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void resetControls()
        {
            _addTeacherViewModel.CIN = "";
            _addTeacherViewModel.TeacherId = "";
            _addTeacherViewModel.FirstName = "";
            _addTeacherViewModel.LastName = "";
            _addTeacherViewModel.Address = "";
            _addTeacherViewModel.Phone = "";
            _addTeacherViewModel.InitInstitute = "";
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            resetControls();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
