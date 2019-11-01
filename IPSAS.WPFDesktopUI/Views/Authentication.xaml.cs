using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

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

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (userTxt.Text == "1" && passwordTxt.Password == "")
            {
                authProgress.IsIndeterminate = true;
                var teachersListWindow = App.ServiceProvider.GetService<TeachersList>();
                teachersListWindow.Show();
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
