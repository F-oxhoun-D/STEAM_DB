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
                        bool result = AuthorizationDB.QueryCheckAvailability(textBoxEmail.Text);
                        if (!result)
                        {
                            string passwordHash = Hash.GetHash(passwordBox.Password.ToString());
                            DateTime date = DateTime.Today;
                            string dateString = date.ToString("yyyy-MM-dd");
                            AuthorizationDB.AddUserInDB(textBoxUserName.Text, textBoxEmail.Text, passwordHash, dateString);
                            MessageBox.Show("Регистрация прошла успешно!");
                            Close();
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
