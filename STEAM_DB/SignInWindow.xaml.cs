using System.Windows;
using System.Windows.Media;

namespace STEAM_DB
{
    /// <summary>
    /// Логика взаимодействия для SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        static bool CheckValidUserName = false;
        static bool CheckValidPassword = false;
        static readonly string[] sep = [" ", ",", "/"];
        static string userName = string.Empty;
        static string password = string.Empty;
        static readonly string errorMessage = "Заполните поля!";

        public SignInWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidUserName && CheckValidPassword)
            {
                string passwordHash = Hash.GetHash(password);
                bool result = Authentication.CheckThePassword(userName, passwordHash);
                if (result)
                {
                    MessageBox.Show("Вы успешно вошли в систему!");
                    Close();
                    Authentication.GetUser(userName, passwordHash);
                    if (userName == "admin")
                    {
                        AdminWindow adminWindow = new();
                        adminWindow.ShowDialog();
                    }
                    else
                    {
                        PersonalAccountWindow window = new();
                        window.ShowDialog();
                    }
                }
                else MessageBox.Show("Данные не верны!");
            }
            else
            {
                if (userName == string.Empty)
                    textBoxUserName.BorderBrush = Brushes.Red;
                if (password == string.Empty)
                    passwordBox.BorderBrush = Brushes.Red;
                MessageBox.Show(errorMessage);
            }
        }

        private void TextBoxUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            userName = string.Join("", textBoxUserName.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries));
            if (userName != string.Empty)
            {
                textBoxUserName.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x4C, 0x4C, 0x72));
                if (!CheckValidUserName) CheckValidUserName = true;
            }
            else
            {
                textBoxUserName.BorderBrush = Brushes.Red;
                MessageBox.Show("Заполните имя!");
                if (CheckValidUserName) CheckValidUserName = false;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            password = string.Join("", passwordBox.Password.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries));
            if (password != string.Empty)
            {
                passwordBox.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x4C, 0x4C, 0x72));
                if (!CheckValidPassword) CheckValidPassword = true;
            }
            else
            {
                passwordBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Введите пароль!");
                if (CheckValidPassword) CheckValidPassword = false;
            }
        }
    }
}
