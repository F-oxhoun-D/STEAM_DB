using System.Windows;
using System.Windows.Media;

namespace STEAM_DB
{
    /// <summary>
    /// Логика взаимодействия для SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        static bool CheckValidUserName = false;
        static bool CheckValidEmail = false;
        static bool CheckValidPassword = false;
        static readonly string[] sep = [" ", ",", "/"];
        static string userName = string.Empty;
        static string email = string.Empty;
        static string password = string.Empty;
        static readonly string errorMessage = "Заполните поля!";

        public SignUpWindow()
        {
            InitializeComponent();
            Clear();
        }

        private static void Clear()
        {
            CheckValidUserName = false;
            CheckValidEmail = false;
            CheckValidPassword = false;
            userName = string.Empty;
            email = string.Empty;
            password = string.Empty;
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidUserName && CheckValidEmail && CheckValidEmail)
            {
                bool checkUserName = Authorization.CheckUserName(userName);
                if (!checkUserName)
                {
                    bool checkEmail = Authorization.CheckEmail(email);
                    if (!checkEmail)
                    {
                        // поучаем хеш пароля
                        string passwordHash = Hash.GetHash(password);
                        // сегодняшняя дата
                        DateTime date = DateTime.Today;
                        // конвертируем в строку вида yyyy-MM-dd
                        string dateString = date.ToString("yyyy-MM-dd");
                        // передача параметров в метод AddUserInDB класса AuthorizationDB
                        Authorization.AddUserInDB(userName, email, passwordHash, dateString);
                        MessageBox.Show("Регистрация прошла успешно!");
                        // закрываем окно регистрации
                        Close();
                        // получаем пользователя, под которым входим в систему
                        Authentication.GetUser(userName, passwordHash);
                        // открываем окно личного кабинета
                        PersonalAccountWindow window = new();
                        window.ShowDialog();
                    }
                    else MessageBox.Show("Данная почта уже существует!");
                }
                else MessageBox.Show("Имя занято!");
            }
            else
            {
                if (userName == string.Empty)
                    textBoxUserName.BorderBrush = Brushes.Red;
                if (email == string.Empty)
                    textBoxEmail.BorderBrush = Brushes.Red;
                if (password == string.Empty)
                    passwordBox.BorderBrush = Brushes.Red;
                MessageBox.Show(errorMessage);
            }
        }

        private void TextBoxUserName_LostFocus(object sender, RoutedEventArgs e) // возникает при окончании ввода текста
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

        private void TextBoxEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            email = string.Join("", textBoxEmail.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries));
            bool checkValidEmail = Authorization.ValidEmail(email);
            if (checkValidEmail)
            {
                textBoxEmail.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x4C, 0x4C, 0x72));
                if (!CheckValidEmail) CheckValidEmail = true;
            }
            else
            {
                textBoxEmail.BorderBrush = Brushes.Red;
                MessageBox.Show("Неверный формат почты!");
                if (CheckValidEmail) CheckValidEmail = false;
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
