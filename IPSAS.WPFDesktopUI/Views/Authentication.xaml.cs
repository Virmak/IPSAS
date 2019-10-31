using System.Windows;

namespace IPSAS.WPFDesktopUI.Views
{
    /// <summary>
    /// Interaction logic for Authetication.xaml
    /// </summary>
    public partial class Authentication : Window
    {
        private Window _teachersListWindow;
        public Authentication()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (userTxt.Text == "1" && passwordTxt.Password == "")
            {
                if (_teachersListWindow == null)
                {
                    _teachersListWindow = new TeachersList();
                }
                _teachersListWindow.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Le nom de l'utilisateur ou le mot de passe est incorrecte",
                    "Erreur d'authentification",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
