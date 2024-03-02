using System.Windows;
using System.Windows.Controls;

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
            // индекс выбранной строки
            int indx = gamesGrid.SelectedIndex;
            if (indx != -1)
            {
                // ячейка
                var cell = new DataGridCellInfo(gamesGrid.Items[indx], gamesGrid.Columns[0]);
                TextBlock content = (TextBlock)cell.Column.GetCellContent(cell.Item);
                // название выбранной игры
                string gameName = content.Text;

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

        private void ButtonAddToWishlist_Click(object sender, RoutedEventArgs e)
        {
            // индекс выбранной строки
            int indx = gamesGrid.SelectedIndex;
            if (indx != -1)
            {
                // ячейка
                var cell = new DataGridCellInfo(gamesGrid.Items[indx], gamesGrid.Columns[0]);
                TextBlock content = (TextBlock)cell.Column.GetCellContent(cell.Item);
                // название выбранной игры
                string gameName = content.Text;

                int gameId = -1;
                bool check = ProcessRequest.CheckTheWishlist(gameName, ref gameId);
                if (!check)
                {
                    ProcessRequest.AddToWishlist(gameId);
                    MessageBox.Show("Игра добавлена в избранное");

                    PersonalAccountWindow.listPage[PersonalAccountWindow.indxWishlist] = new PageWishlist();
                }
                else
                    MessageBox.Show("Игра уже добавлена");
            }
            else
                MessageBox.Show("Выберите игру");

        }
    }
}
