using System.Windows;

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
            gamesGrid.ItemsSource = ProcessRequest.GetListOfGames();

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