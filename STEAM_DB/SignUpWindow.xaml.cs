using System.Windows;

namespace STEAM_DB
{
    /// <summary>
    /// Логика взаимодействия для SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        static readonly string[] sep = [" ", ",", "/"];
        static string userName = null!;
        static string email = null!;
        static string password = null!;
        static readonly string errorMessage = "Заполните поля!";

        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            userName = string.Join("", textBoxUserName.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries));
            email = string.Join("", textBoxEmail.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries));
            password = string.Join("", passwordBox.Password.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries));

            if (userName != "")
            {
                if (email != "")
                {
                    if (password != "")
                    {
                        bool checkValidEmail = Authorization.ValidEmail(email);
                        if (checkValidEmail)
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
                        else MessageBox.Show("Неверный формат почты!");
                    }
                    else MessageBox.Show(errorMessage);
                }
                else MessageBox.Show(errorMessage);
            }
            else MessageBox.Show(errorMessage);
        }
    }
}
