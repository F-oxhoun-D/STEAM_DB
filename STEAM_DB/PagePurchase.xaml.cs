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
    /// Логика взаимодействия для PagePurchase.xaml
    /// </summary>
    public partial class PagePurchase : Page
    {
        public PagePurchase()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            purchasesGrid.ItemsSource = ProcessRequest.GetListOfPurchase(Global.user.UserId); 
        }

        private void ButtonSellTheGame_Click(object sender, RoutedEventArgs e)
        {
            PersonalAccountWindow window = new();
            window.listPage[window.indxPurchase] = new PagePurchase();
        }
    }
}
