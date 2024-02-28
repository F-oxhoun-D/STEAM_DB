using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STEAM_DB
{
    /// <summary>
    /// Логика взаимодействия для PageGame.xaml
    /// </summary>
    public partial class PageGame : Page
    {
        public PageGame()
        {
            InitializeComponent();
            gamesGrid.ItemsSource = ProcessRequest.GetListOfGames();
        }

        private void ButtonBuyGame_Click(object sender, RoutedEventArgs e)
        {
            PersonalAccountWindow window = new();
            window.listPage[window.indxGame] = new PageGame();
        }

        private void ButtonAddToWishlist_Click(object sender, RoutedEventArgs e)
        {
            PersonalAccountWindow window = new();
            window.listPage[window.indxWishlist] = new PageWishlist();
        }
    }
}
