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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STEAM_DB
{
    /// <summary>
    /// Логика взаимодействия для PageWishlist.xaml
    /// </summary>
    public partial class PageWishlist : Page
    {
        public PageWishlist()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            List<string> list = ProcessRequest.GetListOfWishlist(Global.user.UserId);
            foreach (var m in list)
                listBoxWishlist.Items.Add(m);
        }

        private void ButtonBuyGame_Click(object sender, RoutedEventArgs e)
        {
            PersonalAccountWindow.listPage[PersonalAccountWindow.indxGame] = new PageGame();
        }

        private void ButtonRemoveGame_Click(object sender, RoutedEventArgs e)
        {
            PersonalAccountWindow.listPage[PersonalAccountWindow.indxWishlist] = new PageWishlist();
        }
    }
}
