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
            int indx = listBoxWishlist.SelectedIndex;
            if (indx != -1)
            {
                string? gameName = listBoxWishlist.Items[indx].ToString();
                if (gameName != null)
                {
                    int gameId = -1;
                    bool check = ProcessRequest.CheckThePurchase(gameName, ref gameId);
                    if (!check)
                    {
                        ProcessRequest.BuyGame(gameId);
                        MessageBox.Show("Поздравляем с покупкой");

                        PersonalAccountWindow.listPage[PersonalAccountWindow.indxPurchase] = new PagePurchase();
                    }
                    else
                        MessageBox.Show("Данная игра уже куплена Вами");
                }
                else
                    MessageBox.Show("Выберите игру");
            }
        }

        private void ButtonRemoveGame_Click(object sender, RoutedEventArgs e)
        {
            int indx = listBoxWishlist.SelectedIndex;
            if (indx != -1)
            {
                string? gameName = listBoxWishlist.Items[indx].ToString();
                if (gameName != null)
                {
                    int gameId = -1;
                    ProcessRequest.RemoveFromWishlist(gameName, ref gameId);

                    PersonalAccountWindow.listPage[PersonalAccountWindow.indxWishlist] = new PageWishlist();
                    listBoxWishlist.Items.Clear();
                    listBoxWishlist.Items.Refresh();
                    Load();
                }
                else
                    MessageBox.Show("Выберите игру");
            }
        }
    }
}
