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
    /// Логика взаимодействия для PersonalAccountWindow.xaml
    /// </summary>
    public partial class PersonalAccountWindow : Window
    {
        public PersonalAccountWindow()
        {
            InitializeComponent();
            Load();
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WishlistButton_Click(object sender, RoutedEventArgs e)
        {
            PersonalAccountFrame.Content = new PageWishlist();
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            PersonalAccountFrame.Content = new PageGame();
        }

        private void Load()
        {
            userLabel.Content = Global.user.Username.ToString();
        }
    }
}
