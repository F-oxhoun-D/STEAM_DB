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
        static readonly string[] sep = [" ", ",", "/"];
        static string userName = null!;
        static string password = null!;
        static readonly string errorMessage = "Заполните поля!";

        public SignInWindow()
        {
            InitializeComponent();
        }

        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            userName = string.Join("", textBoxUserName.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries));
            password = string.Join("", passwordBox.Password.ToString().Split(sep, StringSplitOptions.RemoveEmptyEntries));

            string errorMessage = "Заполните поля!";
            if (userName != "")
            {
                if (password != "")
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
                            AdminWindow adminWindow = new ();
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
                else MessageBox.Show(errorMessage);
            }
            else MessageBox.Show(errorMessage);
        }
    }
}
