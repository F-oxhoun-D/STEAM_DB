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
            string errorMessage = "Заполните поля!";
            if (textBoxUserName.Text != null)
            {
                if (textBoxEmail.Text != null)
                {
                    if (passwordBox.Password.ToString() != null)
                    {
                        bool result = Authorization.QueryCheckAvailability(textBoxEmail.Text);
                        if (!result)
                        {
                            // поучаем хеш пароля
                            string passwordHash = Hash.GetHash(passwordBox.Password.ToString());
                            // сегодняшняя дата
                            DateTime date = DateTime.Today;
                            // конвертируем в строку вида yyyy-MM-dd
                            string dateString = date.ToString("yyyy-MM-dd");
                            // передача параметров в метод AddUserInDB класса AuthorizationDB
                            Authorization.AddUserInDB(textBoxUserName.Text, textBoxEmail.Text, passwordHash, dateString);
                            MessageBox.Show("Регистрация прошла успешно!");
                            // закрываем окно регистрации
                            Close();
                            // получаем пользователя, под которым входим в систему
                            Authentication.GetUser(textBoxUserName.Text, passwordHash);
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
