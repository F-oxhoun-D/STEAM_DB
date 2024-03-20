using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
            purchasesGrid.ItemsSource = ProcessRequest.GetListOfPurchases(Global.user.UserId); 
        }

        private void ButtonReturnTheGame_Click(object sender, RoutedEventArgs e)
        {
            int indx = purchasesGrid.SelectedIndex;
            if (indx != -1)
            {
                // ячейка
                var cell = new DataGridCellInfo(purchasesGrid.Items[indx], purchasesGrid.Columns[0]);
                TextBlock content = (TextBlock)cell.Column.GetCellContent(cell.Item);
                // название выбранной игры
                string gameName = content.Text;

                int gameId = -1;
                bool check = ProcessRequest.CheckToReturnTheGame(gameName, ref gameId);
                if (check)
                {
                    ProcessRequest.ReturnGame(gameId);
                    MessageBox.Show("Игра успешно возвращена");

                    PersonalAccountWindow.listPage[PersonalAccountWindow.indxPurchase] = new PagePurchase();
                    Load();
                }
                else
                    MessageBox.Show("Нельзя вернуть игру (прошло более двух недель)", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            else
                MessageBox.Show("Выберите игру");
            
        }
    }
}
