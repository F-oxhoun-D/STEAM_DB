using Npgsql;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STEAM_DB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ConnectionToDataBase.GetConnectionString();
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            // делаем кнопку неактивной
            gameButton.IsEnabled = false;
            // в gamesGrid выводим список игр
            gamesGrid.ItemsSource = ProcessRequest.GetListOfGamess();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new ();
            signUpWindow.ShowDialog();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            SignInWindow signInWindow = new ();
            signInWindow.ShowDialog();
        }
    }
}