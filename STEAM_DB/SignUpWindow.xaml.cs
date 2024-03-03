using System.Windows;

namespace STEAM_DB
{
    /// <summary>
    /// Логика взаимодействия для SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            string[] sep = [" ", ",", ".", "/"];
            string strUserName = string.Join("", textBoxUserName.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries));
            string strEmail = string.Join("", textBoxEmail.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries));
            string strPassword = string.Join("", passwordBox.Password.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries));

            string errorMessage = "Заполните поля!";
            if (strUserName != "")
            {
                if (strEmail != "")
                {
                    if (strPassword != "")
                    {
                        bool result = Authorization.QueryCheckAvailability(strEmail);
                        if (!result)
                        {
                            // поучаем хеш пароля
                            string passwordHash = Hash.GetHash(strPassword);
                            // сегодняшняя дата
                            DateTime date = DateTime.Today;
                            // конвертируем в строку вида yyyy-MM-dd
                            string dateString = date.ToString("yyyy-MM-dd");
                            // передача параметров в метод AddUserInDB класса AuthorizationDB
                            Authorization.AddUserInDB(strUserName, strEmail, passwordHash, dateString);
                            MessageBox.Show("Регистрация прошла успешно!");
                            // закрываем окно регистрации
                            Close();
                            // получаем пользователя, под которым входим в систему
                            Authentication.GetUser(strUserName, passwordHash);
                            // открываем окно личного кабинета
                            PersonalAccountWindow window = new();
                            window.ShowDialog();
                        }
                        else
                            MessageBox.Show("Данная почта уже существует!");
                    }
                    else
                        MessageBox.Show(errorMessage);
                }
                else
                    MessageBox.Show(errorMessage);
            }
            else
                MessageBox.Show(errorMessage);
        }
    }
}
