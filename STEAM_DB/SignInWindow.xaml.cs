using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace STEAM_DB
{
    /// <summary>
    /// Логика взаимодействия для SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        public SignInWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "Заполните поля!";
            if (textBoxUserName.Text != null)
            {
                if (passwordBox.Password.ToString() != null)
                {
                    string password = Hash.GetHash(passwordBox.Password.ToString());
                    bool result = Authentication.CheckThePassword(textBoxUserName.Text, password);
                    if (result)
                    {
                        MessageBox.Show("Вы успешно вошли в систему!");
                    }
                    else
                        MessageBox.Show("Данные не верны!");
                }
                else
                    MessageBox.Show(errorMessage);
            }
            else
                MessageBox.Show(errorMessage);
        }
    }
}
